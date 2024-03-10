﻿using MediatR;
using PostsApp.Application.Common.Interfaces;
using PostsApp.Application.Common.Results;
using PostsApp.Application.Reviews.Results;
using PostsApp.Domain.Exceptions;

namespace PostsApp.Application.Reviews.Commands.UpdateReview;

public class UpdateReviewCommandHandler : IRequestHandler<UpdateReviewCommand, ReviewResult>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateReviewCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ReviewResult> Handle(UpdateReviewCommand request, CancellationToken cancellationToken)
    {
        var review = await _unitOfWork.Reviews.GetSingleWhereAsync(review => review.Id == request.ReviewId);
        
        review!.Rating = request.Rating;
        review.Body = request.Body;

        await _unitOfWork.SaveAsync(cancellationToken);
        
        return new ReviewResult
        {
            Id = review.Id,
            BookId = review.Book.Id,
            Body = review.Body,
            Rating = review.Rating,
            User = new UserResult
            {
                Id = review.User.Id,
                Username = review.User.Username,
                Role = review.User.Role.Name
            }
        };
    }
}