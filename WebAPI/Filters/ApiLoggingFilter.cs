using Microsoft.AspNetCore.Mvc.Filters;

namespace WebAPI.Filters
{
    public class ApiLoggingFilter : IActionFilter
    {
        private readonly ILogger<ApiLoggingFilter> _logger;

        public ApiLoggingFilter(ILogger<ApiLoggingFilter> logger)
        {
            _logger = logger;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            // execute before the action method executes
            _logger.LogInformation("Executing: OnActionExecuting");
            _logger.LogInformation("Action: {ActionName}", context.ActionDescriptor.DisplayName);
            _logger.LogInformation("Controller: {ControllerName}", context.ActionDescriptor.RouteValues["controller"]);
            _logger.LogInformation("Method: {HttpMethod}", context.HttpContext.Request.Method);
            _logger.LogInformation("Request Path: {RequestPath}", context.HttpContext.Request.Path);
            _logger.LogInformation("Request Query: {QueryString}", context.HttpContext.Request.QueryString);
            _logger.LogInformation("Request Headers: {Headers}", context.HttpContext.Request.Headers.ToString());
            _logger.LogInformation("Request Body: {Body}", context.HttpContext.Request.Body.ToString()); // Note: Body may not be readable here, consider using a middleware for body logging
            _logger.LogInformation("Timestamp: {Timestamp}", DateTime.UtcNow);           
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            // execute after the action method executes
            _logger.LogInformation("Executing: OnActionExecuted");
            if (context.Exception != null)
            {
                _logger.LogError(context.Exception, "An error occurred while executing the action.");
            }
            else
            {
                _logger.LogInformation("Timestamp: {Timestamp}", DateTime.UtcNow);
                _logger.LogInformation("Response Status Code: {StatusCode}", context.HttpContext.Response.StatusCode);
            }
        }

    }
}
