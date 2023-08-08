using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Pipelines.Caching
{
    public class CachingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>, ICacheableRequest
    {
        private readonly IDistributedCache _cache;
        private readonly ILogger<CachingBehavior<TRequest, TResponse>> _logger;
        private readonly CacheSettings _cacheSettings;

        public CachingBehavior(IDistributedCache cache, ILogger<CachingBehavior<TRequest, TResponse>> logger, CacheSettings cacheSettings)
        {
            _cache = cache;
            _logger = logger;
            _cacheSettings = cacheSettings;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            TResponse response;
            if (request.BypassCache) return await next();

            byte[] cachedResponse = await _cache.GetAsync(request.CacheKey);

            if (cachedResponse == null)
            {
                response = await next();

                TimeSpan timeSpan = TimeSpan.FromDays(_cacheSettings.SlidingExpiration);
                DistributedCacheEntryOptions options = new() { SlidingExpiration = timeSpan };

                byte[] data = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(response));
                await _cache.SetAsync(request.CacheKey, data, options, cancellationToken);
                _logger.LogInformation($"Added to Cache. Cache Key: {request.CacheKey}");
            }
            else
            {
                response = JsonConvert.DeserializeObject<TResponse>(Encoding.UTF8.GetString(cachedResponse));
                _logger.LogInformation($"Fetched from Cache Key: {request.CacheKey}");
            }
            return response;

        }
    }
}
