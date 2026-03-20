using PuzKit3D.Contract.InStock.InstockProducts;
using PuzKit3D.Modules.Feedback.Application.UnitOfWork;
using PuzKit3D.Modules.Feedback.Domain.Entities.ProductReplicas;
using PuzKit3D.Modules.Feedback.Persistence;
using PuzKit3D.SharedKernel.Application.Event;

namespace PuzKit3D.Modules.Feedback.Infrastructure.IntegrationEventHandlers.InstockProducts;

internal sealed class InstockProductCreatedIntegrationEventHandler
    : IIntegrationEventHandler<InstockProductCreatedIntegrationEvent>
{
    private readonly FeedbackDbContext _dbContext;
    private readonly IFeedbackUnitOfWork _unitOfWork;

    public InstockProductCreatedIntegrationEventHandler(
        FeedbackDbContext dbContext,
        IFeedbackUnitOfWork unitOfWork)
    {
        _dbContext = dbContext;
        _unitOfWork = unitOfWork;
    }

    public async Task HandleAsync(
        InstockProductCreatedIntegrationEvent @event,
        CancellationToken cancellationToken = default)
    {
        var productReplica = ProductReplica.Create(
            @event.ProductId,
            "Instock",
            @event.Name);

        await _dbContext.ProductReplicas.AddAsync(productReplica, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
