using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Vendyp.Timesheet.WebApi.Endpoints.Sample.Requests;

namespace Vendyp.Timesheet.WebApi.Endpoints.Sample;

[Route("sample")]
public class Post : BaseEndpointWithoutResponse<PostRequest>
{
    [HttpPost("post")]
    [SwaggerOperation(
        Summary = "",
        Description = "",
        OperationId = "Sample.Post",
        Tags = new[] { "Sample" })
    ]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    public override Task<ActionResult> HandleAsync(
        [FromBody] PostRequest request,
        CancellationToken cancellationToken = new())
    {
        return Task.FromResult<ActionResult>(Ok(request.Name + "," + request.Id));
    }
}