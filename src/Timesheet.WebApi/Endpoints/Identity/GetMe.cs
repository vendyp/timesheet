﻿using Timesheet.Core.Abstractions;
using Timesheet.Domain.Extensions;
using Timesheet.Shared.Abstractions.Contexts;
using Timesheet.WebApi.Contracts.Responses;
using Timesheet.WebApi.Mapping;
using Timesheet.WebApi.Scopes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Timesheet.WebApi.Endpoints.Identity;

public class GetMe : BaseEndpoint<UserResponse>
{
    private readonly IContext _context;
    private readonly IUserService _userService;

    public GetMe(IContext context, IUserService userService)
    {
        _context = context;
        _userService = userService;
    }

    [HttpGet("me")]
    [SwaggerOperation(
        Summary = "Get me",
        Description = "Get profile by user id of its user",
        OperationId = "Identity.GetMe",
        Tags = new[] { "Identity" })
    ]
    [Authorize]
    [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
    public override async Task<ActionResult<UserResponse>> HandleAsync(
        CancellationToken cancellationToken = new())
    {
        var user = await _userService.GetByIdAsync(_context.Identity.Id, cancellationToken);
        if (user is null)
            return BadRequest(Error.Create("Data not found"));

        var dto = user.ToUserResponse();

        if (!user.UserRoles.Any(e => e.RoleId == RoleExtensions.SuperAdministratorId))
            return Ok(dto);

        dto.Scopes.Clear();
        dto.Scopes.AddRange(ScopeManager.Instance.GetAllScopes());

        return Ok(dto);
    }
}