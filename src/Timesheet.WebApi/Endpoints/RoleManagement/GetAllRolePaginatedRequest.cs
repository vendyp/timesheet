using Timesheet.Shared.Abstractions.Queries;
using Microsoft.AspNetCore.Mvc;

namespace Timesheet.WebApi.Endpoints.RoleManagement;

public class GetAllRolePaginatedRequest : BasePaginationCalculation
{
    [FromQuery(Name = "s")] public string? Search { get; set; }
}