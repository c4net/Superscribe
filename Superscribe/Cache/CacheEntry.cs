using Superscribe.Engine;

namespace Superscribe.Cache
{
    using System;

    public class CacheEntry<T>
    {
        public T Info { get; set; }

        private Func<IModuleRouteData, object> _onComplete;

        public Func<IModuleRouteData, object> OnComplete
        {
            get { return _onComplete; }
            set { _onComplete = value; }
        }

    }
}
