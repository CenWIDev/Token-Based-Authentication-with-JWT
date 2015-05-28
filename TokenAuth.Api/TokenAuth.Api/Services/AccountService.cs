using System;
using System.Collections.Generic;
using System.Linq;
using TokenAuth.Api.Factories;
using TokenAuth.Api.Models;

namespace TokenAuth.Api.Services
{
    public static class AccountService
    {
        private static Dictionary<string, Account> AccountStore = new Dictionary<string,Account>();

        /// <summary>
        /// Creates an account and adds it to the AccountStore
        /// </summary>
        /// <param name="email">the email the account logs in with</param>
        /// <param name="password">the password the account logs in with</param>
        /// <returns>the id of the account created</returns>
        public static string CreateEmailPasswordAccount(string email, string password)
        {
            Account account = new Account(email);
            account.AccountId = (AccountStore.Count + 1).ToString();
            
            account.Salt = Guid.NewGuid().ToString();
            account.PasswordHash = HashFactory.GetHashString(account.Salt + password);

            AccountStore.Add(account.AccountId, account);

            return account.AccountId;
        }

        /// <summary>
        /// Creates an account and adds it to the AccountStore
        /// </summary>
        /// <param name="facebookId">the facebook id the account logs in with</param>
        /// <returns>the id of the account created</returns>
        public static string CreateFacebookAccount(string facebookId)
        {
            Account account = new Account(null, facebookId);
            account.AccountId = (AccountStore.Count + 1).ToString();

            AccountStore.Add(account.AccountId, account);

            return account.AccountId;
        }

        /// <summary>
        /// Finds an account by email. If not found, it returns null
        /// </summary>
        /// <param name="email">the email to search for</param>
        /// <returns>the account that was found or null if not found</returns>
        public static Account FindByEmail(string email)
        {
            Account foundAccount = AccountStore.Values.FirstOrDefault(a => a.Email == email);
            return foundAccount;
        }

        /// <summary>
        /// Finds an account by facebook id. If not found, it returns null
        /// </summary>
        /// <param name="facebook id">the id to search for</param>
        /// <returns>the account that was found or null if not found</returns>
        public static Account FindByFacebookId(string facebookId)
        {
            Account foundAccount = AccountStore.Values.FirstOrDefault(a => a.FacebookId == facebookId);
            return foundAccount;
        }

        /// <summary>
        /// verifies an account id exists in the account store
        /// </summary>
        /// <param name="accountId">the id to seek</param>
        /// <returns>whether or not the id was found</returns>
        public static bool VerifyAccountId(string accountId)
        {
            return AccountStore.ContainsKey(accountId);
        }

        /// <summary>
        /// Gets an account from the account store using the accountId
        /// </summary>
        /// <param name="accountId">the identifier of the record</param>
        /// <returns>the account that was found</returns>
        public static Account GetAccountById(string accountId)
        {
            try
            {
                return AccountStore[accountId];
            }
            catch (Exception)
            {
                throw new NotFoundException(string.Format("Could not find account with Id {0}", accountId));
            }
        }

        /// <summary>
        /// Verifies a password is the same as what is in storage
        /// </summary>
        /// <param name="account">the account information that holds the password hash and salt</param>
        /// <param name="password">the password to verify</param>
        /// <returns>whether or not the password is verfied</returns>
        public static bool VerifyPassword(Account account, string password)
        {
            return account.PasswordHash == HashFactory.GetHashString(account.Salt + password);
        }
    }
}