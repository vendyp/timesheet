using Microsoft.AspNetCore.Mvc;

namespace TimesheetService.WebApi.Contracts.Requests;

public class GetCacheRequest
{
    [FromRoute(Name = "key")] public string Key { get; set; } = null!;
}