using PuzKit3D.SharedKernel.Domain.Errors;

namespace PuzKit3D.Modules.User.Domain.Entities;

public static class UserError
{
    public static readonly Error InvalidCredentials = Error.Failure(
        "User.InvalidCredentials",
        "Email ho?c m?t kh?u không ?úng");

    public static readonly Error EmailNotFound = Error.NotFound(
        "User.EmailNotFound",
        "Không tìm th?y ng??i dùng v?i email này");

    public static readonly Error AccountLocked = Error.Failure(
        "User.AccountLocked",
        "Tài kho?n ?ã b? khóa");

    public static readonly Error EmailNotConfirmed = Error.Failure(
        "User.EmailNotConfirmed",
        "Email ch?a ???c xác nh?n");
}
