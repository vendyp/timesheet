using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Vendyp.Timesheet.Shared.Abstractions.Models;
using Vendyp.Timesheet.WebApi.Common;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers(options =>
{
    options.Conventions.Add(
        new CustomRouteToken(
            "namespace",
            c => c.ControllerType.Namespace?.Split('.').Last()
        ));
    options.Conventions.Add(new RouteTokenTransformerConvention(new SlugifyParameterTransformer()));
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = false;
    options.InvalidModelStateResponseFactory = context =>
    {
        var env = context.HttpContext.RequestServices.GetRequiredService<IWebHostEnvironment>();
        if (env.IsProduction())
            return new BadRequestObjectResult(Error.Create("Invalid parameter"));

        return new BadRequestObjectResult(Error.Create("Invalid parameter",
            "To developer, please check your request, this error occur while construct model binding",
            400));
    };
});

var app = builder.Build();

app.UseRouting();
app.MapControllers();
app.Run();