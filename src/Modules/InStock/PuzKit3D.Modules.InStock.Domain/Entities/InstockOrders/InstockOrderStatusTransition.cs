namespace PuzKit3D.Modules.InStock.Domain.Entities.InstockOrders;

public static class InstockOrderStatusTransition
{
    private static readonly Dictionary<InstockOrderStatus, HashSet<InstockOrderStatus>> AllowedTransitions = new()
    {
        {
            InstockOrderStatus.Waiting, new HashSet<InstockOrderStatus>
            {
                InstockOrderStatus.Processing,
                InstockOrderStatus.Cancelled
            }
        },
        {
            InstockOrderStatus.Pending, new HashSet<InstockOrderStatus>
            {
                InstockOrderStatus.Paid,
                InstockOrderStatus.Expired,
                InstockOrderStatus.Cancelled
            }
        },
        {
            InstockOrderStatus.Paid, new HashSet<InstockOrderStatus>
            {
                InstockOrderStatus.Processing
            }
        },
        {
            InstockOrderStatus.Processing, new HashSet<InstockOrderStatus>
            {
                InstockOrderStatus.HandedOverToDelivery
            }
        },
        {
            InstockOrderStatus.HandedOverToDelivery, new HashSet<InstockOrderStatus>
            {
                InstockOrderStatus.Shipping
            }
        },
        {
            InstockOrderStatus.Shipping, new HashSet<InstockOrderStatus>
            {
                InstockOrderStatus.Delivered,
                InstockOrderStatus.Rejected
            }
        },
        {
            InstockOrderStatus.Delivered, new HashSet<InstockOrderStatus>
            {
                InstockOrderStatus.Completed
            }
        },
        {
            InstockOrderStatus.Rejected, new HashSet<InstockOrderStatus>
            {
                InstockOrderStatus.Returned
            }
        },
        {
            InstockOrderStatus.Expired, new HashSet<InstockOrderStatus>()
        },
        {
            InstockOrderStatus.Cancelled, new HashSet<InstockOrderStatus>()
        },
        {
            InstockOrderStatus.Completed, new HashSet<InstockOrderStatus>()
        }
    };

    public static bool IsValidTransition(InstockOrderStatus currentStatus, InstockOrderStatus newStatus)
    {
        if (currentStatus == newStatus)
        {
            return true;
        }

        if (!AllowedTransitions.ContainsKey(currentStatus))
        {
            return false;
        }

        return AllowedTransitions[currentStatus].Contains(newStatus);
    }

    public static IEnumerable<InstockOrderStatus> GetAllowedNextStatuses(InstockOrderStatus currentStatus)
    {
        return AllowedTransitions.ContainsKey(currentStatus) 
            ? AllowedTransitions[currentStatus] 
            : Enumerable.Empty<InstockOrderStatus>();
    }

    public static string GetTransitionPath()
    {
        return "COD: Waiting -> [Processing | Expired | Cancelled], Processing -> Shipping -> Delivered -> Completed; Online: Pending -> [Paid | Expired | Cancelled], Paid -> Processing -> Shipping -> Delivered -> Completed";
    }
}
