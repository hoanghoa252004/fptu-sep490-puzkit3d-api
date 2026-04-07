using PuzKit3D.Modules.CustomDesign.Domain.Entities;
using PuzKit3D.Modules.CustomDesign.Domain.Entities.Proposals;
using PuzKit3D.Modules.CustomDesign.Domain.Entities.CustomDesignRequests;

namespace PuzKit3D.Modules.CustomDesign.Application.Repositories;

public interface IProposalRepository
{
    Task<Proposal?> GetByIdAsync(ProposalId id, CancellationToken cancellationToken = default);
    Task<Proposal?> GetByCodeAsync(string code, CancellationToken cancellationToken = default);
    Task<IEnumerable<Proposal>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<Proposal>> GetByRequestIdAsync(CustomDesignRequestId requestId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Proposal>> GetByStatusAsync(ProposalStatus status, CancellationToken cancellationToken = default);
    Task AddAsync(Proposal proposal, CancellationToken cancellationToken = default);
    void Update(Proposal proposal);
    void Delete(Proposal proposal);
    Task<bool> ExistsByIdAsync(ProposalId id, CancellationToken cancellationToken = default);
    Task<bool> ExistsByCodeAsync(string code, CancellationToken cancellationToken = default);
}
