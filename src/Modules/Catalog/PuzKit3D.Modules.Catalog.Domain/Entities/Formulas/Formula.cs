using PuzKit3D.SharedKernel.Domain;
using PuzKit3D.SharedKernel.Domain.Results;

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

    public static ResultT<Formula> Create(
        string code,
        string expression,
        string? description = null,
        DateTime? updatedAt = null)
    {
        // Validate input
        if (string.IsNullOrWhiteSpace(code))
            return Result.Failure<Formula>(FormulaError.InvalidCode());

        if (string.IsNullOrWhiteSpace(expression))
            return Result.Failure<Formula>(FormulaError.InvalidExpression());


        var formulaId = FormulaId.Create();
        var timestamp = updatedAt ?? DateTime.UtcNow;

        return Result.Success(new Formula(
            formulaId,
            code,
            expression,
            description,
            timestamp));
    }

    public Result Update(
        string code, 
        string expression, 
        string? description = null)
    {
            Code = code;
            Expression = expression;
            Description = description;

        UpdatedAt = DateTime.UtcNow;
        return Result.Success();
    }
}
