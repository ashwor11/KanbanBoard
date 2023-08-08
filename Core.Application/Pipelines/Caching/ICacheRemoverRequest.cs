using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Pipelines.Caching
{
    public interface ICacheRemoverRequest
    {
        public string CacheKey { get; set; }
        public bool BypassCache { get; set; }
    }
}
