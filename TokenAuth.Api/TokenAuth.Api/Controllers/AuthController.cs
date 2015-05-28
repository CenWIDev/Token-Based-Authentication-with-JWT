using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using TokenAuth.Api.Models;
using TokenAuth.Api.Services;

namespace TokenAuth.Api.Controllers
{
    public class AuthController : ApiController
    {
        public HttpResponseMessage Register(AuthRequest data)
        {
            // Validate all your Request Data!

            Account foundAccount = AccountService.FindByEmail(data.Email);

            // If trying to register with an email already registered, respond with a conflict
            if (foundAccount != null)
            {
                return this.Request.CreateResponse(HttpStatusCode.Conflict, new {
                    Message = "An account already exists with this email"
                });
            }

            // Account does not already exist
            string accountId = AccountService.CreateEmailPasswordAccount(data.Email, data.Password);
            string token = JwtService.CreateToken(accountId);

            return this.Request.CreateResponse(HttpStatusCode.OK, new { Token = token });
        }

        public HttpResponseMessage Login(AuthRequest data)
        {
            // Validate all your Request Data!

            Account foundAccount = AccountService.FindByEmail(data.Email);

            // If trying to log in with an email that does not exist or a password that does not match
            if (foundAccount == null || AccountService.VerifyPassword(foundAccount, data.Password) == false)
            {
                return this.Request.CreateResponse(HttpStatusCode.Forbidden, new
                {
                    Message = "Either the username or password was incorrect"
                });
            }

            // Login was verified
            string token = JwtService.CreateToken(foundAccount.AccountId);

            return this.Request.CreateResponse(HttpStatusCode.OK, new { Token = token });
        }

        public async Task<HttpResponseMessage> Facebook(FacebookAuthRequest data)
        {
            // Validate all your Request Data!

            string authToken = await FacebookService.GetAccessToken(data.ClientId, data.RedirectUri, data.Code);
            string id = await FacebookService.GetProfile(authToken);

            string accountId;
            Account foundAccount = AccountService.FindByFacebookId(id);

            // If not found, create the user
            if (foundAccount != null)
            {
                accountId = AccountService.CreateFacebookAccount(id);
            }
            else
            {
                accountId = foundAccount.AccountId;
            }

            // Account does not already exist
            
            string token = JwtService.CreateToken(accountId);

            return this.Request.CreateResponse(HttpStatusCode.OK, new { Token = token });
        }
    }
}
