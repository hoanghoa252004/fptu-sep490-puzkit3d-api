using PuzKit3D.SharedKernel.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzKit3D.Modules.InStock.Domain.Entities.Products;

public sealed class ProductId : StronglyTypedId<Guid>
{
    public ProductId(Guid value) : base(value) { }
}