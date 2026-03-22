using PuzKit3D.SharedKernel.Application.Message.Command;

namespace PuzKit3D.Modules.Partner.Application.UseCases.PartnerProducts.Commands.PatchPartnerProduct;

public sealed record PatchPartnerProductCommand(Guid Id) : ICommand;
