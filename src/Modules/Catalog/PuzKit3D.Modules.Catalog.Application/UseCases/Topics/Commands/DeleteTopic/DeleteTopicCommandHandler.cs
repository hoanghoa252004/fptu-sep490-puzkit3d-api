using PuzKit3D.Modules.Catalog.Application.Repositories;
using PuzKit3D.Modules.Catalog.Application.UnitOfWork;
using PuzKit3D.Modules.Catalog.Domain.Entities.Topics;
using PuzKit3D.SharedKernel.Application.Message.Command;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.Catalog.Application.UseCases.Topics.Commands.DeleteTopic;

internal sealed class DeleteTopicCommandHandler : ICommandHandler<DeleteTopicCommand>
{
    private readonly ITopicRepository _topicRepository;
    private readonly ICatalogUnitOfWork _unitOfWork;

    public DeleteTopicCommandHandler(
        ITopicRepository topicRepository,
        ICatalogUnitOfWork unitOfWork)
    {
        _topicRepository = topicRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(DeleteTopicCommand request, CancellationToken cancellationToken)
    {
        var topicId = TopicId.From(request.Id);
        var topic = await _topicRepository.GetByIdAsync(topicId, cancellationToken);

        if (topic is null)
        {
            return Result.Failure(TopicError.NotFound(request.Id));
        }
        // Get all topics
        var allTopics = await _topicRepository.GetAllAsync(cancellationToken);

        // Build a lookup dictionary for parent-child relationships
        var lookup = allTopics
            .Where(t => t.ParentId != null)
            .GroupBy(t => t.ParentId!)
            .ToDictionary(g => g.Key, g => g.ToList());

        var stack = new Stack<Topic>();
        stack.Push(topic);
        var topicsToDelete = new List<Topic>();
        while (stack.Count > 0)
        {
            var current = stack.Pop();
            topicsToDelete.Add(current);
            if (lookup.TryGetValue(current.Id, out var children))
            {
                foreach (var child in children)
                {
                    stack.Push(child);
                }
            }
        }

        // Execute in transaction
        return await _unitOfWork.ExecuteAsync(async () =>
        {
            foreach (var topic in topicsToDelete)
            {
                topic.Delete();
            }
            _topicRepository.DeleteMultiple(topicsToDelete);

            return Result.Success();
        }, cancellationToken);
    }
}
