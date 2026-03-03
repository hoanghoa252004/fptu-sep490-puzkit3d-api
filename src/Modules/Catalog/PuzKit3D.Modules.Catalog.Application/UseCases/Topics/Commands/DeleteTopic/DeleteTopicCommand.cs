using PuzKit3D.SharedKernel.Application.Message.Command;

namespace PuzKit3D.Modules.Catalog.Application.UseCases.Topics.Commands.DeleteTopic;

public sealed record DeleteTopicCommand(Guid Id) : ICommand;
