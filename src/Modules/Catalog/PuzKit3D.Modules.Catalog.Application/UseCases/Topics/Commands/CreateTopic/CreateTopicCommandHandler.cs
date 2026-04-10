using PuzKit3D.Modules.Catalog.Application.Repositories;
using PuzKit3D.Modules.Catalog.Application.UnitOfWork;
using PuzKit3D.Modules.Catalog.Domain.Entities.Topics;
using PuzKit3D.SharedKernel.Application.Message.Command;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.Catalog.Application.UseCases.Topics.Commands.CreateTopic;

internal sealed class CreateTopicCommandHandler : ICommandTHandler<CreateTopicCommand, Guid>
{
    private readonly ITopicRepository _topicRepository;
    private readonly ICatalogUnitOfWork _unitOfWork;

    public CreateTopicCommandHandler(
        ITopicRepository topicRepository,
        ICatalogUnitOfWork unitOfWork)
    {
        _topicRepository = topicRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ResultT<Guid>> Handle(CreateTopicCommand request, CancellationToken cancellationToken)
    {
        // Check if slug already exists
        var existingTopic = await _topicRepository.GetBySlugAsync(request.Slug, cancellationToken);

        if (existingTopic is not null)
        {
            return Result.Failure<Guid>(TopicError.DuplicateSlug(request.Slug));
        }

        // Execute in transaction
        return await _unitOfWork.ExecuteAsync(async () =>
        {
            // Create topic using factory method
            var topicResult = Topic.Create(
                request.Name,
                request.Slug,
                request.ParentId.HasValue ? TopicId.From(request.ParentId.Value) : null,
                request.FactorPercentage,
                request.Description,
                request.IsActive);

            if (topicResult.IsFailure)
            {
                return Result.Failure<Guid>(topicResult.Error);
            }

            // Add to repository
            _topicRepository.Add(topicResult.Value);

            return Result.Success(topicResult.Value.Id.Value);
        }, cancellationToken);
    }
}
