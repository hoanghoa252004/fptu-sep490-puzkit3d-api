namespace PuzKit3D.Modules.Partner.Domain.Entities.PartnerProductRequests;

public static class PartnerProductRequestStatusTransition
{
    private static readonly Dictionary<PartnerProductRequestStatus, HashSet<PartnerProductRequestStatus>> AllowedTransitions = new()
    {
        // Pending: Customer tạo request, chờ staff xử lý
        {
            PartnerProductRequestStatus.Pending, new HashSet<PartnerProductRequestStatus>
            {
                PartnerProductRequestStatus.Approved,      // Staff đồng ý request
                PartnerProductRequestStatus.CancelledByStaff // Staff từ chối request
            }
        },
        // Approved: Staff đã đồng ý, chờ tạo quotation hoặc customer hủy
        {
            PartnerProductRequestStatus.Approved, new HashSet<PartnerProductRequestStatus>
            {
                PartnerProductRequestStatus.Quoted,        // Quotation đã được tạo
                PartnerProductRequestStatus.CancelledByCustomer // Customer hủy request
            }
        },
        // Quoted: Quotation đã được tạo, chờ customer phản hồi
        {
            PartnerProductRequestStatus.Quoted, new HashSet<PartnerProductRequestStatus>
            {
                PartnerProductRequestStatus.Accepted,      // Customer đồng ý quotation
                PartnerProductRequestStatus.RejectedByCustomer, // Customer từ chối quotation
                PartnerProductRequestStatus.CancelledByCustomer  // Customer hủy quotation
            }
        },
        // Accepted: Customer đã đồng ý quotation (trạng thái cuối)
        {
            PartnerProductRequestStatus.Accepted, new HashSet<PartnerProductRequestStatus>()
        },
        // CancelledByStaff: Staff đã hủy (trạng thái cuối)
        {
            PartnerProductRequestStatus.CancelledByStaff, new HashSet<PartnerProductRequestStatus>()
        },
        // RejectedByCustomer: Customer từ chối quotation (trạng thái cuối)
        {
            PartnerProductRequestStatus.RejectedByCustomer, new HashSet<PartnerProductRequestStatus>()
        },
        // CancelledByCustomer: Customer hủy request (trạng thái cuối)
        {
            PartnerProductRequestStatus.CancelledByCustomer, new HashSet<PartnerProductRequestStatus>()
        }
    };

    public static bool IsValidTransition(
        PartnerProductRequestStatus currentStatus,
        PartnerProductRequestStatus newStatus)
    {
        if (!AllowedTransitions.ContainsKey(currentStatus))
        {
            return false;
        }

        return AllowedTransitions[currentStatus].Contains(newStatus);
    }

    public static IEnumerable<PartnerProductRequestStatus> GetAllowedTransitions(
        PartnerProductRequestStatus currentStatus)
    {
        return AllowedTransitions.ContainsKey(currentStatus)
            ? AllowedTransitions[currentStatus]
            : Enumerable.Empty<PartnerProductRequestStatus>();
    }
}
