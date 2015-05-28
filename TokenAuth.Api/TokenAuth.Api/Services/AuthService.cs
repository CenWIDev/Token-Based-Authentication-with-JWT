using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TokenAuth.Api.Models;

namespace TokenAuth.Api.Services
{
    public static class AuthService
    {
        public static string CheckAuthorizationHeader(HttpRequestMessage request)
        {
            if (request.Headers.Authorization == null)
            {
                throw new AuthorizationException("Authorization header is not present");
            }
            else if (request.Headers.Authorization.Scheme != "Bearer")
            {
                throw new AuthorizationException("Authorization header is not using the correct scheme");
            }
            else if (request.Headers.Authorization.Parameter == null)
            {
                throw new AuthorizationException("A token was not present in the authorization header");
            }
            else if (request.Headers.Authorization.Parameter.Split(new char[] { '.' }).Length != 3)
            {
                throw new AuthorizationException("Authorization header is malformed");
            }
            
            return request.Headers.Authorization.Parameter;
        }
    }
}
