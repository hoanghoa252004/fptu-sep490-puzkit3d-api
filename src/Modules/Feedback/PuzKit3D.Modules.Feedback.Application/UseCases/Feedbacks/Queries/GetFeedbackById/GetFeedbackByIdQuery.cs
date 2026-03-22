using PuzKit3D.SharedKernel.Application.Message.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzKit3D.Modules.Feedback.Application.UseCases.Feedbacks.Queries.GetFeedbackById;

public sealed record GetFeedbackByIdQuery(
    
    Guid FeedbackId) : IQuery<FeedbackResponseDto>;

public sealed record FeedbackResponseDto(
    Guid Id,
    Guid UserId,
    int Rating,
    string? Comment,
    DateTime CreatedAt,
    DateTime UpdatedAt);