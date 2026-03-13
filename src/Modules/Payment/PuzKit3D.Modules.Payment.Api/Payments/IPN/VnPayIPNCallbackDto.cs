namespace PuzKit3D.Modules.Payment.Api.Payments.IPN;

public class VnPayIPNCallbackDto
{
    public string vnp_TmnCode { get; set; } = null!;
    public string vnp_Amount { get; set; } = null!;
    public string vnp_BankCode { get; set; } = null!;
    public string? vnp_BankTranNo { get; set; }
    public string? vnp_CardType { get; set; }
    public string? vnp_PayDate { get; set; }
    public string vnp_OrderInfo { get; set; } = null!;
    public string vnp_TransactionNo { get; set; } = null!;
    public string vnp_ResponseCode { get; set; } = null!;
    public string vnp_TransactionStatus { get; set; } = null!;
    public string vnp_TxnRef { get; set; } = null!;
    public string vnp_SecureHash { get; set; } = null!;
}

public class VnPayIPNResponseDto
{
    public string RspCode { get; set; } = null!;
    public string Message { get; set; } = null!;
}
