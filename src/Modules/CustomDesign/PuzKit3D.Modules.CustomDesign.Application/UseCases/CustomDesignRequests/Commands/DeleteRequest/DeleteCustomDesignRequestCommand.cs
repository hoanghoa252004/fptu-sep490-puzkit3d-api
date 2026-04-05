using PuzKit3D.SharedKernel.Application.Message.Command;

namespace PuzKit3D.Modules.CustomDesign.Application.UseCases.CustomDesignRequests.Commands.DeleteRequest;

public sealed record DeleteCustomDesignRequestCommand(Guid Id) : ICommand;
