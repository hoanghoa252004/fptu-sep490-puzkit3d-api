namespace PuzKit3D.Modules.Partner.Domain.Entities.PartnerProductRequests;

public enum PartnerProductRequestStatus
{
    Pending = 0,
    Cancelled = 1,
    Approved = 2,
    RejectedByStaff = 3,
    Quoted = 4,
    Accepted = 5,
    RejectedByCustomer = 6
}
