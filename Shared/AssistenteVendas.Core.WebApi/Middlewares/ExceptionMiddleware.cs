using AssistenteVendas.Core.Exceptions;
using AssistenteVendas.Core.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Net;

namespace AssistenteVencas.Core.WebApi.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _log;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> log)
        {
            _next = next;
            _log = log;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }

            catch (ForbiddenException ex)
            {
                await HandleExceptionAsync(httpContext, ex);
            }
            catch (UnauthorizedException ex)
            {
                await HandleExceptionAsync(httpContext, ex);
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "{Message}, {StackTrace}", ex.Message, ex.StackTrace);
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            var msg = "Não foi possível completar a solicitação.";

            if (exception is ForbiddenException || exception.InnerException is ForbiddenException)
            {
                context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                msg = exception.InnerException != null ? exception.InnerException?.Message : exception.Message;
            }
            else if (exception is UnauthorizedException || exception.InnerException is UnauthorizedException)
            {
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                return Task.CompletedTask;
            }

            var response = new RetornoPadrao<object>
            {
                Success = false,
                Errors = new List<string>
                {
                    msg
                }
            };
            var json = Serializador.Serializar(response);
            context.Response.ContentType = "application/json";

            return context.Response.WriteAsync(json);

        }
    }
}
