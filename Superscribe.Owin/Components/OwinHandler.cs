using System.IO;
using Superscribe.Engine;

namespace Superscribe.Owin.Components
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Superscribe.Owin.Engine;
    using Superscribe.Owin.Extensions;

    public class OwinHandler
    {
        private readonly IOwinRouteEngine engine;

        public OwinHandler(Func<IDictionary<string, object>, Task> next, IOwinRouteEngine engine)
        {
            this.engine = engine;
        }

        public async Task Invoke(IDictionary<string, object> environment)
        {
            var routeData = environment["superscribe.RouteData"] as OwinRouteData;
            
            if (routeData.ExtraneousMatch)
            {
                environment.SetResponseStatusCode(404);
                environment.WriteResponse("404 - Route match failed");
                return;
            }

            if (routeData.NoMatchingFinalFunction)
            {
                environment.SetResponseStatusCode(405);
                environment.WriteResponse("405 - No final function was configured for this method");
                return;
            }

            // Set status code
            if (routeData.StatusCode > 0)
            {
                environment.SetResponseStatusCode(routeData.StatusCode);
            }

            var response = routeData.Response as Response;
            if (response != null)
            {
                InvokeResponse(environment, response);
                return;
            }            

            string[] outgoingMediaTypes;
            if (environment.TryGetHeaderValues("accept", out outgoingMediaTypes))
            {
                var mediaTypes = ConnegHelpers.GetWeightedValues(outgoingMediaTypes);
                var mediaType = mediaTypes.FirstOrDefault(o => this.engine.Config.MediaTypeHandlers.Keys.Contains(o) && this.engine.Config.MediaTypeHandlers[o].Write != null);
                if (!string.IsNullOrEmpty(mediaType))
                {
                    var formatter = this.engine.Config.MediaTypeHandlers[mediaType];
                    environment.SetResponseContentType(mediaType);

                    await formatter.Write(environment, routeData.Response);

                    return;
                }
            }

            // Is there already a response?
            if (routeData.Response != null)
            {
                return;
            }

            throw new NotSupportedException("Response type is not supported");
        }

        private static void InvokeResponse(IDictionary<string, object> environment, Response response)
        {
            var owinResponseHeaders = Get<IDictionary<string, string[]>>(environment, "owin.ResponseHeaders");
            var owinResponseBody = Get<Stream>(environment, "owin.ResponseBody");            
            owinResponseHeaders["Content-Type"] = new[] { response.ContentType };
            response.Contents(owinResponseBody);
        }

        private static T Get<T>(IDictionary<string, object> env, string key)
        {
            object value;
            return env.TryGetValue(key, out value) && value is T ? (T)value : default(T);
        }

    }
}
