namespace TokenAuth.Api.Models
{
    public class Account
    {
        public string AccountId { get; set; }

        public string Email { get; set; }

        public string PasswordHash { get; set; }

        public string Salt { get; set; }

        public string FacebookId { get; set; }

        public Account(string email)
        {
            this.Email = email;
        }

        public Account(string email, string facebookId)
        {
            this.Email = email;
            this.FacebookId = facebookId;
        }
    }
}