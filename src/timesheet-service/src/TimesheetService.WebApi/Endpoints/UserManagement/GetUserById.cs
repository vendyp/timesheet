using TimesheetService.Core.Abstractions;
using TimesheetService.WebApi.Common;
using TimesheetService.WebApi.Contracts.Requests;
using TimesheetService.WebApi.Contracts.Responses;
using TimesheetService.WebApi.Endpoints.UserManagement.Scopes;
using TimesheetService.WebApi.Mapping;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace TimesheetService.WebApi.Endpoints.UserManagement;

public class GetUserById : BaseEndpoint<GetUserByIdRequest, UserResponse>
{
    private readonly IUserService _userService;

    public GetUserById(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet("users/{userId}")]
    [Authorize]
    [RequiredScope(typeof(UserManagementScope), typeof(UserManagementScopeReadOnly))]
    [SwaggerOperation(
        Summary = "Get user by id",
        Description = "",
        OperationId = "UserManagement.GetById",
        Tags = new[] { "UserManagement" })
    ]
    [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
    public override async Task<ActionResult<UserResponse>> HandleAsync([FromRoute] GetUserByIdRequest request,
        CancellationToken cancellationToken = new())
    {
        var user = await _userService.GetByIdAsync(request.UserId, cancellationToken);
        if (user is null)
            return BadRequest(Error.Create("Data not found"));

        return user.ToUserResponse();
    }
}