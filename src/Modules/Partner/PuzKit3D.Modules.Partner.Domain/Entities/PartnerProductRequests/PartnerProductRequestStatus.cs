namespace PuzKit3D.Modules.Partner.Domain.Entities.PartnerProductRequests;

public enum PartnerProductRequestStatus
{
    Pending = 0,
    CancelledByStaff = 1,
    Approved = 2,
    Quoted = 4,
    Accepted = 5,
    RejectedByCustomer = 6,
    CancelledByCustomer = 7
}
