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

    public async Task<bool> ExistsDuplicateAsync(
        Guid topicId,
        Guid materialId,
        Guid assemblyMethodId,
        DifficultyLevel difficulty,
        IEnumerable<Guid> capabilityIds,
        CancellationToken cancellationToken = default)
    {
        var capabilityIdsList = capabilityIds.ToList();
        var capabilityCount = capabilityIdsList.Count;

        // Find requirements with matching Topic, Material, AssemblyMethod, and Difficulty
        var matchingRequirements = await _context.CustomDesignRequirements
            .Where(r => r.TopicId == topicId
                && r.MaterialId == materialId
                && r.AssemblyMethodId == assemblyMethodId
                && r.Difficulty == difficulty)
            .Select(r => r.Id)
            .ToListAsync(cancellationToken);

        if (!matchingRequirements.Any())
            return false;

        // Check if any matching requirement has the exact same set of capabilities
        foreach (var requirementId in matchingRequirements)
        {
            var requirementCapabilities = await _context.RequirementCapabilityDetails
                .Where(d => d.CustomDesignRequirementId == CustomDesignRequirementId.From(requirementId.Value))
                .Select(d => d.CapabilityId)
                .ToListAsync(cancellationToken);

            if (requirementCapabilities.Count == capabilityCount
                && requirementCapabilities.All(c => capabilityIdsList.Contains(c)))
            {
                return true;
            }
        }

        return false;
    }
}
