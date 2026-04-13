namespace PuzKit3D.Modules.Catalog.Application.UseCases.Formulas.Commands.CalculateFormula;

public sealed class FormulaCalculateRequest
{
    /// <summary>
    /// Single Capability ID - will extract single FactorPercentage from Capability entity
    /// </summary>
    public Guid? CapabilityId { get; set; }

    /// <summary>
    /// List of Capability IDs - will extract FactorPercentage list from Capability entities
    /// </summary>
    public List<Guid>? CapabilityIds { get; set; }

    /// <summary>
    /// Single Material ID - will extract single FactorPercentage from Material entity
    /// </summary>
    public Guid? MaterialId { get; set; }

    /// <summary>
    /// List of Material IDs - will extract FactorPercentage list from Material entities
    /// </summary>
    public List<Guid>? MaterialIds { get; set; }

    /// <summary>
    /// Single Topic ID - will extract single FactorPercentage from Topic entity
    /// </summary>
    public Guid? TopicId { get; set; }

    /// <summary>
    /// List of Topic IDs - will extract FactorPercentage list from Topic entities
    /// </summary>
    public List<Guid>? TopicIds { get; set; }

    /// <summary>
    /// Single Assembly Method ID - will extract single FactorPercentage from AssemblyMethod entity
    /// </summary>
    public Guid? AssemblyMethodId { get; set; }

    /// <summary>
    /// List of Assembly Method IDs - will extract FactorPercentage list from AssemblyMethod entities
    /// </summary>
    public List<Guid>? AssemblyMethodIds { get; set; }

    /// <summary>
    /// Total piece count for the product
    /// </summary>
    public decimal? PieceCount { get; set; }
}



