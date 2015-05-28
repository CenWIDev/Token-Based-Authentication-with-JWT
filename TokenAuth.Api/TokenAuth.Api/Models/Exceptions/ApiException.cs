using System;
using System.Net;

namespace TokenAuth.Api.Models
{
    public abstract class ApiException : Exception
    {
        public HttpStatusCode StatusCode { get; protected set; }

        public ApiException(HttpStatusCode statusCode, string message)
            : base(message)
        {
            this.StatusCode = statusCode;
        }
    }
}