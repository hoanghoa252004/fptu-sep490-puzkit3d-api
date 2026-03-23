namespace PuzKit3D.Modules.SupportTicket.Application.Services;

public interface ISupportTicketCodeGenerator
{
    Task<string> GenerateNextCodeAsync(CancellationToken cancellationToken = default);
}

