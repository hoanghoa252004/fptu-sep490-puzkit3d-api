using PuzKit3D.SharedKernel.Application.Message.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzKit3D.Modules.Delivery.Application.UseCases.DeliveryTrackings.Queries.GetWayBillNumber;

public sealed record GetWayBillNumberQuery(Guid DeliveryTrackingId) : IQuery<string>;
