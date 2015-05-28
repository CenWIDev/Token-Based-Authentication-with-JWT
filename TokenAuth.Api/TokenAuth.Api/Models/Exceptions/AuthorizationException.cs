using System.Net;

namespace TokenAuth.Api.Models
{
    public class AuthorizationException : ApiException
    {
        public AuthorizationException(string message)
            : base(HttpStatusCode.Unauthorized, message)
        {

        }
    }
}