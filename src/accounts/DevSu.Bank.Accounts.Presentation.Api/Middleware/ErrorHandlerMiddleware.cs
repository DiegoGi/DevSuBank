using DevSu.Bank.Accounts.Application.SeedWork;
using DevSu.Bank.Accounts.Common.Constants;
using DevSu.Bank.Accounts.Domain.Resources;
using DevSu.Bank.Accounts.Domain.SeedWork;
using System.Net;
using System.Text.Json;

namespace DevSu.Bank.Accounts.Presentation.Api.Middleware
{
    public class ErrorHandlerMiddleware(RequestDelegate next, ILogger<ErrorHandlerMiddleware> logger)
    {
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await next(context).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                await HandlerExceptionAsync(context, ex);
            }
        }

        private async Task HandlerExceptionAsync(HttpContext context, Exception ex)
        {
            ErrorResponse errorResponse;
            int httpStatusCode;
            var traceId = context.TraceIdentifier;

            switch (ex)
            {
                case DomainValidationException exception:
                    httpStatusCode = (int)HttpStatusCode.BadRequest;
                    errorResponse = new(exception.Message, httpStatusCode, traceId, exception.Details);
                    break;
                case ApplicationValidationException exception:
                    httpStatusCode = (int)HttpStatusCode.BadRequest;
                    errorResponse = new(exception.Message, httpStatusCode, traceId, exception.Details);
                    break;
                case NotFoundException exception:
                    httpStatusCode = (int)HttpStatusCode.NotFound;
                    errorResponse = new(exception.Message, httpStatusCode, traceId, exception.Details);
                    break;
                default:
                    httpStatusCode = (int)HttpStatusCode.InternalServerError;
                    errorResponse = new(Generals.UnkownError, httpStatusCode, traceId, []);
                    break;
            }

            context.Response.StatusCode = httpStatusCode;
            context.Response.ContentType = MimeTypes.Json;
            var result = JsonSerializer.Serialize(errorResponse);
            logger.LogError(ex, result);
            await context.Response.WriteAsync(result);
        }
    }
}
