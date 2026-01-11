using System.Net;
using FabrilOS.API.DTOs;
using Microsoft.AspNetCore.Diagnostics;

namespace FabrilOS.API.Middleware;

public class GlobalExceptionHandler : IExceptionHandler
{
  private readonly ILogger<GlobalExceptionHandler> _logger;
  private readonly IHostEnvironment _env;

  public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger, IHostEnvironment env)
  {
    _logger = logger;
    _env = env;
  }

  public async ValueTask<bool> TryHandleAsync(
      HttpContext httpContext,
      Exception exception,
      CancellationToken cancellationToken)
  {
    _logger.LogError(
        exception,
        "Erro inesperado: {Message}",
        exception.Message);

    httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
    httpContext.Response.ContentType = "application/json";

    var response = new ErrorResponseDto
    {
      StatusCode = httpContext.Response.StatusCode,
      Message = "Ocorreu um erro interno no servidor.",
      Details = _env.IsDevelopment() ? exception.Message : null
    };

    await httpContext.Response.WriteAsJsonAsync(response, cancellationToken);

    return true;
  }
}