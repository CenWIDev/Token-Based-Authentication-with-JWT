namespace TokenAuth.Api.Models
{
    public class AccountViewModel
    {
        public string Email { get; set; }

        public string Name
        {
            get 
            {
                string firstName = this.Email.Split(new char[] { '@' })[0];
                firstName = firstName.Substring(0, 1).ToUpper() + firstName.Substring(1);
                return firstName + " Here"; 
            }
        }

        public AccountViewModel(Account dataModel)
        {
            this.Email = dataModel.Email;
        }
    }
}