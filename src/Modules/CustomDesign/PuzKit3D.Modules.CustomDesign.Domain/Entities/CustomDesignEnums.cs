namespace PuzKit3D.Modules.CustomDesign.Domain.Entities;

public enum DifficultyLevel
{
    Basic = 0,
    Intermediate = 1,
    Advanced = 2
}

public enum CustomDesignRequestStatus
{
    Draft = 0,
    Submitted = 1,
    UnderReview = 2,
    Quoted = 3,
    ProposalSent = 4,
    Accepted = 5,
    Rejected = 6,
    Cancelled = 7
}

public enum ProposalStatus
{
    Draft = 0,
    SubmittedToManager = 1,
    ManagerApproved = 2,
    SentToCustomer = 3,
    CustomerApproved = 4,
    Rejected = 5,
    Cancelled = 6
}

public enum WorkflowStatus
{
    NotStarted = 0,
    InProgress = 1,
    Completed = 2,
    OnHold = 3,
    Cancelled = 4,
    Failed = 5
}
