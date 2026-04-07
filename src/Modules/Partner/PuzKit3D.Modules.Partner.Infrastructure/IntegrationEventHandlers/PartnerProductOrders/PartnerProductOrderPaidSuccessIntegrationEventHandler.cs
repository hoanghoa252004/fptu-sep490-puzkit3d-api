using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PuzKit3D.Contract.Partner.PartnerProductOrders;
using PuzKit3D.Modules.Partner.Application.UnitOfWork;
using PuzKit3D.Modules.Partner.Domain.Entities.PartnerProductOrders;
using PuzKit3D.Modules.Partner.Persistence;
using PuzKit3D.SharedKernel.Application.Event;

namespace PuzKit3D.Modules.Partner.Infrastructure.IntegrationEventHandlers.PartnerProductOrders;

internal sealed class PartnerProductOrderPaidSuccessIntegrationEventHandler
    : IIntegrationEventHandler<PartnerProductOrderPaidSuccessIntegrationEvent>
{
    private readonly PartnerDbContext _dbContext;
    private readonly IPartnerUnitOfWork _unitOfWork;
    private readonly ILogger<PartnerProductOrderPaidSuccessIntegrationEventHandler> _logger;

    public PartnerProductOrderPaidSuccessIntegrationEventHandler(
        PartnerDbContext dbContext,
        IPartnerUnitOfWork unitOfWork,
        ILogger<PartnerProductOrderPaidSuccessIntegrationEventHandler> logger)
    {
        _dbContext = dbContext;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task HandleAsync(
        PartnerProductOrderPaidSuccessIntegrationEvent @event, 
        CancellationToken cancellationToken = default)
    {
        try
        {
            var orderId = PartnerProductOrderId.From(@event.OrderId);
            var order = await _dbContext.PartnerProductOrders
                .FirstOrDefaultAsync(o => o.Id == orderId, cancellationToken);

            if (order == null)
            {
                _logger.LogWarning("PartnerProductOrder not found for payment confirmation. OrderId: {OrderId}", @event.OrderId);
                return;
            }

            var markAsPaidResult = order.MarkAsPaid(@event.PaidAt);
            if (markAsPaidResult.IsFailure)
            {
                _logger.LogError("Failed to mark PartnerProductOrder as paid. OrderId: {OrderId}, Error: {Error}", 
                    @event.OrderId, markAsPaidResult.Error);
                return;
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            _logger.LogInformation("PartnerProductOrder marked as paid successfully. OrderId: {OrderId}, PaidAt: {PaidAt}", 
                @event.OrderId, @event.PaidAt);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error handling PartnerProductOrderPaidIntegrationEvent for OrderId: {OrderId}", @event.OrderId);
            throw;
        }
    }
}
