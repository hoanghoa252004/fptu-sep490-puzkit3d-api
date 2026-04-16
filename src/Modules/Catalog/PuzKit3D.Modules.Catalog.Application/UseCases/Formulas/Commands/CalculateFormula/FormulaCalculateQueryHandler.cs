using NCalc;
using PuzKit3D.Modules.Catalog.Application.Repositories;
using PuzKit3D.Modules.Catalog.Domain.Entities.Formulas;
using PuzKit3D.SharedKernel.Application.Message.Query;
using PuzKit3D.SharedKernel.Domain.Errors;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.Catalog.Application.UseCases.Formulas.Commands.CalculateFormula;

internal sealed class FormulaCalculateQueryHandler
    : IQueryHandler<FormulaCalculateQuery, FormulaCalculateResponse>
{
    private readonly IFormulaRepository _formulaRepository;
    private readonly IFormulaValueValidationRepository _validationRepository;
    private readonly ICapabilityRepository _capabilityRepository;
    private readonly IMaterialRepository _materialRepository;
    private readonly ITopicRepository _topicRepository;
    private readonly IAssemblyMethodRepository _assemblyMethodRepository;

    public FormulaCalculateQueryHandler(
        IFormulaRepository formulaRepository,
        IFormulaValueValidationRepository validationRepository,
        ICapabilityRepository capabilityRepository,
        IMaterialRepository materialRepository,
        ITopicRepository topicRepository,
        IAssemblyMethodRepository assemblyMethodRepository)
    {
        _formulaRepository = formulaRepository;
        _validationRepository = validationRepository;
        _capabilityRepository = capabilityRepository;
        _materialRepository = materialRepository;
        _topicRepository = topicRepository;
        _assemblyMethodRepository = assemblyMethodRepository;
    }

    public async Task<ResultT<FormulaCalculateResponse>> Handle(
        FormulaCalculateQuery request,
        CancellationToken cancellationToken)
    {
        // Step 1: Get formula from DB
        var formulas = await _formulaRepository.GetAllAsync(cancellationToken);
        var formula = formulas.FirstOrDefault(f => f.Code.ToString() == request.FormulaCode);

        if (formula is null)
            return Result.Failure<FormulaCalculateResponse>(FormulaError.NotFound(Guid.Empty));

        // Step 2: Extract factors from entities based on IDs
        // Single IDs map to single factors, List IDs map to list factors
        
        // Capability: single ID -> CapabilityFactor, list IDs -> CapabilityFactors
        decimal capabilityFactor = 0m;
        var capabilityFactors = new List<decimal>();
        
        if (request.Request.CapabilityId.HasValue)
        {
            var capabilities = await _capabilityRepository.GetAllAsync(cancellationToken);
            var capability = capabilities.FirstOrDefault(c => c.Id.Value == request.Request.CapabilityId.Value);
            if (capability != null)
                capabilityFactor = capability.FactorPercentage;
        }
        else if (request.Request.CapabilityIds?.Any() == true)
        {
            var capabilities = await _capabilityRepository.GetAllAsync(cancellationToken);
            capabilityFactors = capabilities
                .Where(c => request.Request.CapabilityIds.Contains(c.Id.Value))
                .Select(c => c.FactorPercentage)
                .ToList();
        }

        // Material: single ID -> MaterialFactor, list IDs -> MaterialFactors
        decimal materialFactor = 0m;
        var materialFactors = new List<decimal>();
        
        if (request.Request.MaterialId.HasValue)
        {
            var materials = await _materialRepository.GetAllAsync(cancellationToken);
            var material = materials.FirstOrDefault(m => m.Id.Value == request.Request.MaterialId.Value);
            if (material != null)
                materialFactor = material.FactorPercentage;
        }
        else if (request.Request.MaterialIds?.Any() == true)
        {
            var materials = await _materialRepository.GetAllAsync(cancellationToken);
            materialFactors = materials
                .Where(m => request.Request.MaterialIds.Contains(m.Id.Value))
                .Select(m => m.FactorPercentage)
                .ToList();
        }

        // Topic: single ID -> TopicFactor, list IDs -> TopicFactors
        decimal topicFactor = 0m;
        var topicFactors = new List<decimal>();
        
        if (request.Request.TopicId.HasValue)
        {
            var topics = await _topicRepository.GetAllAsync(cancellationToken);
            var topic = topics.FirstOrDefault(t => t.Id.Value == request.Request.TopicId.Value);
            if (topic != null)
                topicFactor = topic.FactorPercentage;
        }
        else if (request.Request.TopicIds?.Any() == true)
        {
            var topics = await _topicRepository.GetAllAsync(cancellationToken);
            topicFactors = topics
                .Where(t => request.Request.TopicIds.Contains(t.Id.Value))
                .Select(t => t.FactorPercentage)
                .ToList();
        }

        // AssemblyMethod: single ID -> AssemblyMethodFactor, list IDs -> AssemblyMethodFactors
        decimal assemblyMethodFactor = 0m;
        var assemblyMethodFactors = new List<decimal>();
        
        if (request.Request.AssemblyMethodId.HasValue)
        {
            var assemblyMethods = await _assemblyMethodRepository.GetAllAsync(cancellationToken);
            var assemblyMethod = assemblyMethods.FirstOrDefault(am => am.Id.Value == request.Request.AssemblyMethodId.Value);
            if (assemblyMethod != null)
                assemblyMethodFactor = assemblyMethod.FactorPercentage;
        }
        else if (request.Request.AssemblyMethodIds?.Any() == true)
        {
            var assemblyMethods = await _assemblyMethodRepository.GetAllAsync(cancellationToken);
            assemblyMethodFactors = assemblyMethods
                .Where(am => request.Request.AssemblyMethodIds.Contains(am.Id.Value))
                .Select(am => am.FactorPercentage)
                .ToList();
        }

        // Step 3: Create NCalc Expression
        var expression = new Expression(formula.Expression);

        // Step 4: Register Custom Functions
        expression.EvaluateFunction += (name, args) =>
        {
            if (name == "SUM")
            {
                var list = args.Parameters[0].Evaluate() as List<double>;
                args.Result = list?.Sum() ?? 0.0;
            }

            if (name == "PRODUCT")
            {
                var list = args.Parameters[0].Evaluate() as List<double>;
                args.Result = list == null || !list.Any()
                    ? 0.0
                    : list.Aggregate(1.0, (a, b) => a * b);
            }

            if (name == "AVG")
            {
                var list = args.Parameters[0].Evaluate() as List<double>;
                args.Result = list == null || !list.Any()
                    ? 0.0
                    : list.Average();
            }
        };

        // Step 5: Define All Formula Parameters - Must match FormulaVariable enum
        // Define both single factors and list factors (formula can use either)
        // Note: NCalc works with doubles internally, so we convert decimals to doubles
        
        // Single factors (default: 0.0)
        expression.Parameters["CapabilityFactor"] = (double)capabilityFactor;
        expression.Parameters["MaterialFactor"] = (double)materialFactor;
        expression.Parameters["TopicFactor"] = (double)topicFactor;
        expression.Parameters["AssemblyMethodFactor"] = (double)assemblyMethodFactor;
        
        // List factors (default: empty list)
        expression.Parameters["CapabilityFactors"] = (capabilityFactors ?? new List<decimal>()).Select(x => (double)x).ToList();
        expression.Parameters["MaterialFactors"] = (materialFactors ?? new List<decimal>()).Select(x => (double)x).ToList();
        expression.Parameters["TopicFactors"] = (topicFactors ?? new List<decimal>()).Select(x => (double)x).ToList();
        expression.Parameters["AssemblyMethodFactors"] = (assemblyMethodFactors ?? new List<decimal>()).Select(x => (double)x).ToList();
        
        // Scalar parameters
        expression.Parameters["PieceCount"] = (double)(request.Request.PieceCount ?? 0m);

        // Verify all possible formula variables are defined
        var expectedVariables = new[] 
        { 
            "CapabilityFactor",
            "CapabilityFactors",
            "MaterialFactor",
            "MaterialFactors", 
            "TopicFactor",
            "TopicFactors",
            "AssemblyMethodFactor",
            "AssemblyMethodFactors",
            "PieceCount" 
        };

        foreach (var variable in expectedVariables)
        {
            if (!expression.Parameters.ContainsKey(variable))
            {
                return Result.Failure<FormulaCalculateResponse>(
                    SharedKernel.Domain.Errors.Error.Validation("Formula.MissingParameter", 
                        $"Required parameter '{variable}' is not defined for formula evaluation"));
            }
        }

        // Step 6: Evaluate Expression
        decimal rawResult;
        try
        {
            var result = expression.Evaluate();
            rawResult = Convert.ToDecimal(result);
        }
        catch (Exception ex)
        {
            return Result.Failure<FormulaCalculateResponse>(
                SharedKernel.Domain.Errors.Error.Validation("Formula.EvaluationError", $"Expression evaluation failed: {ex.Message}"));
        }

        // Step 7: Validation Logic
        string? validationOutput = null;
        if (formula.IsNeedValidation)
        {
            var validations = await _validationRepository.GetAllAsync(cancellationToken);
            
            // Filter validations for this formula
            var formulaValidations = validations
                .Where(v => v.FormulaId == formula.Id)
                .ToList();

            if (formulaValidations.Any())
            {
                var matchedValidation = formulaValidations.FirstOrDefault(v =>
                    rawResult >= v.MinValue &&
                    rawResult <= v.MaxValue);

                if (matchedValidation is not null)
                {
                    validationOutput = matchedValidation.Output;
                }
                else
                {
                    // No range matched for this result
                    return Result.Failure<FormulaCalculateResponse>(
                        SharedKernel.Domain.Errors.Error.Validation("Formula.ValidationFailed", 
                            $"Result {rawResult} does not match any validation range. Available ranges: {string.Join(", ", formulaValidations.Select(v => $"[{v.MinValue}-{v.MaxValue}]"))}"));
                }
            }
            else
            {
                // Formula requires validation but no ranges defined
                return Result.Failure<FormulaCalculateResponse>(
                    SharedKernel.Domain.Errors.Error.Validation("Formula.NoValidationRanges", 
                        $"Formula '{request.FormulaCode}' requires validation but no validation ranges are defined"));
            }
        }

        // Step 8: Return Result
        var roundedResult = Math.Round(rawResult);
        return Result.Success(new FormulaCalculateResponse(roundedResult, validationOutput));
    }
}



