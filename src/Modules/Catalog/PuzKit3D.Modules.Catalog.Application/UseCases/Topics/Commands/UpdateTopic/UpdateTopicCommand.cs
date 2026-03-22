using PuzKit3D.SharedKernel.Application.Message.Command;

namespace PuzKit3D.Modules.Catalog.Application.UseCases.Topics.Commands.UpdateTopic;

public sealed record UpdateTopicCommand(
    Guid Id,
    string? Name,
    string? Slug,
    Guid? ParentId,
    string? Description,
    bool? IsActive) : ICommand;

