namespace TokenAuth.Api.Models
{
    public class FacebookAuthRequest
    {
        public string ClientId { get; set; }

        public string RedirectUri { get; set; }

        public string Code { get; set; }
    }
}
