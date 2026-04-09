using PuzKit3D.Modules.Catalog.Application.Repositories;
using PuzKit3D.Modules.Catalog.Application.UnitOfWork;
using PuzKit3D.Modules.Catalog.Domain.Entities.Topics;
using PuzKit3D.SharedKernel.Application.Message.Command;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.Catalog.Application.UseCases.Topics.Commands.UpdateTopic;

internal sealed class UpdateTopicCommandHandler : ICommandHandler<UpdateTopicCommand>
{
    private readonly ITopicRepository _topicRepository;
    private readonly ICatalogUnitOfWork _unitOfWork;

    public UpdateTopicCommandHandler(
        ITopicRepository topicRepository,
        ICatalogUnitOfWork unitOfWork)
    {
        _topicRepository = topicRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(UpdateTopicCommand request, CancellationToken cancellationToken)
    {
        var topicId = TopicId.From(request.Id);
        var topic = await _topicRepository.GetByIdAsync(topicId, cancellationToken);

        if (topic is null)
        {
            return Result.Failure(TopicError.NotFound(request.Id));
        }

        // Check if new slug already exists (but different from current topic)
        if (topic.Slug != request.Slug)
        {
            var existingTopic = await _topicRepository.GetBySlugAsync(request.Slug, cancellationToken);
            if (existingTopic is not null)
            {
                return Result.Failure(TopicError.DuplicateSlug(request.Slug));
            }
        }

        var topicsToUpdate = new List<Topic>();
        if ((request.IsActive != topic.IsActive) && request.IsActive == false)
        {
            // Get all topics
            var allTopics = await _topicRepository.GetAllAsync(cancellationToken);

            // Build a lookup dictionary for parent-child relationships
            var lookup = allTopics
                .Where(t => t.ParentId != null)
                .GroupBy(t => t.ParentId!)
                .ToDictionary(g => g.Key, g => g.ToList());

            var stack = new Stack<Topic>();
            stack.Push(topic);
            while (stack.Count > 0)
            {
                var current = stack.Pop();
                topicsToUpdate.Add(current);
                if (lookup.TryGetValue(current.Id, out var children))
                {
                    foreach (var child in children)
                    {
                        stack.Push(child);
                    }
                }
            }
        }

        // Execute in transaction
        return await _unitOfWork.ExecuteAsync(async () =>
        {
            // Update topic
            var updateResult = topic.Update(
                request.Name,
                request.Slug,
                request.ParentId.HasValue ? TopicId.From(request.ParentId.Value) : null,
                request.FactorPercentage,
                request.Description,
                request.IsActive);

            foreach (var item in topicsToUpdate.Where(t => t.Id != topic.Id))
            {
                item.Update(
                    item.Name, 
                    item.Slug, 
                    item.ParentId, 
                    null,
                    item.Description, 
                    false);
                _topicRepository.Update(item);
            }

            if (updateResult.IsFailure)
            {
                return updateResult;
            }

            return Result.Success();
        }, cancellationToken);
    }
}
