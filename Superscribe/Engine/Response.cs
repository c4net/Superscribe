using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace Superscribe.Engine
{

    public class Response
    {

        /// <summary>
        /// Null object representing no body
        /// </summary>
        public static Action<Stream> NoBody = s => { };

        private string _contentType;

        /// <summary>
        /// Initializes a new instance of the <see cref="Response"/> class.
        /// </summary>
        public Response()
        {
            Contents = NoBody;
            ContentType = "text/html";
            Headers = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            StatusCode = HttpStatusCode.OK;
        }

        /// <summary>
        /// Gets the collection of HTTP response headers that should be sent back to the client.
        /// </summary>
        /// <value>An <see cref="IDictionary{TKey,TValue}"/> instance, containing the key/value pair of headers.</value>
        public IDictionary<string, string> Headers { get; set; }

        /// <summary>
        /// Gets or sets the type of the content.
        /// </summary>
        /// <value>The type of the content.</value>
        /// <remarks>The default value is <c>text/html</c>.</remarks>
        public string ContentType
        {
            get { return Headers.ContainsKey("content-type") ? Headers["content-type"] : _contentType; }
            set { _contentType = value; }
        }

        /// <summary>
        /// Gets the delegate that will render contents to the response stream.
        /// </summary>
        /// <value>An <see cref="Action{T}"/> delegate, containing the code that will render contents to the response stream.</value>
        /// <remarks>The host of Nancy will pass in the output stream after the response has been handed back to it by Nancy.</remarks>
        public Action<Stream> Contents { get; set; }

        /// <summary>
        /// Gets or sets the HTTP status code that should be sent back to the client.
        /// </summary>
        /// <value>A <see cref="HttpStatusCode"/> value.</value>
        public HttpStatusCode StatusCode { get; set; }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <remarks>This method can be overridden in sub-classes to dispose of response specific resources.</remarks>
        public virtual void Dispose()
        {
        }

    }

}
