using Microsoft.AspNetCore.Mvc;

namespace Timesheet.WebApi.Contracts.Requests;

public class DeleteCacheRequest
{
    [FromRoute(Name = "key")] public string Key { get; set; } = null!;
}