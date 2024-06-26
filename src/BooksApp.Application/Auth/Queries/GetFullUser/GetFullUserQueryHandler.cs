﻿using MediatR;
using PostsApp.Application.Common.Interfaces;
using PostsApp.Domain.User;
using PostsApp.Domain.User.ValueObjects;

namespace PostsApp.Application.Auth.Queries.GetFullUser;

internal sealed class GetFullUserQueryHandler : IRequestHandler<GetFullUserQuery, User?>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetFullUserQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public async Task<User?> Handle(GetFullUserQuery request, CancellationToken cancellationToken)
    {
        return await _unitOfWork.Users.GetSingleById(request.UserId);
    }
}