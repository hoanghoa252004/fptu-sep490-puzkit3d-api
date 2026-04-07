using Microsoft.Extensions.Logging;
using MediatR;
using PuzKit3D.Modules.CustomDesign.Application.Repositories;
using PuzKit3D.Modules.CustomDesign.Domain.Entities.CustomDesignAssets.DomainEvents;
using PuzKit3D.Modules.CustomDesign.Domain.Entities.CustomDesignRequests;
using PuzKit3D.SharedKernel.Application.Exceptions;

namespace PuzKit3D.Modules.CustomDesign.Application.DomainEventHandlers.CustomDesignAssets;

internal sealed class CustomDesignAssetCreatedDomainEventHandler : INotificationHandler<CustomDesignAssetCreatedDomainEvent>
{
    private readonly ICustomDesignRequestRepository _requestRepository;

    public CustomDesignAssetCreatedDomainEventHandler(
        ICustomDesignRequestRepository requestRepository)
    {
        _requestRepository = requestRepository;
    }

    public async Task Handle(
        CustomDesignAssetCreatedDomainEvent domainEvent,
        CancellationToken cancellationToken)
    {
        var requestResult = await _requestRepository.GetByIdAsync(
            CustomDesignRequestId.From(domainEvent.RequestId),
            cancellationToken);

        if (requestResult.IsFailure)
        {
            throw new PuzKit3DException("Request Id not found");
        }

        var request = requestResult.Value;

        // Increment the counter
        request.IncrementUsedSupportConceptDesignTime();

        _requestRepository.Update(request);
    }
}
