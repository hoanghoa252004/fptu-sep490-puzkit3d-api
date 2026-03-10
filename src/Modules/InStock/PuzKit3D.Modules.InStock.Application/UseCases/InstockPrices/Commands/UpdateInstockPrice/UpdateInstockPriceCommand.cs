using PuzKit3D.SharedKernel.Application.Message.Command;

namespace PuzKit3D.Modules.InStock.Application.UseCases.InstockPrices.Commands.UpdateInstockPrice;

public sealed record UpdateInstockPriceCommand(
    Guid PriceId,
    string? Name = null,
    DateTime? EffectiveFrom = null,
    DateTime? EffectiveTo = null,
    int? Priority = null) : ICommand;
