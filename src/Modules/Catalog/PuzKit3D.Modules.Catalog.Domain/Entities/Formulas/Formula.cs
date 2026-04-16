using PuzKit3D.SharedKernel.Domain;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.Catalog.Domain.Entities.Formulas;

public class Formula : AggregateRoot<FormulaId>
{
    public FormulaCode Code { get; private set; }
    public string Expression { get; private set; } = null!;
    public string? Description { get; private set; }
    public bool IsNeedValidation { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    // Navigation properties
    public ICollection<FormulaValueValidation> FormulaValueValidations { get; private set; } = new List<FormulaValueValidation>();

    private Formula(
        FormulaId id,
        FormulaCode code,
        string expression,
        string? description,
        bool isNeedValidation,
        DateTime updatedAt) : base(id)
    {
        Code = code;
        Expression = expression;
        Description = description;
        IsNeedValidation = isNeedValidation;
        UpdatedAt = updatedAt;
    }

    private Formula() : base()
    {
    }

    public static ResultT<Formula> Create(
        FormulaCode code,
        string expression,
        bool isNeedValidation = false,
        string? description = null,
        DateTime? updatedAt = null)
    {
        // Validate input
        if (string.IsNullOrWhiteSpace(expression))
            return Result.Failure<Formula>(FormulaError.InvalidExpression());

        var formulaId = FormulaId.Create();
        var timestamp = updatedAt ?? DateTime.UtcNow;

        return Result.Success(new Formula(
            formulaId,
            code,
            expression,
            description,
            isNeedValidation,
            timestamp));
    }

    public Result Update(
        string expression, 
        string? description = null)
    {
        if (string.IsNullOrWhiteSpace(expression))
            return Result.Failure(FormulaError.InvalidExpression());

        Expression = expression;
        Description = description;

        UpdatedAt = DateTime.UtcNow;
        return Result.Success();
    }

    public void SetNeedValidation(bool value)
    {
        IsNeedValidation = value;
        UpdatedAt = DateTime.UtcNow;
    }
}


