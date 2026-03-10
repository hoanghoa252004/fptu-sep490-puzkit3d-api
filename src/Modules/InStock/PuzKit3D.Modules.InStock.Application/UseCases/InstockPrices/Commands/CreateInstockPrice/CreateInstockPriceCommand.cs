using PuzKit3D.SharedKernel.Application.Message.Command;

namespace PuzKit3D.Modules.InStock.Application.UseCases.InstockPrices.Commands.CreateInstockPrice;

public sealed record CreateInstockPriceCommand(
    string Name,
    DateTime EffectiveFrom,
    DateTime EffectiveTo,
    int Priority,
    bool IsActive = false) : ICommandT<Guid>;
