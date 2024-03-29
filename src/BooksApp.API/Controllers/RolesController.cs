using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PostsApp.Application.Roles.Commands.UpdateRole;
using PostsApp.Application.Roles.Queries.GetRoles;
using PostsApp.Common.Extensions;
using PostsApp.Contracts.Requests.Role;

namespace PostsApp.Controllers;

[Route("roles")]
public class RolesController : Controller
{
    private readonly ISender _sender;

    public RolesController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var command = new GetRoleQuery();

        var roles = await _sender.Send(command, cancellationToken);

        return Ok(roles);
    }
    
    [Authorize(Policy = "AdminOrModerator")]
    [HttpPut]
    public async Task<IActionResult> UpdateRole(ChangeRoleRequest request, CancellationToken cancellationToken)
    {
        var command = new UpdateRoleCommand
        {
            ChangerId = HttpContext.GetId()!, 
            Role = request.Role, 
            UserId = request.UserId
        };

        await _sender.Send(command, cancellationToken);
        
        return Ok("Operation succeeded");
    }
}