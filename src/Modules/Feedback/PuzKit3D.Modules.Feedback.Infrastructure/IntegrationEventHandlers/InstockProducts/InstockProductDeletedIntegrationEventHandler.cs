using PuzKit3D.Contract.InStock.InstockProducts;
using PuzKit3D.Modules.Feedback.Application.UnitOfWork;
using PuzKit3D.Modules.Feedback.Persistence;
using PuzKit3D.SharedKernel.Application.Event;

namespace PuzKit3D.Modules.Feedback.Infrastructure.IntegrationEventHandlers.InstockProducts;

internal sealed class InstockProductDeletedIntegrationEventHandler
    : IIntegrationEventHandler<InstockProductDeletedIntegrationEvent>
{
    private readonly FeedbackDbContext _dbContext;
    private readonly IFeedbackUnitOfWork _unitOfWork;

    public InstockProductDeletedIntegrationEventHandler(
        FeedbackDbContext dbContext,
        IFeedbackUnitOfWork unitOfWork)
    {
        _dbContext = dbContext;
        _unitOfWork = unitOfWork;
    }

    public async Task HandleAsync(
        InstockProductDeletedIntegrationEvent @event,
        CancellationToken cancellationToken = default)
    {
        var productReplica = await _dbContext.ProductReplicas.FindAsync(
            new object[] { @event.ProductId }, cancellationToken);

        if (productReplica is not null)
        {
            _dbContext.ProductReplicas.Remove(productReplica);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
