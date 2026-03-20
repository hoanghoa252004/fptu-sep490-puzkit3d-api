using PuzKit3D.Contract.InStock.InstockProducts;
using PuzKit3D.Modules.Feedback.Application.UnitOfWork;
using PuzKit3D.Modules.Feedback.Persistence;
using PuzKit3D.SharedKernel.Application.Event;

namespace PuzKit3D.Modules.Feedback.Infrastructure.IntegrationEventHandlers.InstockProducts;

internal sealed class InstockProductUpdatedIntegrationEventHandler
    : IIntegrationEventHandler<InstockProductUpdatedIntegrationEvent>
{
    private readonly FeedbackDbContext _dbContext;
    private readonly IFeedbackUnitOfWork _unitOfWork;

    public InstockProductUpdatedIntegrationEventHandler(
        FeedbackDbContext dbContext,
        IFeedbackUnitOfWork unitOfWork)
    {
        _dbContext = dbContext;
        _unitOfWork = unitOfWork;
    }

    public async Task HandleAsync(
        InstockProductUpdatedIntegrationEvent @event,
        CancellationToken cancellationToken = default)
    {
        var productReplica = await _dbContext.ProductReplicas.FindAsync(
            new object[] { @event.ProductId }, cancellationToken);

        if (productReplica is not null)
        {
            // Update the replica with new data
            // Since ProductReplica is immutable, we need to recreate it
            _dbContext.ProductReplicas.Remove(productReplica);
            
            var updatedReplica = Domain.Entities.ProductReplicas.ProductReplica.Create(
                @event.ProductId,
                "Instock",
                @event.Name);

            await _dbContext.ProductReplicas.AddAsync(updatedReplica, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
