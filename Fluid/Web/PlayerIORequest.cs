using System;
using System.Net;
using System.Text;

namespace Fluid.Web
{
    public class PlayerIORequest
    {
        /// <summary>
        /// Gets the request method
        /// </summary>
        public string Method { get; private set; }

        /// <summary>
        /// Gets the request uri
        /// </summary>
        public Uri Uri { get; private set; }

        /// <summary>
        /// Gets the request headers
        /// </summary>
        public WebHeaderCollection Headers { get; private set; }

        /// <summary>
        /// Gets the body of the request
        /// </summary>
        public byte[] Body { get; private set; }

        /// <summary>
        /// Decodes the body
        /// </summary>
        /// <param name="encoding">The encoding to use</param>
        /// <returns>The decoded body</returns>
        public string DecodeBody(Encoding encoding)
        {
            if (Body == null)
            {
                return null;
            }

            return encoding.GetString(Body);
        }

        /// <summary>
        /// Gets the debug string
        /// </summary>
        public override string ToString()
        {
            return string.Format("{0} {1}", Method, Uri);
        }

        /// <summary>
        /// Creates a new player io request
        /// </summary>
        /// <param name="method">The request method</param>
        /// <param name="uri">The uri</param>
        /// <param name="headers">The headers</param>
        /// <param name="body">The body</param>
        public PlayerIORequest(string method, Uri uri, WebHeaderCollection headers, byte[] body)
        {
            Method = method;
            Uri = uri;
            Headers = headers;
            Body = body;
        }
    }
}
