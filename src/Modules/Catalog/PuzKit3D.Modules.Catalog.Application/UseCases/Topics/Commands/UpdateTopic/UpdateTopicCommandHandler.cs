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

        // Execute in transaction
        return await _unitOfWork.ExecuteAsync(async () =>
        {
            // Update topic
            var updateResult = topic.Update(
                request.Name,
                request.Slug,
                request.ParentId.HasValue ? TopicId.From(request.ParentId.Value) : null,
                request.Description);

            if (updateResult.IsFailure)
            {
                return updateResult;
            }

            // Update active status
            if (request.IsActive)
            {
                topic.Activate();
            }
            else
            {
                topic.Deactivate();
            }

            return Result.Success();
        }, cancellationToken);
    }
}
