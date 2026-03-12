namespace PuzKit3D.Modules.InStock.Domain.Entities.InstockOrders;

public static class InstockOrderStatusTransition
{
    private static readonly Dictionary<InstockOrderStatus, HashSet<InstockOrderStatus>> AllowedTransitions = new()
    {
        {
            InstockOrderStatus.PaymentPending, new HashSet<InstockOrderStatus>
            {
                InstockOrderStatus.Paid,
                InstockOrderStatus.Expired
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
                InstockOrderStatus.Shipped
            }
        },
        {
            InstockOrderStatus.Shipped, new HashSet<InstockOrderStatus>
            {
                InstockOrderStatus.Completed
            }
        },
        {
            InstockOrderStatus.Expired, new HashSet<InstockOrderStatus>()
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
        return "PaymentPending -> [Paid | Expired], Paid -> Processing -> Shipped -> Completed";
    }
}
