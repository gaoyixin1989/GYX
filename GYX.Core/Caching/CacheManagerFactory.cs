using GYX.Core.Caching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GYX.Core
{
    public class CacheManagerFactory
    {
        public static ICacheManager MemoryCache
        {
            get
            {
                return new MemoryCacheManager();
            }
        }
    }
}
