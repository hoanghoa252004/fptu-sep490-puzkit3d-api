using System;
using System.Collections.Generic;

namespace PuzKit3D.Modules.CustomDesign.Domain.Entities.Proposals;

public static class ProposalStatusTransition
{
    private static readonly Dictionary<ProposalStatus, HashSet<ProposalStatus>> AllowedTransitions = new()
    {
        // Draft (0): Proposal vừa được tạo, chưa gửi duyệt
        {
            ProposalStatus.Draft, new HashSet<ProposalStatus>
            {
                ProposalStatus.ApprovedByManager,      // Gửi duyệt manager
                ProposalStatus.RejectedByManager,      // Manager từ chối
                ProposalStatus.Cancelled               // Hủy
            }
        },
        // ApprovedByManager (1): Manager đã duyệt
        {
            ProposalStatus.ApprovedByManager, new HashSet<ProposalStatus>
            {
                ProposalStatus.ApprovedByCustomer,     // Gửi cho customer
                ProposalStatus.RejectedByManager,      // Manager từ chối (có thể nhận lại)
                ProposalStatus.Cancelled               // Hủy
            }
        },
        // RejectedByManager (2): Manager từ chối
        {
            ProposalStatus.RejectedByManager, new HashSet<ProposalStatus>
            {
                ProposalStatus.Draft,                  // Quay lại Draft
                ProposalStatus.Cancelled               // Hủy
            }
        },
        // ApprovedByCustomer (3): Customer đã duyệt
        {
            ProposalStatus.ApprovedByCustomer, new HashSet<ProposalStatus>
            {
                ProposalStatus.Expired                 // Quá hạn (trạng thái cuối)
            }
        },
        // RejectedByCustomer (4): Customer từ chối
        {
            ProposalStatus.RejectedByCustomer, new HashSet<ProposalStatus>()  // Trạng thái cuối
        },
        // Cancelled (5): Đã hủy
        {
            ProposalStatus.Cancelled, new HashSet<ProposalStatus>()  // Trạng thái cuối
        },
        // Expired (6): Quá hạn
        {
            ProposalStatus.Expired, new HashSet<ProposalStatus>()  // Trạng thái cuối
        }
    };

    public static bool IsValidTransition(ProposalStatus currentStatus, ProposalStatus newStatus)
    {
        if (!AllowedTransitions.ContainsKey(currentStatus))
        {
            return false;
        }

        return AllowedTransitions[currentStatus].Contains(newStatus);
    }

    public static string GetAllValidStatus()
    {
        return string.Join(", ", Enum.GetNames(typeof(ProposalStatus)));
    }
}
