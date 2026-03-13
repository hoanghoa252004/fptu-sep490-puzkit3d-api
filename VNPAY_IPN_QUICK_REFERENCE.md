# VNPAY IPN Implementation - Quick Reference

## ?? File Structure

```
src/Modules/Payment/
??? PuzKit3D.Modules.Payment.Api/
?   ??? Payments/IPN/
?       ??? IPN.cs                          ? Main endpoint
?       ??? VnPayIPNCallbackDto.cs          ? Request/Response DTOs
??? PuzKit3D.Modules.Payment.Application/
?   ??? Abstractions/
?   ?   ??? IVnPaySignatureValidator.cs    ? Interface for validation
?   ??? UseCases/Transactions/Commands/ProcessVnPayIPN/
?       ??? ProcessVnPayIPNCommand.cs       ? MediatR command
?       ??? ProcessVnPayIPNCommandHandler.cs ? Handler implementation
??? PuzKit3D.Modules.Payment.Domain/
?   ??? Entities/Transactions/
?       ??? Transaction.cs                  ? Updated with new methods
??? PuzKit3D.Modules.Payment.Infrastructure/
    ??? DependencyInjection.cs               ? Updated registration
    ??? PaymentGateways/VNPAY/
        ??? VnPaySignatureValidator.cs      ? Validator implementation
```

## ?? Request Flow

```
VNPAY Server
     ?
     ??? GET /IPN?vnp_Amount=...&vnp_SecureHash=...
     ?
     ??? IPN Endpoint (IPN.cs)
           ?
           ??? ProcessVnPayIPNCommand
           ?
           ??? ProcessVnPayIPNCommandHandler
                 ?
                 ??? IVnPaySignatureValidator.ValidateSignature()
                 ?
                 ??? ITransactionRepository.FindAsync()
                 ?
                 ??? IPaymentRepository.GetByIdAsync()
                 ?
                 ??? transaction.UpdateToSuccess() or UpdateToFailed()
                 ?
                 ??? payment.UpdateStatus()
                 ?
                 ??? unitOfWork.SaveChangesAsync()
                     ?
                     ??? Response (RspCode: 00, 01, 02, 04, 97, 99)
```

## ?? Key Methods

### IPN Endpoint
```csharp
// GET /IPN
app.MapGet("/IPN", async (
    HttpContext httpContext,
    ISender sender,
    CancellationToken cancellationToken) => { ... })
```

### Signature Validation
```csharp
// Validate HMACSHA512 checksum
public bool ValidateSignature(IQueryCollection queryCollection, string secureHash)
```

### Transaction Status Update
```csharp
// Update to success
public Result UpdateToSuccess(string? transactionNo = null, string? rawResponsePayload = null)

// Update to failed
public Result UpdateToFailed(string? rawResponsePayload = null)
```

## ?? Response Codes Matrix

| Scenario | Code | Status | Action |
|----------|------|--------|--------|
| Success | 00 | 200 | Both transaction & payment updated to paid/success |
| Not Found | 01 | 400 | Transaction not found by TxnRef |
| Already Confirmed | 02 | 200 | Transaction status is not Pending |
| Invalid Amount | 04 | 400 | Amount mismatch with order |
| Invalid Signature | 97 | 400 | HMACSHA512 validation failed |
| Internal Error | 99 | 500 | Database or processing error |
| No Parameters | 99 | 400 | No query parameters received |

## ?? Configuration Required

```json
{
  "VNPAY": {
    "HashSecret": "your-secret-key",
    "TmnCode": "your-merchant-code",
    "BaseUrl": "https://sandbox.vnpayment.vn/paygate/...",
    "ReturnUrl": "https://yourdomain.com/...",
    "Command": "pay",
    "Version": "2.1.0",
    "Locale": "vn",
    "CurrCode": "VND",
    "OrderType": "other",
    "BankCode": ""
  }
}
```

## ?? Database Changes

### Transaction
- `Status`: Pending ? Success/Failed
- `TransactionNo`: vnp_TransactionNo (from VNPAY)
- `UpdatedAt`: Current UTC time

### Payment
- `Status`: Pending ? Paid/Failed
- `PaidAt`: Current UTC time (only for success)
- `UpdatedAt`: Current UTC time

## ?? Quick Test

### Using cURL
```bash
curl "http://localhost:7001/IPN?vnp_TmnCode=TEST&vnp_Amount=1000000&vnp_BankCode=NCB&vnp_OrderInfo=Test&vnp_TransactionNo=123456&vnp_ResponseCode=00&vnp_TransactionStatus=00&vnp_TxnRef=999&vnp_SecureHash=xxx"
```

### Expected Success Response
```json
{
  "RspCode": "00",
  "Message": "Confirm Success"
}
```

## ?? Important Notes

1. **Signature Validation is Mandatory**
   - Every callback MUST be validated
   - Never skip signature check
   - Invalid signatures return 400 Bad Request

2. **Idempotency is Built-in**
   - Safe to retry failed callbacks
   - Already processed transactions return 02
   - Client can retry without side effects

3. **Amount is in Cents**
   - VNPAY sends amount × 100
   - Handler divides by 100 to get VND
   - Always validate exact amount match

4. **Success Requires BOTH Codes**
   - ResponseCode = "00" AND TransactionStatus = "00"
   - Any other combination is failure
   - Handler logs both for debugging

5. **Logging is Comprehensive**
   - All major steps are logged
   - PaymentId and TxnRef in each log
   - Check logs for debugging

## ?? Quick Troubleshooting

| Error | Cause | Fix |
|-------|-------|-----|
| RspCode 97 | Bad signature | Verify HashSecret |
| RspCode 01 | No transaction | Create transaction first |
| RspCode 04 | Amount mismatch | Check ÷100 conversion |
| RspCode 02 | Already processed | Normal behavior on retry |
| RspCode 99 | System error | Check logs for details |

## ?? Implementation Checklist

- [x] IPN endpoint created
- [x] Signature validation implemented
- [x] Transaction status update logic
- [x] Payment status update logic
- [x] Error handling and response codes
- [x] Logging and debugging
- [x] Dependency injection configured
- [x] Database integration
- [x] Documentation complete
- [x] Code compiled and verified

## ?? Monitoring Recommendations

```
Metrics to track:
- Payment success rate (RspCode 00)
- Failed transactions (ResponseCode != 00)
- Duplicate attempts (RspCode 02)
- Validation failures (RspCode 97)
- Processing errors (RspCode 99)
- Average processing time
- Peak transaction times
```

---

**Status**: ? Ready for Testing
**Build Status**: ? Successful
**Documentation**: ? Complete
