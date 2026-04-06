using MediatR;
using PuzKit3D.SharedKernel.Application.Message.Command;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.InStock.Application.UseCases.InstockOrderConfigs.Commands.UpdateInstockOrderConfig;

public record UpdateInstockOrderConfigCommand(
    int? OrderMustCompleteInDays) : ICommand;
