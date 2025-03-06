using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace ControleLancamentos.Application.PipelineBehavior
{
    public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;

        public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
        {
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            // Registra o início da execução da request
            _logger.LogInformation("Iniciando a execução da request {RequestName} com dados: {@RequestData}",
                typeof(TRequest).Name, request);

            var stopwatch = Stopwatch.StartNew();

            try
            {
                // Chama o próximo behavior ou o handler final
                var response = await next();

                // Registra o sucesso da execução
                stopwatch.Stop();
                _logger.LogInformation("Request {RequestName} executada com sucesso em {ElapsedMilliseconds} ms. Resposta: {@Response}",
                    typeof(TRequest).Name, stopwatch.ElapsedMilliseconds, response);

                return response;
            }
            catch (Exception ex)
            {
                // Registra erros durante a execução
                stopwatch.Stop();
                _logger.LogError(ex, "Erro ao executar a request {RequestName} após {ElapsedMilliseconds} ms. Mensagem: {ErrorMessage}",
                    typeof(TRequest).Name, stopwatch.ElapsedMilliseconds, ex.Message);

                throw; // Re-lança a exceção para que o pipeline continue a tratá-la
            }
        }
    }

}
