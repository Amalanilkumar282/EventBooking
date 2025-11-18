using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace EventBooking.Api.Middleware
{
    /// <summary>
    /// Global exception middleware that maps FluentValidation.ValidationException to HTTP 400 with a structured JSON body.
    /// It also handles generic exceptions and returns a 500 with a minimal message.
    /// </summary>
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (ValidationException vex)
            {
                _logger.LogWarning(vex, "Request validation failed");
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                context.Response.ContentType = "application/json";

                var failures = vex.Errors.Where(e => e != null);
                var errors = failures
                    .GroupBy(e => string.IsNullOrWhiteSpace(e.PropertyName) ? "" : e.PropertyName)
                    .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray());

                var response = new
                {
                    type = "https://example.com/validation-error",
                    title = "One or more validation errors occurred.",
                    status = 400,
                    errors
                };

                var json = JsonSerializer.Serialize(response);
                await context.Response.WriteAsync(json);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception");
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                context.Response.ContentType = "application/json";

                var response = new
                {
                    type = "https://example.com/internal-server-error",
                    title = "An unexpected error occurred.",
                    status = 500
                };

                var json = JsonSerializer.Serialize(response);
                await context.Response.WriteAsync(json);
            }
        }
    }
}
