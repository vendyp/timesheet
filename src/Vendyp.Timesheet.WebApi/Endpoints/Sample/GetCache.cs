using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Vendyp.Timesheet.Shared.Abstractions.Cache;
using Vendyp.Timesheet.WebApi.Endpoints.Sample.Requests;

namespace Vendyp.Timesheet.WebApi.Endpoints.Sample;

public class GetCache : BaseEndpoint<GetCacheRequest, string?>
{
    private readonly ICache _cache;

    public GetCache(ICache cache)
    {
        _cache = cache;
    }

    [HttpGet("cache/{key}")]
    [SwaggerOperation(
        Summary = "Get data from cache by key",
        Description = "",
        OperationId = "Sample.GetCache",
        Tags = new[] { "Sample" })
    ]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    public override async Task<ActionResult<string?>> HandleAsync([FromRoute] GetCacheRequest request,
        CancellationToken cancellationToken = new())
    {
        var data = await _cache.GetAsync<string>(request.Key);

        if (string.IsNullOrWhiteSpace(data))
            return NotFound();

        return data;
    }
}