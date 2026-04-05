using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzKit3D.Modules.CustomDesign.Domain.Entities.CustomDesignRequests;

public static class CustomDesignRequestStatusTransition
{
    private static readonly Dictionary<CustomDesignRequestStatus, HashSet<CustomDesignRequestStatus>> AllowedTransitions = new()
    {
        {
            CustomDesignRequestStatus.Submitted, new HashSet<CustomDesignRequestStatus>
            {
                CustomDesignRequestStatus.MissingInformation,
                CustomDesignRequestStatus.Rejected,
                CustomDesignRequestStatus.Approved
            }
        },
        {
            CustomDesignRequestStatus.Approved, new HashSet<CustomDesignRequestStatus>
            {
                CustomDesignRequestStatus.Processing
            }
        },
        {
            CustomDesignRequestStatus.Processing, new HashSet<CustomDesignRequestStatus>
            {
                CustomDesignRequestStatus.Expired,
                CustomDesignRequestStatus.Cancelled,
                CustomDesignRequestStatus.Completed
            }
        },
        {
            CustomDesignRequestStatus.Completed, new HashSet<CustomDesignRequestStatus>()
        },
        {
            CustomDesignRequestStatus.Expired, new HashSet<CustomDesignRequestStatus>()
        },
        {
            CustomDesignRequestStatus.Cancelled, new HashSet<CustomDesignRequestStatus>()
        },
        {
            CustomDesignRequestStatus.Rejected, new HashSet<CustomDesignRequestStatus>()
        },
        {
            CustomDesignRequestStatus.MissingInformation, new HashSet<CustomDesignRequestStatus>{
                CustomDesignRequestStatus.Submitted,
            }
        }
    };

    public static bool IsValidTransition(CustomDesignRequestStatus currentStatus, CustomDesignRequestStatus newStatus)
    {
        if (!AllowedTransitions.ContainsKey(currentStatus))
        {
            return false;
        }

        return AllowedTransitions[currentStatus].Contains(newStatus);
    }

    public static string GetAllValidStatus()
    {
        return string.Join(", ", Enum.GetNames(typeof(CustomDesignRequestStatus)));
    }
}
