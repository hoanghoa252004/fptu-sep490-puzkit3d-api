using PuzKit3D.Modules.SupportTicket.Application.Repositories;
using PuzKit3D.Modules.SupportTicket.Domain.Entities.SupportTickets;
using PuzKit3D.SharedKernel.Application.Authorization;
using PuzKit3D.SharedKernel.Application.Media;
using PuzKit3D.SharedKernel.Application.Message.Query;
using PuzKit3D.SharedKernel.Application.Pagination;
using PuzKit3D.SharedKernel.Application.User;
using PuzKit3D.SharedKernel.Domain.Results;
using System.Net.Sockets;

namespace PuzKit3D.Modules.SupportTicket.Application.UseCases.SupportTickets.Queries.GetSupportTickets;

internal sealed class GetSupportTicketsQueryHandler
    : IQueryHandler<GetSupportTicketsQuery, PagedResult<SupportTicketDto>>
{
    private readonly ISupportTicketRepository _repository;
    private readonly ICurrentUser _currentUser;
    private readonly IMediaAssetService _mediaAssetService;

    public GetSupportTicketsQueryHandler(
        ISupportTicketRepository repository,
        ICurrentUser currentUser, IMediaAssetService mediaAssetService)
    {
        _repository = repository;
        _currentUser = currentUser;
        _mediaAssetService = mediaAssetService;
    }

    public async Task<ResultT<PagedResult<SupportTicketDto>>> Handle(
        GetSupportTicketsQuery request,
        CancellationToken cancellationToken)
    {
        if (!_currentUser.IsAuthenticated)
            throw new UnauthorizedAccessException("User must be authenticated");

        var userId = Guid.Parse(_currentUser.UserId!);
        var isStaff = _currentUser.IsInRole(Roles.Staff);

        // Get tickets based on user role
        var ticketsResult = isStaff
            ? await _repository.GetAllAsync(cancellationToken)
            : await _repository.GetByUserIdAsync(userId, cancellationToken);

        if (ticketsResult.IsFailure)
            return Result.Failure<PagedResult<SupportTicketDto>>(ticketsResult.Error);

        var query = ticketsResult.Value.AsQueryable();

        // Apply status filter if provided
        if (!string.IsNullOrWhiteSpace(request.Status))
        {
            if (Enum.TryParse<SupportTicketStatus>(request.Status, ignoreCase: true, out var statusEnum))
            {
                query = query.Where(st => st.Status == statusEnum);
            }
        }

        // Apply pagination
        var totalCount = query.Count();
        var tickets = query
            .OrderByDescending(st => st.CreatedAt)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToList();

        var dtos = tickets.Select(st => new SupportTicketDto(
            st.Id.Value,
            st.Code,
            st.UserId,
            st.OrderId,
            st.Type.ToString(),
            st.Status.ToString(),
            st.Reason,
            _mediaAssetService.BuildAssetUrl(st.Proof),
            st.CreatedAt,
            st.UpdatedAt,
            st.Details
                .Select(d => new SupportTicketDetailDto(
                    d.Id.Value,
                    d.OrderItemId,
                    d.DriveId,
                    d.Quantity,
                    d.Note))
                .ToList())).OrderByDescending(x => x.CreatedAt).ToList();

        var pagedResult = new PagedResult<SupportTicketDto>(
            dtos,
            request.PageNumber,
            request.PageSize,
            totalCount);

        return Result.Success(pagedResult);
    }
}

