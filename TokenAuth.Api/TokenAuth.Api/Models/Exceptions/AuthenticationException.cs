using System.Net;

namespace TokenAuth.Api.Models
{
    public class AuthenticationException : ApiException
    {
        public AuthenticationException(string messaage)
            : base(HttpStatusCode.Forbidden, messaage)
        {
        }
    }
}