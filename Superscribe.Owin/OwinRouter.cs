﻿namespace Superscribe.Owin
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using global::Owin;

    using Superscribe.Owin.Extensions;
    using Superscribe.Utils;

    public class OwinRouter
    {
        private readonly Func<IDictionary<string, object>, Task> next;

        private readonly IAppBuilder builder;

        private readonly SuperscribeOwinConfig config;

        public OwinRouter(Func<IDictionary<string, object>, Task> next, IAppBuilder builder, SuperscribeOwinConfig config)
        {
            this.next = next;
            this.builder = builder;
            this.config = config;
        }

        public async Task Invoke(IDictionary<string, object> environment)
        {
            var path = environment["owin.RequestPath"].ToString();
            var method = environment["owin.RequestMethod"].ToString();

            var routeData = new OwinRouteData { Environment = environment, Config = config };
            environment["superscribe.RouteData"] = routeData;

            var walker = new RouteWalker<OwinRouteData>(ʃ.Base);
            walker.WalkRoute(path, method, routeData);
            
            if (walker.IncompleteMatch)
            {
                environment.SetResponseStatusCode(404);
                environment.WriteResponse("404 - Route was incomplete").RunSynchronously();
                return;
            }

            if (walker.ExtraneousMatch)
            {
                environment.SetResponseStatusCode(404);
                environment.WriteResponse("404 - Route match failed").RunSynchronously(); ;
                return;
            }

            if (routeData.Pipeline.Any())
            {
                IAppBuilder branch = builder.New();
                foreach (var middleware in routeData.Pipeline)
                {
                    branch.Use(middleware);
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