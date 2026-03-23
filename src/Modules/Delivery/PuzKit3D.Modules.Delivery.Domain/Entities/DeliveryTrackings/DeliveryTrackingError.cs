using PuzKit3D.SharedKernel.Domain.Errors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzKit3D.Modules.Delivery.Domain.Entities.DeliveryTrackings;


public static class DeliveryTrackingError
{
    public static Error InvalidOrderId() =>
        Error.Validation("INVALID_ORDER_ID", "Order ID cannot be empty");

    public static Error InvalidDeliveryOrderCode() =>
        Error.Validation("INVALID_DELIVERY_ORDER_CODE", "Delivery order code cannot be empty");

    public static Error InvalidExpectedDeliveryDate() =>
        Error.Validation("INVALID_EXPECTED_DELIVERY_DATE", "Expected delivery date must be in the future");

    public static Error InvalidDetail() =>
        Error.Validation("INVALID_DETAIL", "Delivery tracking detail cannot be null");

    public static Error ItemAlreadyExists(Guid itemId) =>
        Error.Validation("ITEM_ALREADY_EXISTS", $"Item {itemId} already exists in this delivery");

    public static Error InvalidStatusTransition(DeliveryTrackingStatus from, DeliveryTrackingStatus to) =>
        Error.Validation("INVALID_STATUS_TRANSITION", $"Cannot transition from {from} to {to}");

    public static Error AlreadyReturned() =>
        Error.Validation("ALREADY_RETURNED", "Item has already been returned");
}
