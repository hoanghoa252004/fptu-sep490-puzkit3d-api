namespace PuzKit3D.Modules.Partner.Domain.Entities.PartnerProductQuotations;

public static class PartnerProductQuotationStatusTransition
{
    private static readonly Dictionary<PartnerProductQuotationStatus, HashSet<PartnerProductQuotationStatus>> AllowedTransitions = new()
    {
        // Quoted: Quotation đã được tạo, chờ customer phản hồi
        {
            PartnerProductQuotationStatus.Quoted, new HashSet<PartnerProductQuotationStatus>
            {
                PartnerProductQuotationStatus.Accepted,    // Customer đồng ý quotation
                PartnerProductQuotationStatus.RejectedByCustomer, // Customer từ chối quotation
                PartnerProductQuotationStatus.CancelledByStaff,   // Staff hủy quotation
                PartnerProductQuotationStatus.CancelledByCustomer // Customer hủy quotation
            }
        },
        // Accepted: Customer đã đồng ý quotation (trạng thái cuối)
        {
            PartnerProductQuotationStatus.Accepted, new HashSet<PartnerProductQuotationStatus>()
        },
        // RejectedByCustomer: Customer từ chối quotation (trạng thái cuối)
        {
            PartnerProductQuotationStatus.RejectedByCustomer, new HashSet<PartnerProductQuotationStatus>()
        },
        // CancelledByStaff: Staff hủy quotation (trạng thái cuối)
        {
            PartnerProductQuotationStatus.CancelledByStaff, new HashSet<PartnerProductQuotationStatus>()
        },
        // CancelledByCustomer: Customer hủy quotation (trạng thái cuối)
        {
            PartnerProductQuotationStatus.CancelledByCustomer, new HashSet<PartnerProductQuotationStatus>()
        }
    };

    public static bool IsValidTransition(
        PartnerProductQuotationStatus currentStatus,
        PartnerProductQuotationStatus newStatus)
    {
        if (!AllowedTransitions.ContainsKey(currentStatus))
        {
            return false;
        }

        return AllowedTransitions[currentStatus].Contains(newStatus);
    }

    public static IEnumerable<PartnerProductQuotationStatus> GetAllowedTransitions(
        PartnerProductQuotationStatus currentStatus)
    {
        return AllowedTransitions.ContainsKey(currentStatus)
            ? AllowedTransitions[currentStatus]
            : Enumerable.Empty<PartnerProductQuotationStatus>();
    }
}
