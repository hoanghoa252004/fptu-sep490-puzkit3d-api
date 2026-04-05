using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzKit3D.Modules.CustomDesign.Domain.Entities.CustomDesignRequests;

public enum CustomDesignRequestStatus
{
    Submitted,           // Customer submitted request
    MissingInformation,  // Need more info from customer
    Approved,            // Staff approved request
    Processing,          // System is processing (milestones / AI / work)
    Completed,           // All work finished
    Rejected,            // Rejected by staff
    Cancelled,           // Cancelled by customer
    Expired              // No response from customer (timeout)
}
