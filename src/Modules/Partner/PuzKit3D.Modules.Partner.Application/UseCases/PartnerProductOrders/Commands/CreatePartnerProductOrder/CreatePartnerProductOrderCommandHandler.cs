using PuzKit3D.Modules.InStock.Domain.Entities.InstockOrders;
using PuzKit3D.Modules.Partner.Application.Repositories;
using PuzKit3D.Modules.Partner.Application.Services;
using PuzKit3D.Modules.Partner.Application.UnitOfWork;
using PuzKit3D.Modules.Partner.Domain.Entities.PartnerProductOrders;
using PuzKit3D.Modules.Partner.Domain.Entities.PartnerProductQuotations;
using PuzKit3D.Modules.Partner.Domain.Entities.PartnerProducts;
using PuzKit3D.SharedKernel.Application.Message.Command;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.Partner.Application.UseCases.PartnerProductOrders.Commands.CreatePartnerProductOrder;

internal sealed class CreatePartnerProductOrderCommandHandler : ICommandTHandler<CreatePartnerProductOrderCommand, Guid>
{
    private readonly IPartnerProductOrderRepository _orderRepository;
    private readonly IPartnerProductQuotationRepository _quotationRepository;
    private readonly IPartnerUnitOfWork _unitOfWork;
    private readonly IPartnerProductOrderCodeGenerator _codeGenerator;
    private readonly IPartnerProductRepository _partnerProductRepository;

    public CreatePartnerProductOrderCommandHandler(
        IPartnerProductOrderRepository orderRepository,
        IPartnerProductQuotationRepository quotationRepository,
        IPartnerUnitOfWork unitOfWork,
        IPartnerProductOrderCodeGenerator codeGenerator,
        IPartnerProductRepository partnerProductRepository)
    {
        _orderRepository = orderRepository;
        _quotationRepository = quotationRepository;
        _unitOfWork = unitOfWork;
        _codeGenerator = codeGenerator;
        _partnerProductRepository = partnerProductRepository;
    }

    public async Task<ResultT<Guid>> Handle(
        CreatePartnerProductOrderCommand request,
        CancellationToken cancellationToken)
    {
        var result = await _unitOfWork.ExecuteAsync(async () =>
        {
            var code = await _codeGenerator.GenerateNextCodeAsync(cancellationToken);

            // Lấy quotation
            var quotation = await _quotationRepository.GetByIdAsync(
                PartnerProductQuotationId.From(request.QuotationId),
                cancellationToken);

            if (quotation is null)
            {
                return Result.Failure<Guid>(PartnerProductQuotationError.NotFound(request.QuotationId));
            }

            var existedOrder = await _orderRepository.GetByQuotationIdAsync(quotation.Id, cancellationToken);

            if (existedOrder != null)
            {
                return Result.Failure<Guid>(
                    PartnerProductOrderError.AlreadyExistsForQuotation());
            }

            // Kiểm tra quotation status phải là Accepted
            if (quotation.Status != PartnerProductQuotationStatus.Accepted)
            {
                return Result.Failure<Guid>(
                    PartnerProductOrderError.QuotationNotAccepted(quotation.Status));
            }

            // Tính toán grand total amount
            var calculatedGrandTotalAmount = quotation.GrandTotalAmount + request.ShippingFee - request.UserCoinAmount;

            // Create order
            var orderResult = PartnerProductOrder.Create(
                code,
                quotation.Id,
                request.CustomerId,
                request.CustomerName,
                request.CustomerPhone,
                request.CustomerEmail,
                request.CustomerProvinceName,
                request.CustomerDistrictName,
                request.CustomerWardName,
                request.DetailAddress,
                quotation.SubTotalAmount,
                request.ShippingFee,
                quotation.ShippingFee,
                quotation.ImportTaxAmount,
                request.UserCoinAmount,
                calculatedGrandTotalAmount,
                request.PaymentMethod);

            if (orderResult.IsFailure)
            {
                return Result.Failure<Guid>(orderResult.Error);
            }

            var order = orderResult.Value;

            if(request.Items != null && request.Items.Any())
            {
                // Create order details
                foreach (var item in request.Items)
                {
                    // Get product name
                    var product = await _partnerProductRepository.GetByIdAsync(
                        PartnerProductId.From(item.PartnerProductId),
                        cancellationToken);

                    string? productName = product?.Name;

                    // Create order detail
                    var detailResult = PartnerProductOrderDetail.Create(
                            order.Id,
                            PartnerProductId.From(item.PartnerProductId),
                            productName,
                            item.UnitPrice,
                            item.Quantity);

                    if (detailResult.IsFailure)
                    {
                        return Result.Failure<Guid>(detailResult.Error);
                    }

                    order.AddDetail(detailResult.Value);
                }
            }
            order.RaiseOrderCreatedDomainEvent();

            _orderRepository.Add(order);

            return Result.Success(order.Id.Value);
        }, cancellationToken);

        return result;
    }
}
