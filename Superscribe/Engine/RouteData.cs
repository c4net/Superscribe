using Superscribe.Models;

namespace Superscribe.Engine
{
    using System.Collections.Generic;

    using Superscribe.Utils;

    public class RouteData : IModuleRouteData
    {

        public IDictionary<string, object> QueryParameters { get; set; }

        public IDictionary<string, object> Parameters { get; set; }

        public IDictionary<string, object> Environment { get; set; }
        
        public object Response { get; set; }
        
        public bool ExtraneousMatch { get; set; }
        
        public bool NoMatchingFinalFunction { get; set; }

        public bool FinalFunctionExecuted { get; set; }

        public string Method { get; set; }

        public string Url { get; set; }
        
        public RouteData()
        {
            this.Environment = new Dictionary<string, object>();
            this.Parameters = new Dictionary<string, object>();
        }

        public virtual T Bind<T>() where T : class
        {
            throw new System.NotImplementedException();
        }

        public virtual T Require<T>() where T : class
        {
            throw new System.NotImplementedException();
        }
    }
}
