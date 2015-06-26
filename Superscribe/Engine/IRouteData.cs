using Superscribe.Utils;

namespace Superscribe.Engine
{
    using System.Collections.Generic;

    public interface IRouteData
    {
        string Method { get; set; }

        string Url { get; set; }

        IDictionary<string, object> QueryParameters { get; set; }

        IDictionary<string, object> Parameters { get; set; }

        IDictionary<string, object> Environment { get; set; }
        
        object Response { get; set; }
        
        bool ExtraneousMatch { get; set; }

        bool FinalFunctionExecuted { get; set; }

        bool NoMatchingFinalFunction { get; set; }
    }
}
