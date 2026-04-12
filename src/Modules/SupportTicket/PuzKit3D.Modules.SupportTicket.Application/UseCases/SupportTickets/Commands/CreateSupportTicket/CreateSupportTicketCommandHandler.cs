using MediatR;
using PuzKit3D.Modules.SupportTicket.Application.Repositories;
using PuzKit3D.Modules.SupportTicket.Application.UnitOfWork;
using PuzKit3D.Modules.SupportTicket.Domain.Entities.SupportTickets;
using PuzKit3D.Modules.SupportTicket.Domain.Entities.SupportTicketDetails;
using PuzKit3D.SharedKernel.Application.Message.Command;
using PuzKit3D.SharedKernel.Domain.Results;
using SupportTicketEntity = PuzKit3D.Modules.SupportTicket.Domain.Entities.SupportTickets.SupportTicket;
using PuzKit3D.Modules.SupportTicket.Domain.Entities.OrderReplicas;
using PuzKit3D.Modules.SupportTicket.Application.Services;

namespace PuzKit3D.Modules.SupportTicket.Application.UseCases.SupportTickets.Commands.CreateSupportTicket;

internal sealed class CreateSupportTicketCommandHandler
    : ICommandTHandler<CreateSupportTicketCommand, Guid>
{
    private readonly ISupportTicketRepository _repository;
    private readonly IOrderReplicaRepository _orderReplicaRepository;
    private readonly IOrderDetailReplicaRepository _orderDetailReplicaRepository;
    private readonly IDriveReplicaRepository _partReplicaRepository;
    private readonly ISupportTicketUnitOfWork _unitOfWork;
    private readonly ISupportTicketCodeGenerator _supportTicketCodeGenerator;

    public CreateSupportTicketCommandHandler(
        ISupportTicketRepository repository,
        IOrderReplicaRepository orderReplicaRepository,
        IOrderDetailReplicaRepository orderDetailReplicaRepository,
        IDriveReplicaRepository partReplicaRepository,
        ISupportTicketUnitOfWork unitOfWork,
        ISupportTicketCodeGenerator supportTicketCodeGenerator)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _orderReplicaRepository = orderReplicaRepository;
        _orderDetailReplicaRepository = orderDetailReplicaRepository;
        _partReplicaRepository = partReplicaRepository;
        _supportTicketCodeGenerator = supportTicketCodeGenerator;
    }

    public async Task<ResultT<Guid>> Handle(CreateSupportTicketCommand request, CancellationToken cancellationToken)
    {

        // Check if order exists
        var order = await _orderReplicaRepository.GetByIdAsync(request.OrderId, cancellationToken);
        if (order.IsFailure == true)
        {
            return Result.Failure<Guid>(OrderReplicaError.OrderNotFound(request.OrderId));
        }

        // Validate: Order status must be HandedOverToDelivery
        if (order.Value.Status != "HandedOverToDelivery")
        {
            return Result.Failure<Guid>(SupportTicketError.OrderStatusNotEligibleForSupportTicket(request.OrderId, order.Value.Status));
        }

        // Check if there are existing support tickets for this order
        // Only 1 support ticket per order is allowed, regardless of status
        var existingTicketsResult = await _repository.GetByOrderIdAsync(request.OrderId, cancellationToken);
        if (existingTicketsResult.IsSuccess && existingTicketsResult.Value.Any())
        {
            return Result.Failure<Guid>(
                SupportTicketError.OrderAlreadyHasSupportTicket(request.OrderId));
        }

        // Validation: requires at least 1 detail
        if (request.Details.Count == 0)
            return Result.Failure<Guid>(SupportTicketError.DetailsRequiredForReplacePart());

        foreach(var item in request.Details)
        {
            // Check if order detail exists
            var orderDetail = await _orderDetailReplicaRepository.GetByIdAsync(item.OrderDetailId, cancellationToken);

            if (orderDetail is null)
            {
                return Result.Failure<Guid>(OrderReplicaError.OrderDetailNotFound(item.OrderDetailId));
            }

            // Validation: if type is ReplaceDrive, DriveId is required
            if (request.Type == SupportTicketType.ReplaceDrive && item.DriveId == null)
            {
                return Result.Failure<Guid>(SupportTicketError.PartIdRequiredForReplacePart());
            }

            // Validation: if type is ReplaceDrive, check if part exists and has sufficient quantity
            if (request.Type == SupportTicketType.ReplaceDrive && item.DriveId.HasValue)
            {
                var drive = await _partReplicaRepository.GetByIdAsync(item.DriveId.Value, cancellationToken);
                
                if (drive is null)
                {
                    return Result.Failure<Guid>(SupportTicketError.PartNotFound(item.DriveId.Value));
                }

                //if (item.Quantity > part.Quantity)
                //{
                //    return Result.Failure<Guid>(SupportTicketError.ReplacePartQuantityExceedsAvailable(item.PartId.Value, part.Quantity, item.Quantity));
                //}
            }

            // Validation: if type is Exchange, quantity must be <= orderDetail quantity
            if (request.Type == SupportTicketType.Exchange && item.Quantity > orderDetail.Quantity)
            {
                return Result.Failure<Guid>(SupportTicketError.ExchangeQuantityExceedsOrderDetailQuantity(item.OrderDetailId, orderDetail.Quantity));
            }
        }

        return await _unitOfWork.ExecuteAsync(async () =>
        {
            var code = await _supportTicketCodeGenerator.GenerateNextCodeAsync(cancellationToken);
            var result = SupportTicketEntity.Create(
            request.UserId,
            request.OrderId,
            request.Type,
            request.Reason,
            request.Proof,
            code);

            if (result.IsFailure)
                return Result.Failure<Guid>(result.Error);

            var ticket = result.Value;

            // Add details to the ticket
            foreach (var detail in request.Details)
            {
                var detailResult = SupportTicketDetail.Create(
                    ticket.Id,
                    detail.OrderDetailId,
                    detail.DriveId,
                    detail.Quantity,
                    detail.Note);

                if (detailResult.IsFailure)
                    return Result.Failure<Guid>(detailResult.Error);

                ticket.AddDetail(detailResult.Value);
            }

            await _repository.AddAsync(ticket, cancellationToken);

            ticket.RaiseCreateSupportTicket();
            return Result.Success(result.Value.Id.Value);
        }, cancellationToken);
    }
}
