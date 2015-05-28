using System.Net;
using System.Net.Http;
using System.Web.Http;
using TokenAuth.Api.Models;
using TokenAuth.Api.Services;

namespace TokenAuth.Api.Controllers
{
    public class UserController : ApiController
    {
        public HttpResponseMessage GetUser()
        {
            try
            {
                string authToken = AuthService.CheckAuthorizationHeader(this.Request);
                string accountId = JwtService.Verify(authToken);

                Account account = AccountService.GetAccountById(accountId);
                AccountViewModel responseData = new AccountViewModel(account);
                
                return this.Request.CreateResponse(HttpStatusCode.OK, responseData);
            }
            catch (ApiException exp)
            {
                return this.Request.CreateErrorResponse(exp.StatusCode, exp.Message);
            }
        }
    }
}