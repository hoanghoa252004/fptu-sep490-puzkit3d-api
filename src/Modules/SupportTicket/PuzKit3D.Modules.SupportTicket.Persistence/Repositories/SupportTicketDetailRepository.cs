using Microsoft.EntityFrameworkCore;
using PuzKit3D.Modules.SupportTicket.Application.Repositories;
using PuzKit3D.Modules.SupportTicket.Domain.Entities.SupportTicketDetails;
using PuzKit3D.SharedKernel.Domain.Errors;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.SupportTicket.Persistence.Repositories;

internal sealed class SupportTicketDetailRepository : ISupportTicketDetailRepository
{
    private readonly SupportTicketDbContext _dbContext;

    public SupportTicketDetailRepository(SupportTicketDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<ResultT<List<SupportTicketDetail>>> GetBySupportTicketIdAsync(
        Guid supportTicketId,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var details = await _dbContext.SupportTicketDetails
                .Where(d => d.SupportTicketId.Value == supportTicketId)
                .ToListAsync(cancellationToken);

            return Result.Success(details);
        }
        catch (Exception ex)
        {
            return Result.Failure<List<SupportTicketDetail>>(
                Error.Failure("SupportTicketDetail.GetError", ex.Message));
        }
    }
}
