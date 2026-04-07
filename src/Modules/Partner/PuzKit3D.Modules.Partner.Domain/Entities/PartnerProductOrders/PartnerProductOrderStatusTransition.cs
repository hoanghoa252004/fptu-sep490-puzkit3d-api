namespace PuzKit3D.Modules.Partner.Domain.Entities.PartnerProductOrders;

public static class PartnerProductOrderStatusTransition
{
    private static readonly Dictionary<PartnerProductOrderStatus, HashSet<PartnerProductOrderStatus>> AllowedTransitions = new()
    {
        // Pending (0): Đơn hàng vừa được tạo
        {
            PartnerProductOrderStatus.Pending, new HashSet<PartnerProductOrderStatus>
            {
                PartnerProductOrderStatus.Paid,       // Thanh toán
                PartnerProductOrderStatus.Expired,    // Hết hạn
                PartnerProductOrderStatus.CancelledByCustomer, // Hủy bởi khách hàng
            }
        },
        // Paid (1): Khách hàng đã thanh toán
        {
            PartnerProductOrderStatus.Paid, new HashSet<PartnerProductOrderStatus>
            {
                PartnerProductOrderStatus.OrderedFromPartner, // Đã đặt hàng
                PartnerProductOrderStatus.CancelledByCustomer, // Hủy bởi khách hàng
                PartnerProductOrderStatus.CancelledByStaff // Hủy bởi staff
            }
        },
        // WatingForReorder (2): Đang chờ đặt hàng lại
        {
            PartnerProductOrderStatus.WatingForReorder, new HashSet<PartnerProductOrderStatus>
            {
                PartnerProductOrderStatus.OrderedFromPartner, // Đã đặt hàng
                PartnerProductOrderStatus.CancelledByCustomer, // Hủy bởi khách hàng
                PartnerProductOrderStatus.CancelledByStaff // Hủy bởi staff
            }
        },
        // OrderedFromPartner (3): Đã đặt hàng từ nhà cung cấp
        {
            PartnerProductOrderStatus.OrderedFromPartner, new HashSet<PartnerProductOrderStatus>
            {
                PartnerProductOrderStatus.ReceivedAtWarehouse // Hàng về kho
            }
        },
        // ReceivedAtWarehouse (4): Hàng đã về kho
        {
            PartnerProductOrderStatus.ReceivedAtWarehouse, new HashSet<PartnerProductOrderStatus>
            {
                PartnerProductOrderStatus.CheckingFailed, // Kiểm hàng không thành công
                PartnerProductOrderStatus.Processing      // Kiểm hàng thành công
            }
        },
        // CheckingFailed (5): Kiểm hàng không thành công
        {
            PartnerProductOrderStatus.CheckingFailed, new HashSet<PartnerProductOrderStatus>
            {
                PartnerProductOrderStatus.WatingForReorder, // Chờ đặt hàng lại từ nhà cung cấp
                PartnerProductOrderStatus.CancelledByCustomer, // Hủy bởi khách hàng
                PartnerProductOrderStatus.CancelledByStaff // Hủy bởi staff
            }
        },
        // Processing (6): Đang chuẩn bị hàng
        {
            PartnerProductOrderStatus.Processing, new HashSet<PartnerProductOrderStatus>
            {
                PartnerProductOrderStatus.HandedOverToDelivery, // Giao cho đơn vị vận chuyển
                PartnerProductOrderStatus.CancelledByStaff // Hủy bởi staff
            }
        },
        // HandedOverToDelivery (7): Giao cho vận chuyển
        {
            PartnerProductOrderStatus.HandedOverToDelivery, new HashSet<PartnerProductOrderStatus>
            {
                PartnerProductOrderStatus.Completed, // Hoàn tất
                PartnerProductOrderStatus.Returned   // Trả lại
            }
        },
        // Completed (8): Hoàn tất - Trạng thái cuối
        {
            PartnerProductOrderStatus.Completed, new HashSet<PartnerProductOrderStatus>()
        },
        // Expired (9): Hết hạn - Trạng thái cuối
        {
            PartnerProductOrderStatus.Expired, new HashSet<PartnerProductOrderStatus>()
        },
        // CancelledByCustomer (10): Hủy bởi khách hàng - Trạng thái cuối
        {
            PartnerProductOrderStatus.CancelledByCustomer, new HashSet<PartnerProductOrderStatus>()
        },
        // CancelledByStaff (11): Hủy bởi staff - Trạng thái cuối
        {
            PartnerProductOrderStatus.CancelledByStaff, new HashSet<PartnerProductOrderStatus>()
        },
        // Returned (12): Trả lại - Trạng thái cuối
        {
            PartnerProductOrderStatus.Returned, new HashSet<PartnerProductOrderStatus>()
        }
    };

    public static bool IsValidTransition(PartnerProductOrderStatus currentStatus, PartnerProductOrderStatus newStatus)
    {
        if (!AllowedTransitions.ContainsKey(currentStatus))
        {
            return false;
        }

        return AllowedTransitions[currentStatus].Contains(newStatus);
    }

    public static IEnumerable<PartnerProductOrderStatus> GetAllowedTransitions(PartnerProductOrderStatus currentStatus)
    {
        return AllowedTransitions.ContainsKey(currentStatus)
            ? AllowedTransitions[currentStatus]
            : Enumerable.Empty<PartnerProductOrderStatus>();
    }
}

