using PuzKit3D.SharedKernel.Application.Message.Command;

namespace PuzKit3D.Modules.Catalog.Application.UseCases.Topics.Commands.CreateTopic;

public sealed record CreateTopicCommand(
    string Name,
    string Slug,
    Guid? ParentId,
    decimal FactorPercentage,
    string? Description,
    bool IsActive) : ICommandT<Guid>;
