namespace Superscribe.Engine
{
    using System;

    using Superscribe.Models;

    public interface IRouteEngine
    {
        GraphNode Base { get; }
        
        IRouteWalker Walker();

        GraphNode Route(string routeTemplate);

        GraphNode Route(string routeTemplate, Func<object, object> func);

        GraphNode Route(GraphNode node);

        GraphNode Route(GraphNode config, Func<object, object> func);

        GraphNode Route(Func<RouteGlue, GraphNode> config);

        GraphNode Route(Func<RouteGlue, GraphNode> config, Func<object, object> func);
        
        GraphNode Get(string routeTemplate, Func<object, object> func);

        GraphNode Get(GraphNode config, Func<object, object> func);
        
        GraphNode Get(Func<RouteGlue, GraphNode> config, Func<object, object> func);
        
        GraphNode Post(string routeTemplate, Func<object, object> func);
        
        GraphNode Post(GraphNode config, Func<object, object> func);
        
        GraphNode Post(Func<RouteGlue, GraphNode> config, Func<object, object> func);
        
        GraphNode Put(string routeTemplate, Func<object, object> func);
        
        GraphNode Put(GraphNode config, Func<object, object> func);
        
        GraphNode Put(Func<RouteGlue, GraphNode> config, Func<object, object> func);
        
        GraphNode Patch(string routeTemplate, Func<object, object> func);
        
        GraphNode Patch(GraphNode config, Func<object, object> func);
        
        GraphNode Patch(Func<RouteGlue, GraphNode> config, Func<object, object> func);
        
        GraphNode Delete(string routeTemplate, Func<object, object> func);
        
        GraphNode Delete(GraphNode config, Func<object, object> func);
        
        GraphNode Delete(Func<RouteGlue, GraphNode> config, Func<object, object> func);
        
    }
}
