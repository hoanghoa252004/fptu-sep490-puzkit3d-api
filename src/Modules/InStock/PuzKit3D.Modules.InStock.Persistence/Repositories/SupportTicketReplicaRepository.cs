using Microsoft.EntityFrameworkCore;
using PuzKit3D.Modules.InStock.Application.Repositories;
using PuzKit3D.Modules.InStock.Domain.Entities.Replicas;
using PuzKit3D.SharedKernel.Domain.Errors;
using PuzKit3D.SharedKernel.Domain.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzKit3D.Modules.InStock.Persistence.Repositories;

internal sealed class SupportTicketReplicaRepository : ISupportTicketReplicaRepository
{
    private readonly InStockDbContext _dbContext;

    public SupportTicketReplicaRepository(InStockDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<ResultT<List<SupportTicketReplica>>> GetByOrderIdAsync(Guid orderId, CancellationToken cancellationToken = default)
    {
        try
        {
            var tickets = await _dbContext.SupportTicketReplicas
                .Where(st => st.OrderId == orderId)
                .ToListAsync(cancellationToken);

            return Result.Success(tickets);
        }
        catch (Exception ex)
        {
            return Result.Failure<List<SupportTicketReplica>>(
                Error.Failure("SupportTicket.GetError", ex.Message));
        }
    }
}
