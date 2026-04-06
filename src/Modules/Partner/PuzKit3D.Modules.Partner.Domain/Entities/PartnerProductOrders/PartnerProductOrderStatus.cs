namespace PuzKit3D.Modules.Partner.Domain.Entities.PartnerProductOrders;

public enum PartnerProductOrderStatus
{
    Pending = 0, // mới tạo order

    Paid = 1, // khách hàng đã thanh toán

    WatingForReorder = 2, // đang chờ đặt hàng từ đối tác cung cấp - cập nhật khi khách hàng chấp nhận gia hạn thời gian đặt hàng lại
    OrderedFromPartner = 3, // đã đặt hàng từ đối tác cung cấp - staff cập nhật tay
    ReceivedAtWarehouse = 4, // hàng đã về kho và sẵn sàng giao cho khách - lấy time cập nhật tự động trong ImportServiceConfig
    CheckingFailed = 5, // kiểm hàng không thành công - Staff cập nhật tay // thông báo cho khách hàng và yêu cầu khách hàng xác nhận lại đơn hàng hoặc hủy đơn hàng

    Processing = 6, // kiểm tra hàng thành công và đang chuẩn bị hàng cho khách - Staff cập nhật tay
    HandedOverToDelivery = 7, // đã giao cho đơn vị vận chuyển để giao cho khách
    Completed = 8, // đơn hàng đã hoàn tất

    Expired = 9, // đơn hàng đã hết hạn
    CancelledByCustomer = 10, // đơn hàng đã bị hủy bởi khách hàng
    CancelledByStaff = 11, // đơn hàng đã bị hủy bởi staff
    Returned = 12, // đơn hàng đã được trả lại
}
