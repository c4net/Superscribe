﻿using Superscribe.Engine;

namespace Superscribe.Owin.Components
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using global::Owin;

    using Superscribe.Owin.Engine;

    public class OwinRouter
    {
        private readonly Func<IDictionary<string, object>, Task> next;

        private readonly IAppBuilder builder;

        private readonly IOwinRouteEngine engine;

        public OwinRouter(Func<IDictionary<string, object>, Task> next, IAppBuilder builder, IOwinRouteEngine engine)
        {
            this.next = next;
            this.builder = builder;
            this.engine = engine;
        }

        public async Task Invoke(IDictionary<string, object> environment)
        {
            var path = environment["owin.RequestPath"].ToString();           
            var method = environment["owin.RequestMethod"].ToString();            
           
            var routeData = new OwinRouteData { Environment = environment, Config = engine.Config };
            if (environment.ContainsKey("Microsoft.Owin.Query#dictionary"))
            {
                var queryParams = environment["Microsoft.Owin.Query#dictionary"] as IDictionary<string, string[]>;
                if (queryParams != null)
                {
                    foreach (var key in queryParams.Keys)
                    {
                        if (routeData.QueryParameters.ContainsKey(key))
                        {
                            continue;
                        }

                        var value = queryParams[key];
                        if (value.Length <= 1)
                        {
                            routeData.QueryParameters[key] = value.FirstOrDefault();
                            continue;
                        }

                        routeData.QueryParameters[key] = value;
                    }
                }
            }

            var walker = this.engine.Walker();            
            var data = walker.WalkRoute(path, method, routeData);

            environment["superscribe.RouteData"] = data;
            environment["route.Parameters"] = routeData.Parameters;
            environment[Constants.SuperscribeRouteWalkerEnvironmentKey] = walker;
            environment[Constants.SuperscribeRouteDataProviderEnvironmentKey] = new OwinRouteDataProvider(data);

            if (routeData.Pipeline.Any())
            {
                IAppBuilder branch = this.builder.New();
                foreach (var middleware in routeData.Pipeline)
                {
                    var func = middleware.Obj as Func<IAppBuilder, IAppBuilder>;
                    if (func != null)
                    {
                        branch = func(branch);
                    }
                    else
                    {
                        branch.Use(middleware.Obj, middleware.Args);    
                    }
                }

                branch.Use(typeof(RedirectMiddleware), this.next);

                var continuation = (Func<IDictionary<string, object>, Task>)branch.Build(typeof(Func<IDictionary<string, object>, Task>));
                await continuation(environment);
            }
            else
            {
                await this.next(environment);
            }
        }
    }
}
