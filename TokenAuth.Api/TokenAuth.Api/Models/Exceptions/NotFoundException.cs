using System.Net;

namespace TokenAuth.Api.Models
{
    public class NotFoundException : ApiException
    {
        public NotFoundException(string message)
            : base(HttpStatusCode.NotFound, message)
        {
        }
    }
}