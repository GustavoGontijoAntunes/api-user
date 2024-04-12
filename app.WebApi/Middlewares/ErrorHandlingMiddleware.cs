using app.Domain.Exceptions;
using app.Domain.Resources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;
using System.Net;

namespace app.WebApi.Middlewares
{
    public class ErrorHandlingMiddleware
    {
        private readonly ILogger<ErrorHandlingMiddleware> _logger;
        private readonly IStringLocalizer<CustomMessages> _localizer;
        private readonly RequestDelegate _next;

        public ErrorHandlingMiddleware(RequestDelegate next,
            ILogger<ErrorHandlingMiddleware> logger,
            IStringLocalizer<CustomMessages> localizer)
        {
            _next = next;
            _logger = logger;
            _localizer = localizer;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }

            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            if (exception is DomainException)
            {
                _logger.LogWarning(exception, $"Handle {nameof(DomainException)} of HttpStatus 400");

                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                if ((exception as DomainException).ValidationErrors?.Any() ?? false)
                {
                    var errorMessages = (exception as DomainException)
                        .ValidationErrors
                        .ToArray();

                    var messages = new Dictionary<string, string[]>
                    {{ "Error", errorMessages}};

                    return FormatDetailedResponse(context, messages, nameof(DomainException));
                }

                return FormatErrorResponse(context, _localizer[exception.Message].Value);
            }

            else if (exception is ReadExcelException)
            {
                _logger.LogWarning(exception, $"Handle {nameof(ReadExcelException)} of HttpStatus 400");

                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                var message = _localizer["ReadExcelError"].Value;

                var errorMessages = (exception as ReadExcelException).ReadErrors
                    .Select(s => string.Format(message, s.Item1, s.Item2))
                    .ToArray();

                var messages = new Dictionary<string, string[]>
                {{  "Error", errorMessages }};

                return FormatDetailedResponse(context, messages, nameof(ReadExcelException));
            }

            else
            {
                _logger.LogError(exception, "HandleException of HttpStatus 500");
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            }

            return FormatErrorResponse(context, exception.Message);
        }

        private Task FormatDetailedResponse(
            HttpContext context, Dictionary<string, string[]> messages, string exceptionName)
        {
            var badRequest = new ValidationProblemDetails(messages)
            {
                Status = (int)HttpStatusCode.BadRequest,
                Type = exceptionName
            };

            var response = JsonConvert.SerializeObject(badRequest);
            return context.Response.WriteAsync(response);
        }

        private Task FormatErrorResponse(HttpContext context, string message)
        {
            var response = JsonConvert.SerializeObject(new { message });

            return context.Response.WriteAsync(response);
        }
    }
}