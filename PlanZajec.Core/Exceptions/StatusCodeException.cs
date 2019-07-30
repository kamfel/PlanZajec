using System;
using System.Net;

namespace PlanZajec.Core
{
    /// <summary>
    /// Thrown when HTML Status Code isn't the code that is expected
    /// </summary>
    [Serializable]
    public class StatusCodeException : Exception
    {
        public HttpStatusCode StatusCode { get; }

        public StatusCodeException() { }

        public StatusCodeException(string message) : base(message) { }

        public StatusCodeException(string message, HttpStatusCode statusCode) : base(message)
        {
            StatusCode = statusCode;
        }

        public StatusCodeException(string message, Exception inner) : base(message, inner) { }

        protected StatusCodeException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
