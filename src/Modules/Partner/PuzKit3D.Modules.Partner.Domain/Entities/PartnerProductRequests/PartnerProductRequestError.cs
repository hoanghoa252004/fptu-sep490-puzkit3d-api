using PuzKit3D.SharedKernel.Domain.Errors;

namespace PuzKit3D.Modules.Partner.Domain.Entities.PartnerProductRequests;

public static class PartnerProductRequestError
{
    public static Error InvalidCode() => Error.Validation(
        "PartnerProductRequest.InvalidCode",
        "Partner product request code cannot be empty.");

    public static Error InvalidQuantity() => Error.Validation(
        "PartnerProductRequest.InvalidQuantity",
        "Total requested quantity must be greater than 0.");

    public static Error NotFound(Guid id) => Error.NotFound(
        "PartnerProductRequest.NotFound",
        $"Partner product request with ID '{id}' was not found.");

    public static Error NotFoundByCode(string code) => Error.NotFound(
        "PartnerProductRequest.NotFoundByCode",
        $"Partner product request with code '{code}' was not found.");

    public static Error DuplicateCode(string code) => Error.Conflict(
        "PartnerProductRequest.DuplicateCode",
        $"Partner product request with code '{code}' already exists.");
}
