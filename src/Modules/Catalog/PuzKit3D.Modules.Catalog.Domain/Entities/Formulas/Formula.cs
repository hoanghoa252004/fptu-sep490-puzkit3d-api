using PuzKit3D.SharedKernel.Domain;

namespace PuzKit3D.Modules.Catalog.Domain.Entities.Formulas;

public class Formula : AggregateRoot<FormulaId>
{
    public string Code { get; private set; } = null!;
    public string Expression { get; private set; } = null!;
    public string? Description { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    // Navigation properties
    public ICollection<FormulaValueValidation> FormulaValueValidations { get; private set; } = new List<FormulaValueValidation>();

    private Formula(
        FormulaId id,
        string code,
        string expression,
        string? description,
        DateTime updatedAt) : base(id)
    {
        Code = code;
        Expression = expression;
        Description = description;
        UpdatedAt = updatedAt;
    }

    private Formula() : base()
    {
    }

    public static Formula Create(
        string code,
        string expression,
        string? description = null,
        DateTime? updatedAt = null)
    {
        var formulaId = FormulaId.Create();
        var timestamp = updatedAt ?? DateTime.UtcNow;
        
        return new Formula(
            formulaId,
            code,
            expression,
            description,
            timestamp);
    }

    public void Update(string? code = null, string? expression = null, string? description = null)
    {
        if (code != null)
            Code = code;

        if (expression != null)
            Expression = expression;

        if (description != null)
            Description = description;

        UpdatedAt = DateTime.UtcNow;
    }
}
