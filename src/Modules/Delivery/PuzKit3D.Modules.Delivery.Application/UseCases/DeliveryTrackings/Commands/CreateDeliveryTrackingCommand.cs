using PuzKit3D.Modules.Delivery.Domain.Entities.DeliveryTrackings;
using PuzKit3D.SharedKernel.Application.Message.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PuzKit3D.Modules.Delivery.Application.UseCases.DeliveryTrackings.Commands;

public sealed record CreateDeliveryTrackingCommand(Guid OrderId, DeliveryTrackingType DeliveryTrackingType) : ICommandT<Guid>;
