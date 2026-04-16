using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzKit3D.Modules.InStock.Application.UseCases.InstockProducts.Queries.GetInstockProductById;

public sealed record GetInstockProductByIdDriveDetailDto(
    Guid DriveId,
    string DriveName,
    int Quantity);