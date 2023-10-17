using TimesheetService.Shared.Abstractions.Queries;
using Microsoft.AspNetCore.Mvc;

namespace TimesheetService.WebApi.Endpoints.RoleManagement;

public class GetAllRolePaginatedRequest : BasePaginationCalculation
{
    [FromQuery(Name = "s")] public string? Search { get; set; }
}