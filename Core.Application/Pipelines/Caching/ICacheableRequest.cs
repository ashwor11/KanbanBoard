using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Pipelines.Caching
{
    public interface ICacheableRequest
    {
        public bool BypassCache { get; set; }
        public string CacheKey { get; set; }
        public int SlidingExpiration { get; set; }
    }
}
