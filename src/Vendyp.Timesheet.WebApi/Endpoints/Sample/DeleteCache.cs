using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Vendyp.Timesheet.Shared.Abstractions.Cache;
using Vendyp.Timesheet.WebApi.Endpoints.Sample.Requests;

namespace Vendyp.Timesheet.WebApi.Endpoints.Sample;

public class DeleteCache : BaseEndpointWithoutResponse<DeleteCacheRequest>
{
    private readonly ICache _cache;

    public DeleteCache(ICache cache)
    {
        _cache = cache;
    }

    [HttpDelete("cache/{key}")]
    [SwaggerOperation(
        Summary = "Remove data from cache by key",
        Description = "",
        OperationId = "Sample.DeleteCache",
        Tags = new[] { "Sample" })
    ]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    public override async Task<ActionResult> HandleAsync([FromRoute] DeleteCacheRequest request,
        CancellationToken cancellationToken = new CancellationToken())
    {
        await _cache.DeleteAsync(request.Key);
        return Ok();
    }
}