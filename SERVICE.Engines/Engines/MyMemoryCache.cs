using Microsoft.Extensions.Caching.Memory;
using SERVICE.Engine.Interfaces;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SERVICE.Engine.Engines
{
    public class MyMemoryCache : IMyCache
    {
        private readonly IMemoryCache _memoryCache;
        private readonly ConcurrentDictionary<object, ICacheEntry> _cacheEntries = new ConcurrentDictionary<object, ICacheEntry>();

        public MyMemoryCache(IMemoryCache memoryCache)
        {
            this._memoryCache = memoryCache;
        }

        public void Clear()
        {
            foreach (var cacheEntry in this._cacheEntries.Keys.ToList())
                this._memoryCache.Remove(cacheEntry);
        }

        public ICacheEntry CreateEntry(object key)
        {
            var entry = this._memoryCache.CreateEntry(key);
            entry.RegisterPostEvictionCallback(this.PostEvictionCallback);
            this._cacheEntries.AddOrUpdate(key, entry, (o, cacheEntry) =>
            {
                cacheEntry.Value = entry;
                return cacheEntry;
            });
            return entry;
        }

        public void Dispose()
        {
            this._memoryCache.Dispose();
        }

        public IEnumerator<KeyValuePair<object, object>> GetEnumerator()
        {
            return this._cacheEntries.Select(pair => new KeyValuePair<object, object>(pair.Key, pair.Value.Value)).GetEnumerator();

        }

        public void Remove(object key)
        {
            this._memoryCache.Remove(key);
        }

        public bool TryGetValue(object key, out object value)
        {
            return this._memoryCache.TryGetValue(key, out value);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        private void PostEvictionCallback(object key, object value, EvictionReason reason, object state)
        {
            if (reason != EvictionReason.Replaced)
                this._cacheEntries.TryRemove(key, out var _);
        }

        public IEnumerator<object> Keys => this._cacheEntries.Keys.GetEnumerator();
    }
}
