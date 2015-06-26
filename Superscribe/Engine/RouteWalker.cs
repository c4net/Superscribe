namespace Superscribe.Engine
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Superscribe.Cache;
    using Superscribe.Models;

    public class RouteWalker : IRouteWalker
    {
        private readonly GraphNode baseNode;

        private readonly IRouteCache routeCache;

        public RouteWalker(GraphNode baseNode, IRouteCache routeCache)
        {
            this.baseNode = baseNode;
            this.routeCache = routeCache;
        }

        public bool ParamConversionError { get; private set; }

        public string Route { get; set; }

        public string Method { get; set; }

        public Queue<string> RemainingSegments { get; private set; }

        public IModuleRouteData WalkRoute(string route, string method, IModuleRouteData info)
        {
            this.Method = info.Method = method;
            this.Route = info.Url = route;

            CacheEntry<IModuleRouteData> cacheEntry;
            if (routeCache.TryGet(method + "-" + route, out cacheEntry))
            {
                info.Parameters = cacheEntry.Info.Parameters;
                info.Response = cacheEntry.OnComplete(info);

                info.FinalFunctionExecuted = true;

                return info;
            }

            this.RemainingSegments = new Queue<string>(route.Split('/'));
            this.WalkRoute(info, this.baseNode);

            return info;
        }

        public string PeekNextSegment()
        {
            if (this.RemainingSegments.Any())
            {
                return this.RemainingSegments.Peek();
            }

            return string.Empty;
        }

        public void WalkRoute(IModuleRouteData info, GraphNode match)
        {
            FinalFunction onComplete = null;
            while (match != null)
            {
                foreach (var action in match.ActionFunctions.Values)
                {
                    action(info, this.PeekNextSegment());
                }

                if (this.RemainingSegments.Any())
                {
                    this.RemainingSegments.Dequeue();
                }
                
                if (onComplete != null)
                {
                    if (onComplete.IsExclusive)
                    {
                        onComplete = null;
                    }
                }

                if (match.FinalFunctions.Count > 0)
                {
                    var function = match.FinalFunctions.FirstOrDefault(o => o.Method == this.Method)
                                   ?? match.FinalFunctions.FirstOrDefault(o => string.IsNullOrEmpty(o.Method));

                    if (function != null)
                    {
                        onComplete = function;
                    }
                }

                var nextMatch = this.FindNextMatch(info, this.PeekNextSegment(), match.Edges);
                if (nextMatch == null)
                {
                    if (this.HasFinalsButNoneMatchTheCurrentMethod(match))
                    {
                        info.NoMatchingFinalFunction = true;
                        return;
                    }
                }

                match = nextMatch;
            }

            if (this.RemainingSegments.Any(o => !string.IsNullOrEmpty(o)))
            {
                info.ExtraneousMatch = true;
                return;
            }

            if (onComplete != null)
            {
                routeCache.Store(this.Method + "-" + this.Route, new CacheEntry<IModuleRouteData> { Info = info, OnComplete = onComplete.Function });
                info.Response = onComplete.Function(info);
                info.FinalFunctionExecuted = true;
            }
        }

        private GraphNode FindNextMatch(IModuleRouteData info, string segment, IEnumerable<GraphNode> states)
        {
            return !string.IsNullOrEmpty(segment) ?
                states.FirstOrDefault(o => o.ActivationFunction(info, segment))
                : null;
        }

        private bool HasFinalsButNoneMatchTheCurrentMethod(GraphNode match)
        {
            return match.FinalFunctions.Count > 0
                   && !match.FinalFunctions.Any(o => string.IsNullOrEmpty(o.Method) || o.Method == this.Method);
        }
    }
}
