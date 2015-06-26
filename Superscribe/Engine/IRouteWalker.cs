namespace Superscribe.Engine
{
    using System.Collections.Generic;

    public interface IRouteWalker
    {
        bool ParamConversionError { get; }

        string Route { get; set; }

        string Method { get; set; }

        Queue<string> RemainingSegments { get; }
        
        IModuleRouteData WalkRoute(string route, string method, IModuleRouteData info);
    }
}