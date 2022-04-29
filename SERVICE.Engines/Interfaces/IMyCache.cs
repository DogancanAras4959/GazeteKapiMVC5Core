using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Text;

namespace SERVICE.Engine.Interfaces
{
    public interface IMyCache : IEnumerable<KeyValuePair<object, object>>, IMemoryCache
    {
        void Clear();
    }
}
