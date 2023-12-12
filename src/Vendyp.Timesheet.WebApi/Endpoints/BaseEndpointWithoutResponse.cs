﻿using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Mvc;

// ReSharper disable RouteTemplates.RouteTokenNotResolved

namespace Vendyp.Timesheet.WebApi.Endpoints;

[Route("api/[namespace]")]
public abstract class BaseEndpointWithoutResponse<TReq> : EndpointBaseAsync.WithRequest<TReq>.WithActionResult
{
}