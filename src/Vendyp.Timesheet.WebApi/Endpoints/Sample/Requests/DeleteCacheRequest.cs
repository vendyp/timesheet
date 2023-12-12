using Microsoft.AspNetCore.Mvc;

namespace Vendyp.Timesheet.WebApi.Endpoints.Sample.Requests;

public class DeleteCacheRequest
{
    [FromRoute(Name = "key")] public string Key { get; set; } = null!;
}