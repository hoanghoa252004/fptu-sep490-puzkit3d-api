using PuzKit3D.Modules.Feedback.Application.Repositories;
using PuzKit3D.Modules.Feedback.Domain.Entities.Feedbacks;
using PuzKit3D.SharedKernel.Application.Message.Query;
using PuzKit3D.SharedKernel.Domain.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzKit3D.Modules.Feedback.Application.UseCases.Feedbacks.Queries.GetFeedbackById;

public sealed class GetFeedbackByIdQueryHandler : IQueryHandler<GetFeedbackByIdQuery, FeedbackResponseDto>
{
    private readonly IFeedbackRepository _feedbackRepository;

    public GetFeedbackByIdQueryHandler(
        IFeedbackRepository feedbackRepository)
    {
        _feedbackRepository = feedbackRepository;
    }
    public async Task<ResultT<FeedbackResponseDto>> Handle(GetFeedbackByIdQuery request, CancellationToken cancellationToken)
    {
        // Check if feedback exists
        var feedback = await _feedbackRepository.GetByIdAsync(
            FeedbackId.From(request.FeedbackId),
            cancellationToken);

        if (feedback is null)
        {
            return Result.Failure<FeedbackResponseDto>(FeedbackError.FeedbackNotFound(request.FeedbackId));
        }
        return Result.Success(new FeedbackResponseDto(
            feedback.Id.Value,
            feedback.UserId,
            feedback.Rating,
            feedback.Comment,
            feedback.CreatedAt,
            feedback.UpdatedAt));
    }
}
