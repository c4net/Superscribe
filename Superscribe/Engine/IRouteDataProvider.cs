namespace Superscribe.Engine
{
    using System;

    public interface IRouteDataProvider
    {
        IModuleRouteData GetData(string url, string method, Func<IModuleRouteData> factory);
    }
}