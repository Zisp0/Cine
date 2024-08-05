using System.Net;
using System.Text.Json;
using CineAPI.Common;
using FluentValidation;

namespace CineAPI.Middleware
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlerMiddleware> _logger;

        private static readonly JsonSerializerOptions JsonSerializerOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        public ErrorHandlerMiddleware(RequestDelegate next, ILogger<ErrorHandlerMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (CineException ex)
            {
                _logger.LogError(ex, "Un error se ha producido.");
                await HandleExceptionAsync(httpContext, ex, true);
            }
            catch (ValidationException ex)
            {
                await HandleExceptionAsync(httpContext, ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Un error se ha producido.");
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception, bool isMessageTrace = false)
        {
            var data = new Dictionary<string, string>();
            var statusCode = HttpStatusCode.BadRequest;
            var message = isMessageTrace
                ? exception.Message
                : "Un error ha ocurrido por favor contactese con el administrador.";

            switch (exception)
            {
                case CineException ex:
                    statusCode = ex.HttpStatusCode ?? HttpStatusCode.BadRequest;
                    break;
                case ValidationException ex:
                    message = string.Join(Environment.NewLine, ex.Errors
                        .Select(v => v.ErrorMessage));
                    data = ex.Errors.ToDictionary(x => x.PropertyName, x => x.ErrorMessage);
                    break;
            }

            var errorResponse = new
            {
                Message = message,
                ErrorDetail = exception.Message,
                Data = data
            };
            var payload = JsonSerializer.Serialize(errorResponse, JsonSerializerOptions);
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            return context.Response.WriteAsync(payload);
        }
    }
}