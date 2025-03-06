using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace ConsolidadoDiario.Application.PipelineBehavior
{
    public interface ICacheable
    {
        string CacheKey { get; } 
        int CacheDurationMinutes { get; } 
    }

    public class CachingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly IDistributedCache _cache;
        private readonly ILogger<CachingBehavior<TRequest, TResponse>> _logger;

        public CachingBehavior(IDistributedCache cache, ILogger<CachingBehavior<TRequest, TResponse>> logger)
        {
            _cache = cache;
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            // Verifica se a request é uma consulta (query) que deve ser armazenada em cache
            if (request is ICacheable cacheableRequest)
            {
                var cacheKey = cacheableRequest.CacheKey;

                // Tenta obter do cache
                var cachedResult = await _cache.GetStringAsync(cacheKey, cancellationToken);
                if (cachedResult != null)
                {
                    _logger.LogInformation("Retornando resultado do cache para a chave {CacheKey}", cacheKey);
                    return JsonSerializer.Deserialize<TResponse>(cachedResult)!;
                }

                // Executa o handler se o cache não estiver disponível
                var response = await next();

                // Armazena o resultado no cache
                await _cache.SetStringAsync(cacheKey, JsonSerializer.Serialize(response), new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(cacheableRequest.CacheDurationMinutes)
                }, cancellationToken);

                _logger.LogInformation("Resultado armazenado no cache para a chave {CacheKey}", cacheKey);

                return response;
            }

            // Se a request não for cacheável, apenas executa o handler
            return await next();
        }
    }
}
