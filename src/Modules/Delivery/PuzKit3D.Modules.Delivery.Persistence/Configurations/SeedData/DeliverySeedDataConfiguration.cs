using Microsoft.EntityFrameworkCore;
using PuzKit3D.Modules.Delivery.Domain.Entities.Replicas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzKit3D.Modules.Delivery.Persistence.Configurations.SeedData;

internal static class DeliverySeedDataConfiguration
{
    private static readonly DateTime SeedDate = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);

}
