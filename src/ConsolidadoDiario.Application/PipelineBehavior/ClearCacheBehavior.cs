using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace ConsolidadoDiario.Application.PipelineBehavior
{
    public interface IClearCache
    {
        string CacheKey { get; } 
    }
    public class ClearCacheBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly IDistributedCache _cache;
        private readonly ILogger<ClearCacheBehavior<TRequest, TResponse>> _logger;

        public ClearCacheBehavior(IDistributedCache cache, ILogger<ClearCacheBehavior<TRequest, TResponse>> logger)
        {
            _cache = cache;
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            // Executa o handler primeiro
            var response = await next();

            // Verifica se a request deve limpar o cache
            if (request is IClearCache clearCacheRequest)
            {
                var cacheKey = clearCacheRequest.CacheKey;
                await _cache.RemoveAsync(cacheKey, cancellationToken);                
            }

            return response;
        }
    }
}
