using PuzKit3D.Contract.InStock.InstockOrders;
using PuzKit3D.SharedKernel.Application.Event;
using Microsoft.Extensions.Logging;
using PuzKit3D.Modules.InStock.Persistence;
using PuzKit3D.Modules.InStock.Application.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using PuzKit3D.Modules.InStock.Domain.Entities.InstockOrders;

namespace PuzKit3D.Modules.InStock.Infrastructure.IntegrationEventHandlers.InstockOrders;

internal sealed class InstockOrderPaidSuccessIntegrationEventHandler
    : IIntegrationEventHandler<InstockOrderPaidSuccessIntegrationEvent>
{
    private readonly InStockDbContext _dbContext;
    private readonly IInStockUnitOfWork _unitOfWork;
    private readonly ILogger<InstockOrderPaidSuccessIntegrationEventHandler> _logger;

    public InstockOrderPaidSuccessIntegrationEventHandler(
        InStockDbContext dbContext,
        IInStockUnitOfWork unitOfWork,
        ILogger<InstockOrderPaidSuccessIntegrationEventHandler> logger)
    {
        _dbContext = dbContext;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task HandleAsync(
        InstockOrderPaidSuccessIntegrationEvent @event,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var orderId = InstockOrderId.From(@event.OrderId);
            var order = await _dbContext.InstockOrders
                .FirstOrDefaultAsync(o => o.Id == orderId, cancellationToken);

            if (order == null)
            {
                _logger.LogWarning("InstockOrder not found for payment confirmation. OrderId: {OrderId}", @event.OrderId);
                return;
            }

            var markAsPaidResult = order.MarkAsPaid(@event.PaidAt);
            if (markAsPaidResult.IsFailure)
            {
                _logger.LogWarning("Failed to mark order as paid. OrderId: {OrderId}, Error: {Error}",
                    @event.OrderId, markAsPaidResult.Error.Message);
                return;
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            _logger.LogInformation("InstockOrder marked as paid. OrderId: {OrderId}, PaidAt: {PaidAt}",
                @event.OrderId, @event.PaidAt);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error handling InstockOrderPaidIntegrationEvent for OrderId: {OrderId}", @event.OrderId);
            throw;
        }
    }
}
