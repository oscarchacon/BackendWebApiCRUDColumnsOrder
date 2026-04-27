using System;
using System.Net;
using System.Threading.Tasks;
using BusinesRules.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace WebApi.Middleware;

/// <summary>
/// Middleware for handling exceptions in the HTTP request pipeline.
/// </summary>
public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="ExceptionHandlingMiddleware"/> class.
    /// </summary>
    /// <param name="next">The next middleware in the request pipeline.</param>
    /// <param name="logger">The logger for logging exceptions.</param>
    public ExceptionHandlingMiddleware(
        RequestDelegate next,
        ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    /// <summary>
    /// Invokes the middleware to handle exceptions in the HTTP pipeline.
    /// </summary>
    /// <param name="context">The HTTP context for the current request.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception exception)
        {
            _logger.LogError(
                exception, "Exception occurred: {Message}", exception.Message);

            var response = context.Response;
            response.ContentType = "application/json";

            response.StatusCode = exception switch
            {
                NotFoundException => (int)HttpStatusCode.NotFound,
                BadRequestException => (int)HttpStatusCode.BadRequest,
                NotAllowedException => (int)HttpStatusCode.MethodNotAllowed,
                _ => (int)HttpStatusCode.InternalServerError,
            };

            var responseError = new
            {
                estado = response.StatusCode,
                mensaje = exception?.Message,
                error = exception?.ToString()
            };
            await response.WriteAsync(JsonConvert.SerializeObject(responseError));
        }
    }
}
