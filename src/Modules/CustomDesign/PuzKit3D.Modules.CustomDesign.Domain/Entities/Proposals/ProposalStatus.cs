namespace PuzKit3D.Modules.CustomDesign.Domain.Entities.Proposals;

public enum ProposalStatus
{
    Draft = 0, // Proposal được staff tạo, chưa gửi duyệt
    ApprovedByManager = 1, // manager duyệt
    RejectedByManager = 2, // manager từ chối
    ApprovedByCustomer = 3, // customer duyệt
    RejectedByCustomer = 4, // customer từ chối
    Cancelled = 5, // hủy
    Expired = 6 // quá hạn
}
