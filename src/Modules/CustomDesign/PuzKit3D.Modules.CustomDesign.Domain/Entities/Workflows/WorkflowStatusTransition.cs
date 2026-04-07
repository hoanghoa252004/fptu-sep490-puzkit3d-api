using System;
using System.Collections.Generic;

namespace PuzKit3D.Modules.CustomDesign.Domain.Entities.Workflows;

public static class WorkflowStatusTransition
{
    private static readonly Dictionary<WorkflowStatus, HashSet<WorkflowStatus>> AllowedTransitions = new()
    {
        // Draft (0): Tạo nhưng customer chưa accept proposal
        {
            WorkflowStatus.Draft, new HashSet<WorkflowStatus>
            {
                WorkflowStatus.Waiting,                 // Chờ customer accept
                WorkflowStatus.CancelledByStaff        // Hủy bởi staff
            }
        },
        // Waiting (1): Customer accept, chờ thanh toán
        {
            WorkflowStatus.Waiting, new HashSet<WorkflowStatus>
            {
                WorkflowStatus.Pending,                 // Chờ thanh toán
                WorkflowStatus.CancelledByCustomer,    // Hủy bởi customer
                WorkflowStatus.CancelledByStaff        // Hủy bởi staff
            }
        },
        // Pending (2): Chờ thanh toán
        {
            WorkflowStatus.Pending, new HashSet<WorkflowStatus>
            {
                WorkflowStatus.ReadyToStart,            // Thanh toán xong, sẵn sàng bắt đầu
                WorkflowStatus.CancelledByCustomer,    // Hủy bởi customer
                WorkflowStatus.CancelledByStaff        // Hủy bởi staff
            }
        },
        // ReadyToStart (3): Sẵn sàng bắt đầu
        {
            WorkflowStatus.ReadyToStart, new HashSet<WorkflowStatus>
            {
                WorkflowStatus.InProgress,              // Bắt đầu thực hiện
                WorkflowStatus.CancelledByCustomer,    // Hủy bởi customer
                WorkflowStatus.CancelledByStaff        // Hủy bởi staff
            }
        },
        // InProgress (4): Đang thực hiện
        {
            WorkflowStatus.InProgress, new HashSet<WorkflowStatus>
            {
                WorkflowStatus.Done,                    // Hoàn thành, đợi review
                WorkflowStatus.RejectedByCustomer,    // Customer từ chối
                WorkflowStatus.CancelledByStaff        // Hủy bởi staff
            }
        },
        // OnHold (không có trong enum gốc nhưng được dùng):
        // Thêm nếu WorkflowStatus có OnHold
        // Done (5): Hoàn thành, đợi review
        {
            WorkflowStatus.Done, new HashSet<WorkflowStatus>
            {
                WorkflowStatus.Completed,               // Đã review, hoàn thành cuối cùng
                WorkflowStatus.RejectedByCustomer,    // Customer từ chối
                WorkflowStatus.CancelledByStaff        // Hủy bởi staff
            }
        },
        // RejectedByCustomer (6): Customer từ chối
        {
            WorkflowStatus.RejectedByCustomer, new HashSet<WorkflowStatus>
            {
                WorkflowStatus.InProgress               // Quay lại thực hiện
            }
        },
        // Rejected (7): Từ chối (không rõ ai từ chối - cần xác định lại)
        {
            WorkflowStatus.Rejected, new HashSet<WorkflowStatus>()  // Trạng thái cuối
        },
        // CancelledByStaff (8): Hủy bởi staff
        {
            WorkflowStatus.CancelledByStaff, new HashSet<WorkflowStatus>()  // Trạng thái cuối
        },
        // CancelledByCustomer (9): Hủy bởi customer
        {
            WorkflowStatus.CancelledByCustomer, new HashSet<WorkflowStatus>()  // Trạng thái cuối
        },
        // Completed (10): Hoàn thành
        {
            WorkflowStatus.Completed, new HashSet<WorkflowStatus>()  // Trạng thái cuối
        }
    };

    public static bool IsValidTransition(WorkflowStatus currentStatus, WorkflowStatus newStatus)
    {
        if (!AllowedTransitions.ContainsKey(currentStatus))
        {
            return false;
        }

        return AllowedTransitions[currentStatus].Contains(newStatus);
    }

    public static string GetAllValidStatus()
    {
        return string.Join(", ", Enum.GetNames(typeof(WorkflowStatus)));
    }
}
