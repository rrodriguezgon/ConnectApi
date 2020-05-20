using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System.Net;

namespace OrderMailboxHub.Host.Filters
{
    public class ExceptionFilter
        : IExceptionFilter
    {
        private readonly ILogger<ExceptionFilter> _logger;

        public ExceptionFilter(ILogger<ExceptionFilter> logger)
        {
            _logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            var controllerName = context.RouteData.Values["controller"];

            var actionName = context.RouteData.Values["action"];

            _logger.LogError(context.Exception, controllerName.ToString(), actionName.ToString());

            context.ExceptionHandled = true;
            HttpResponse response = context.HttpContext.Response;
            response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.Result = new JsonResult(context.Exception);
        }
    }
}
