using System;
using System.Threading.Tasks;

namespace Superscribe.Owin
{
    using System.Collections.Generic;
    using System.IO;

    using Superscribe.Owin.Extensions;

    public class SuperscribeOwinOptions : SuperscribeOptions
    {
        public SuperscribeOwinOptions()
        {
            this.MediaTypeHandlers = new Dictionary<string, MediaTypeHandler> {
                {
                    "text/html", 
                    GetTextHtmlMediaTypeHandler() 
                } 
            };
            
            this.ScanForModules = true;
        }

        private static MediaTypeHandler GetTextHtmlMediaTypeHandler()
        {
            return new MediaTypeHandler
            {
                Read = ReadTextHtml,
                Write = WriteTextHtml
            };
        }

        private static object ReadTextHtml(IDictionary<string, object> env, object obj)
        {
            using (var reader = new StreamReader(env.GetRequestBody())) return reader.ReadToEnd();
        }

        private static Task WriteTextHtml(IDictionary<string, object> env, object obj)
        {
            var value = string.Empty;
            if (obj != null)
            {
                value = obj.ToString();
            }

            return env.WriteResponse(value);
        }

        public Dictionary<string, MediaTypeHandler> MediaTypeHandlers { get; set; }

        public bool ScanForModules { get; set; }
    }
}
