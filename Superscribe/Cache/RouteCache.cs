using Finsa.CodeServices.Common.Collections.Concurrent;

namespace Superscribe.Cache
{

    public interface IRouteCache
    {
        bool TryGet<T>(string url, out CacheEntry<T> entry);

        void Store<T>(string url, CacheEntry<T> handler);
    }

    public class RouteCache : IRouteCache
    {
        public ConcurrentDictionary<string, object> Cache = new ConcurrentDictionary<string, object>();
        
        public bool TryGet<T>(string url, out CacheEntry<T> entry)
        {
            object result;

            if (Cache.TryGetValue(url, out result))
            {
                entry = (CacheEntry<T>)result;
                return true;
            }

            entry = null;
            return false;
        }
        
        public void Store<T>(string url, CacheEntry<T> handler)
        {
            object foundObject;
            this.Cache.TryAdd(url, handler, out foundObject);
        }
    }
}
