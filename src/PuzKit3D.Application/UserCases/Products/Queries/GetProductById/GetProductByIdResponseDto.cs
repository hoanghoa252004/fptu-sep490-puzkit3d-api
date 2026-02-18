using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzKit3D.Application.UserCases.Products.Queries.GetProductById;

public sealed record GetProductByIdResponseDto(
    int Id,
    string Name, 
    string Description
);
