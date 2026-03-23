using PuzKit3D.Modules.Delivery.Application.DTOs;
using PuzKit3D.Modules.Delivery.Application.Repositories;
using PuzKit3D.Modules.Delivery.Application.Services;
using PuzKit3D.Modules.Delivery.Domain.Entities.DeliveryTrackings;
using PuzKit3D.SharedKernel.Application.Message.Query;
using PuzKit3D.SharedKernel.Domain.Errors;
using PuzKit3D.SharedKernel.Domain.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzKit3D.Modules.Delivery.Application.UseCases.DeliveryTrackings.Queries.GetWayBillNumber;

public sealed class GetWayBillNumberQueryHandler : IQueryHandler<GetWayBillNumberQuery, string>
{
    private readonly IDeliveryTrackingRepository _deliveryTrackingRepository;
    private readonly IDeliveryService _deliveryService;
    public GetWayBillNumberQueryHandler(IDeliveryTrackingRepository deliveryTrackingRepository, IDeliveryService deliveryService)
    {
        _deliveryTrackingRepository = deliveryTrackingRepository;
        _deliveryService = deliveryService;
    }
    public async Task<ResultT<string>> Handle(GetWayBillNumberQuery request, CancellationToken cancellationToken)
    {
        var trackingId = DeliveryTrackingId.From(request.DeliveryTrackingId);
        var tracking = await _deliveryTrackingRepository.GetByIdAsync(trackingId, cancellationToken);

        if (tracking is null)
        {
            return Result.Failure<string>(
                Error.NotFound("DeliveryTracking.NotFound",
                    $"Delivery tracking with ID {request.DeliveryTrackingId} not found"));
        }

        var tokenResult = await _deliveryService.GeneratePrintTokenAsync(new List<string> { tracking.DeliveryOrderCode });

        if (tokenResult.IsFailure)
        {
            Result.Failure<string>(
                Error.NotFound("DeliveryTracking.GenerateTokenFailed",
                    $"Generate token failed for {request.DeliveryTrackingId}"));
        }

        // Get the print order URL using the token
        var urlResult = await _deliveryService.GetPrintOrderUrlAsync(tokenResult.Value);

        if (urlResult.IsFailure)
        {
            Result.Failure<string>(
                Error.NotFound("DeliveryTracking.FailedToPrint",
                    $"Failed to print waybill of {request.DeliveryTrackingId}"));
        }
        return Result.Success(urlResult.Value);
    }
}
