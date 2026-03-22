using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.SupportTicket.Application.UnitOfWork;

public interface ISupportTicketUnitOfWork
{
    Task<T> ExecuteAsync<T>(Func<Task<T>> action, CancellationToken cancellationToken = default);

    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
