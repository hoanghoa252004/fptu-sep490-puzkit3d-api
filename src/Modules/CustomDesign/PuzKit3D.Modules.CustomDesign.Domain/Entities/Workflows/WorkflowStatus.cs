namespace PuzKit3D.Modules.CustomDesign.Domain.Entities.Workflows;

public enum WorkflowStatus
{
    Draft = 0, // tạo nhưng cus ch accept proposal
    Waiting = 1, // Customer accept chờ thanh toán
    Pending = 2, // chờ thanh toán
    ReadyToStart = 3,
    InProgress = 4, // đang thực hiện
    Done = 5, // hoàn thành đợi review
    RejectedByCustomer = 6,
    Rejected = 7,
    CancelledByStaff = 8,
    CancelledByCustomer = 9,
    Completed = 10 // hoàn thành
}
