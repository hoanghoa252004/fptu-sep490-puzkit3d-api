namespace PuzKit3D.Modules.Partner.Domain.Entities.PartnerProductQuotations;

public enum PartnerProductQuotationStatus
{
    Pending = 0,
    CancelledByStaff = 1,
    Quoted = 4,
    Accepted = 5,
    RejectedByCustomer = 6,
    CancelledByCustomer = 7
}
