using PuzKit3D.Modules.CustomDesign.Domain.Entities;
using PuzKit3D.Modules.CustomDesign.Domain.Entities.Workflows;
using PuzKit3D.Modules.CustomDesign.Domain.Entities.Proposals;
using PuzKit3D.Modules.CustomDesign.Domain.Entities.Phases;

namespace PuzKit3D.Modules.CustomDesign.Application.Repositories;

public interface IWorkflowRepository
{
    Task<Workflow?> GetByIdAsync(WorkflowId id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Workflow>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<Workflow>> GetByProposalIdAsync(ProposalId proposalId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Workflow>> GetByPhaseIdAsync(PhaseId phaseId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Workflow>> GetByStatusAsync(WorkflowStatus status, CancellationToken cancellationToken = default);
    Task<Workflow?> GetByProposalAndPhaseAsync(ProposalId proposalId, PhaseId phaseId, CancellationToken cancellationToken = default);
    Task AddAsync(Workflow workflow, CancellationToken cancellationToken = default);
    void Update(Workflow workflow);
    void Delete(Workflow workflow);
    Task<bool> ExistsByIdAsync(WorkflowId id, CancellationToken cancellationToken = default);
}
