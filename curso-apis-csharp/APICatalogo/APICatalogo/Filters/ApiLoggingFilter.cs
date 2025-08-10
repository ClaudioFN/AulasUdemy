using Microsoft.AspNetCore.Mvc.Filters;

namespace APICatalogo.Filters;

public class ApiLoggingFilter : IActionFilter
{
    private readonly ILogger<ApiLoggingFilter> _logger;

    public ApiLoggingFilter(ILogger<ApiLoggingFilter> logger)
    {
        _logger = logger;
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        // Executa depois do metodo Action
        _logger.LogInformation("########## Executando -> OnActionExecuted");
        _logger.LogInformation("##################################################");
        _logger.LogInformation($"{DateTime.Now.ToLongTimeString()}");
        _logger.LogInformation($"ModelState: {context.HttpContext.Response.StatusCode}");
    }

    public void OnActionExecuting(ActionExecutingContext context)
    {
        // Executa antes do metodo Action
        _logger.LogInformation("########## Executando -> OnActionExecuting");
        _logger.LogInformation("##################################################");
        _logger.LogInformation($"{DateTime.Now.ToLongTimeString()}");
        _logger.LogInformation($"ModelState: {context.ModelState.IsValid}");
    }
}
