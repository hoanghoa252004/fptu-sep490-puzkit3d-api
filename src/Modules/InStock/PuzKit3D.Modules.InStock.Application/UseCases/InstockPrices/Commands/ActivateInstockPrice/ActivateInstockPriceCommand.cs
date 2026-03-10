using PuzKit3D.SharedKernel.Application.Message.Command;

namespace PuzKit3D.Modules.InStock.Application.UseCases.InstockPrices.Commands.ActivateInstockPrice;

public sealed record ActivateInstockPriceCommand(Guid PriceId) : ICommand;
