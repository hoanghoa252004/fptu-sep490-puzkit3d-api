using Microsoft.EntityFrameworkCore;
using PuzKit3D.Modules.CustomDesign.Application.Repositories;
using PuzKit3D.Modules.CustomDesign.Domain.Entities;
using PuzKit3D.Modules.CustomDesign.Domain.Entities.CustomDesignRequirements;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.CustomDesign.Persistence.Repositories;

internal sealed class CustomDesignRequirementRepository : ICustomDesignRequirementRepository
{
    private readonly CustomDesignDbContext _context;
    private readonly ITopicReplicaRepository _topicReplicaRepository;
    private readonly IMaterialReplicaRepository _materialReplicaRepository;
    private readonly IAssemblyMethodReplicaRepository _assemblyMethodReplicaRepository;
    private readonly ICapabilityReplicaRepository _capabilityReplicaRepository;

    public CustomDesignRequirementRepository(
        CustomDesignDbContext context,
        ITopicReplicaRepository topicReplicaRepository,
        IMaterialReplicaRepository materialReplicaRepository,
        IAssemblyMethodReplicaRepository assemblyMethodReplicaRepository,
        ICapabilityReplicaRepository capabilityReplicaRepository)
    {
        _context = context;
        _topicReplicaRepository = topicReplicaRepository;
        _materialReplicaRepository = materialReplicaRepository;
        _assemblyMethodReplicaRepository = assemblyMethodReplicaRepository;
        _capabilityReplicaRepository = capabilityReplicaRepository;
    }


    public async Task<ResultT<CustomDesignRequirement>> GetByIdAsync(
        CustomDesignRequirementId id,
        CancellationToken cancellationToken = default)
    {
        var requirement = await _context.CustomDesignRequirements
            .FirstOrDefaultAsync(r => r.Id == id, cancellationToken);

        if (requirement == null)
            return Result.Failure<CustomDesignRequirement>(CustomDesignRequirementError.NotFound());
        return Result.Success(requirement);
    }

    public async Task<ResultT<CustomDesignRequirement>> GetByCodeAsync(
        string code,
        CancellationToken cancellationToken = default)
    {
        var requirement = await _context.CustomDesignRequirements
            .FirstOrDefaultAsync(r => r.Code == code, cancellationToken);

        if (requirement == null)
            return Result.Failure<CustomDesignRequirement>(CustomDesignRequirementError.NotFound());
        return Result.Success(requirement);
    }

    public async Task<IEnumerable<CustomDesignRequirement>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        return await _context.CustomDesignRequirements
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<CustomDesignRequirement>> GetActiveAsync(
        CancellationToken cancellationToken = default)
    {
        return await _context.CustomDesignRequirements
            .Where(r => r.IsActive)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(
        CustomDesignRequirement requirement,
        CancellationToken cancellationToken = default)
    {
        await _context.CustomDesignRequirements.AddAsync(requirement, cancellationToken);
    }

    public void Update(CustomDesignRequirement requirement)
    {
        _context.CustomDesignRequirements.Update(requirement);
    }

    public void Delete(CustomDesignRequirement requirement)
    {
        _context.CustomDesignRequirements.Remove(requirement);
    }

    public async Task<bool> ExistsByIdAsync(CustomDesignRequirementId id, CancellationToken cancellationToken = default)
    {
        return await _context.CustomDesignRequirements
            .AnyAsync(r => r.Id == id, cancellationToken);
    }

    public async Task<bool> ExistsByCodeAsync(string code, CancellationToken cancellationToken = default)
    {
        return await _context.CustomDesignRequirements
            .AnyAsync(r => r.Code == code, cancellationToken);
    }
}
