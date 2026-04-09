using PuzKit3D.SharedKernel.Domain;

namespace PuzKit3D.Modules.Catalog.Domain.Entities.Formulas;

public class FormulaValueValidation : AggregateRoot<FormulaValueValidationId>
{
    public FormulaId FormulaId { get; private set; } = null!;
    public decimal MinValue { get; private set; }
    public decimal MaxValue { get; private set; }
    public string Output { get; private set; } = null!;
    public DateTime UpdatedAt { get; private set; }

    private FormulaValueValidation(
        FormulaValueValidationId id,
        FormulaId formulaId,
        decimal minValue,
        decimal maxValue,
        string output,
        DateTime updatedAt) : base(id)
    {
        FormulaId = formulaId;
        MinValue = minValue;
        MaxValue = maxValue;
        Output = output;
        UpdatedAt = updatedAt;
    }

    private FormulaValueValidation() : base()
    {
    }

    public static FormulaValueValidation Create(
        FormulaId formulaId,
        decimal minValue,
        decimal maxValue,
        string output,
        DateTime? updatedAt = null)
    {
        var validationId = FormulaValueValidationId.Create();
        var timestamp = updatedAt ?? DateTime.UtcNow;
        
        return new FormulaValueValidation(
            validationId,
            formulaId,
            minValue,
            maxValue,
            output,
            timestamp);
    }

    public void Update(decimal? minValue = null, decimal? maxValue = null, string? output = null)
    {
        if (minValue.HasValue)
            MinValue = minValue.Value;

        if (maxValue.HasValue)
            MaxValue = maxValue.Value;

        if (output != null)
            Output = output;

        UpdatedAt = DateTime.UtcNow;
    }
}
