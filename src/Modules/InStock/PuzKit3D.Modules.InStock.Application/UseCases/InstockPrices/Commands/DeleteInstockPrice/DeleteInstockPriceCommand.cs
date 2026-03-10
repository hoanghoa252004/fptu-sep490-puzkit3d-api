using PuzKit3D.SharedKernel.Application.Message.Command;

namespace PuzKit3D.Modules.InStock.Application.UseCases.InstockPrices.Commands.DeleteInstockPrice;

public sealed record DeleteInstockPriceCommand(Guid PriceId) : ICommand;
