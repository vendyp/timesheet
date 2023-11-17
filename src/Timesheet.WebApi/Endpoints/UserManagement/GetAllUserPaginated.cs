using System.Linq.Dynamic.Core;
using Timesheet.Domain.Entities;
using Timesheet.Shared.Abstractions.Databases;
using Timesheet.Shared.Abstractions.Queries;
using Timesheet.WebApi.Common;
using Timesheet.WebApi.Contracts.Requests;
using Timesheet.WebApi.Contracts.Responses;
using Timesheet.WebApi.Endpoints.UserManagement.Scopes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;

namespace Timesheet.WebApi.Endpoints.UserManagement;

public class GetAllUserPaginated : BaseEndpoint<GetAllUserPaginatedRequest, PagedList<UserResponse>>
{
    private readonly IDbContext _dbContext;

    public GetAllUserPaginated(IDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet("users")]
    [Authorize]
    [RequiredScope(typeof(UserManagementScope), typeof(UserManagementScopeReadOnly))]
    [SwaggerOperation(
        Summary = "Get user paginated",
        Description = "",
        OperationId = "UserManagement.GetAll",
        Tags = new[] { "UserManagement" })
    ]
    [ProducesResponseType(typeof(PagedList<UserResponse>), StatusCodes.Status200OK)]
    public override async Task<ActionResult<PagedList<UserResponse>>> HandleAsync(
        [FromQuery] GetAllUserPaginatedRequest query,
        CancellationToken cancellationToken = new())
    {
        var queryable = _dbContext.Set<User>().AsQueryable();

        if (!string.IsNullOrWhiteSpace(query.Username))
            queryable = queryable.Where(e => EF.Functions.Like(e.Username, $"%{query.Username}%"));

        if (!string.IsNullOrWhiteSpace(query.Fullname))
            queryable = queryable.Where(e => EF.Functions.Like(e.FullName!, $"{query.Fullname}"));

        if (string.IsNullOrWhiteSpace(query.OrderBy))
            query.OrderBy = nameof(Domain.Entities.User.CreatedAt);

        if (string.IsNullOrWhiteSpace(query.OrderType))
            query.OrderType = "DESC";
        
        queryable = queryable.OrderBy($"{query.OrderBy} {query.OrderType}");

        var users = await queryable.Select(user => new UserResponse
            {
                UserId = user.UserId,
                Username = user.Username,
                LastPasswordChangeAt = user.LastPasswordChangeAt,
                FullName = user.FullName,
                Email = user.Email,
                CreatedAt = user.CreatedAt,
                CreatedByName = user.CreatedByName,
                LastUpdatedAt = user.LastUpdatedAt,
                LastUpdatedByName = user.LastUpdatedByName
            })
            .Skip(query.CalculateSkip())
            .Take(query.Size)
            .ToListAsync(cancellationToken);

        var response = new PagedList<UserResponse>(users, query.Page, query.Size);

        return response;
    }
}