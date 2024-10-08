﻿using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace PostsApp.Common.Requirements;

public class AuthorizedRequirement : IAuthorizationRequirement {}

public class AuthorizedRequirementHandler : AuthorizationHandler<AuthorizedRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
        AuthorizedRequirement requirement)
    {
        if (context.User.HasClaim(user => user.Type == ClaimTypes.Email))
        {
            context.Succeed(requirement);
        }
        else
        {
            var reason = new AuthorizationFailureReason(this, "You are already authorized");
            context.Fail(reason);
        }

        return Task.CompletedTask;
    }
}