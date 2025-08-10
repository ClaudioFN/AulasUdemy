using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace APICatalogo.Filters
{
    public class ApiExceptionFiltercs : IExceptionFilter
    {
        private readonly ILogger<ApiExceptionFiltercs> _logger;

        public ApiExceptionFiltercs(ILogger<ApiExceptionFiltercs> logger)
        {
            _logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            _logger.LogError(context.Exception, "Ocorreu uma exceção não tratada: Status Code 500");

            context.Result = new ObjectResult("Erro ao tratar sua solicitação: Status Code 500")
            {
                StatusCode = StatusCodes.Status500InternalServerError,
            };

        }
    }
}
