using PuzKit3D.SharedKernel.Application.Message.Command;

namespace PuzKit3D.Modules.Partner.Application.UseCases.PartnerProducts.Commands.DeletePartnerProduct;

public sealed record DeletePartnerProductCommand(Guid Id) : ICommand;
