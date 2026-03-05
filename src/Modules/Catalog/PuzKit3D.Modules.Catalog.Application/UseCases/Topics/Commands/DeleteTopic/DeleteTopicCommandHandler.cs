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

        // Execute in transaction
        return await _unitOfWork.ExecuteAsync(async () =>
        {
            _topicRepository.Delete(topic);
            return Result.Success();
        }, cancellationToken);
    }
}
