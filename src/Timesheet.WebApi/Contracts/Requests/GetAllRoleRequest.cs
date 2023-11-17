using Microsoft.AspNetCore.Mvc;

namespace Timesheet.WebApi.Contracts.Requests;

public class GetAllRoleRequest
{
    [FromQuery(Name = "s")] public string? Search { get; set; }
}