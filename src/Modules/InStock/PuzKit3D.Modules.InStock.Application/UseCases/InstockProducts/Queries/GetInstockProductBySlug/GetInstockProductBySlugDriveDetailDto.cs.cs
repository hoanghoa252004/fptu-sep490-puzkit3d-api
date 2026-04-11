using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzKit3D.Modules.InStock.Application.UseCases.InstockProducts.Queries.GetInstockProductBySlug;

public sealed record GetInstockProductBySlugDriveDetailDto(
    Guid DriveId,
    string DriveName,
    int Quantity);
