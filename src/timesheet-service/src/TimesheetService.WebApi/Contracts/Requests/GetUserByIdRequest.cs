using Microsoft.AspNetCore.Mvc;

namespace TimesheetService.WebApi.Contracts.Requests;

public class GetUserByIdRequest
{
    [FromRoute(Name = "userId")] public Guid UserId { get; set; }
}