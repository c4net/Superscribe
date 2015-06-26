namespace Superscribe.Owin.Engine
{
    using System;

    using Superscribe.Engine;

    public class OwinRouteDataProvider : IRouteDataProvider
    {
        private readonly IModuleRouteData data;

        public OwinRouteDataProvider(IModuleRouteData data)
        {
            this.data = data;
        }

        public IModuleRouteData GetData(string uri, string method, Func<IModuleRouteData> factory)
        {
            return this.data;
        }
    }
}
