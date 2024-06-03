﻿using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PostsApp.Application.Images.Commands.CreateImage;
using PostsApp.Application.Images.Commands.DeleteImage;
using PostsApp.Application.Users.Commands.DeleteUser;
using PostsApp.Application.Users.Commands.InsertAvatar;
using PostsApp.Application.Users.Commands.UpdateUser;
using PostsApp.Application.Users.Commands.UpdateUsername;
using PostsApp.Application.Users.Queries.GetSingleUser;
using PostsApp.Application.Users.Queries.GetUsers;
using PostsApp.Common.Constants;
using PostsApp.Common.Contracts.Requests.User;
using PostsApp.Common.Contracts.Responses.User;
using PostsApp.Common.Extensions;
using PostsApp.Domain.Exceptions;
using Toycloud.AspNetCore.Mvc.ModelBinding;

namespace PostsApp.Controllers;

[Route("user")]
public class UserController : Controller
{
    private readonly ISender _sender;

    public UserController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet]
    [Authorize(Policy = Policies.Authorized)]
    public IActionResult GetUser()
    {
        var username = HttpContext.GetUsername()!;
        var role = HttpContext.GetRole()!;
        var id = HttpContext.GetId();
        return Ok(new UserResponse { Id = id, Username = username, Role = role });
    }
    
    [HttpGet("many")]
    public async Task<IActionResult> GetManyUsers(int? page, int? limit, string? q, CancellationToken cancellationToken)
    {
        var query = new GetUsersQuery { Query = q, Page = page, Limit = limit };
        var users = await _sender.Send(query, cancellationToken);
        return Ok(users);
    }

    [HttpGet("single/{id:int}")]
    public async Task<IActionResult> GetUserById(int id, CancellationToken cancellationToken)
    {
        try
        {
            var query = new GetSingleUserQuery { Id = id };
            var user = await _sender.Send(query, cancellationToken);
            return Ok(user);
        }
        catch (UserException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete]
    [Authorize(Policy = Policies.Authorized)]
    public async Task<IActionResult> DeleteUser(CancellationToken cancellationToken)
    {
        try
        {
            var command = new DeleteUserCommand { Id = HttpContext.GetId() };
            await _sender.Send(command, cancellationToken);
        }
        catch (UserException ex)
        {
            return BadRequest(ex.Message);
        }

        await HttpContext.SignOutAsync();

        return Ok("User was deleted");
    }

    [HttpPut("username")]
    [Authorize(Policy = Policies.Authorized)]
    public async Task<IActionResult> UpdateUsername([FromBodyOrDefault] UpdateUsername request,
        CancellationToken cancellationToken)
    {
        var command = new UpdateUserCommand
        {
            Id = HttpContext.GetId(),
            NewUsername = request.NewUsername,
        };

        await _sender.Send(command, cancellationToken);

        HttpContext.ChangeUsername(request.NewUsername);

        return Ok("Username was updated");
    }
    
    [HttpPut("avatar")]
    [Authorize(Policy = Policies.Authorized)]
    public async Task<IActionResult> UpdateAvatar([FromBodyOrDefault] InsertAvatarRequest request,
        CancellationToken cancellationToken)
    {
        var createImageCommand = new CreateImageCommand
        {
            Image = request.Image
        };
        
        var imageName = await _sender.Send(createImageCommand, cancellationToken);
        
        var userQuery = new GetSingleUserQuery
        {
            Id = HttpContext.GetId()
        };
        
        var user = await _sender.Send(userQuery, cancellationToken);

        if (user.AvatarName is not null)
        {
            var deleteImageCommand = new DeleteImageCommand
            {
                ImageName = user.AvatarName
            };
            await _sender.Send(deleteImageCommand, cancellationToken);
        }
        
        var command = new InsertAvatarCommand
        {
            Id = HttpContext.GetId(),
            ImageName = imageName
        };

        var result = await _sender.Send(command, cancellationToken);

        return Ok(result);
    }
}