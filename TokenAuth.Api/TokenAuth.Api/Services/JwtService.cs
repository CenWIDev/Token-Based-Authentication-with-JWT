using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TokenAuth.Api.Models;

namespace TokenAuth.Api.Services
{
    public class JwtService
    {
        private const string Secret = "MySup3rS3cr3tK3y!";

        /// <summary>
        /// Created a token that is good for 7 days from issuance for my service that contains
        /// the accountId as the subject
        /// </summary>
        /// <param name="accountId">the id of the account the token is for</param>
        /// <returns>a signed token</returns>
        public static string CreateToken(string accountId)
        {
            Dictionary<string, object> header = new Dictionary<string, object>
            {
                { "typ", "JWT" },
                { "alg", "none" }
            };

            Dictionary<string, object> payload = new Dictionary<string, object>
            {
                { "iss", "https://api.myawesomeservice.com" },
                { "iat", GetUnixTimestamp(DateTime.UtcNow) },
                { "exp", GetUnixTimestamp(DateTime.UtcNow.AddDays(7)) },
                { "aud", "https://www.myawesomeservice.com" },
                { "sub", accountId }
            };

            string encodedHeader = Base64EncodeDictionary(header);
            string encodedPayload = Base64EncodeDictionary(payload);

            string unsignedToken = encodedHeader + "." + encodedPayload;

            string signedToken = unsignedToken + "." + SignToken(unsignedToken, Secret);

            return signedToken;
        }

        /// <summary>
        /// Verifies the token is well-formed and that the accountId within the subject
        /// is a valid Id
        /// </summary>
        /// <param name="token">the token from the Authorization header</param>
        /// <returns>the accountId from the subject</returns>
        public static string Verify(string token)
        {
            string[] parts = token.Split(new char[] { '.' });

            string header = parts[0];
            string payload = parts[1];
            string signature = parts[2];

            // Verify there was no tampering with the payload or header
            if (signature != SignToken(header + "." + payload, Secret))
            {
                throw new AuthorizationException("The token could not be verified");
            }

            Dictionary<string, object> payloadData = Base64DecodeDictionary(payload);

            // Check if token expired
            if (GetUnixTimestamp(DateTime.UtcNow) > (long)payloadData["exp"])
            {
                throw new AuthorizationException("The token has expired");
            }

            string accountId = (string)payloadData["sub"];

            bool valid = AccountService.VerifyAccountId(accountId);

            if(valid == false)
            {
                throw new AuthenticationException("Could not verify the user exists");
            }

            return accountId;
        }

        /// <summary>
        /// Given an unsigned token, it will create a signature for the token
        /// </summary>
        /// <param name="unsignedToken">the base64 encoded and concatenated header and payload</param>
        /// <param name="secret">the token to encrypt the signature with</param>
        /// <returns>the signature for the token</returns>
        private static string SignToken(string unsignedToken, string secret)
        {
            var encoding = new ASCIIEncoding();

            byte[] signature;

            using (var crypto = new HMACSHA256(encoding.GetBytes(secret)))
            {
                signature = crypto.ComputeHash(encoding.GetBytes(unsignedToken));
            }

            string encodedSignature = Base64EncodeBytes(signature);

            return encodedSignature;
        }

        /// <summary>
        /// Takes in a dictionary, converts to JSON and base64 encodes it
        /// </summary>
        /// <param name="dictionary">the dictionary to encode</param>
        /// <returns>the base64 encoded string representation</returns>
        private static string Base64EncodeDictionary(Dictionary<string, object> dictionary)
        {
            string json = JsonConvert.SerializeObject(dictionary, Formatting.None);
            byte[] bytes = Encoding.UTF8.GetBytes(json);
            return Base64EncodeBytes(bytes);
        }

        /// <summary>
        /// Takes in a byte array and base64 encodes it
        /// </summary>
        /// <param name="bytes">the data to base 64 encode</param>
        /// <returns>the base64 encoded string</returns>
        private static string Base64EncodeBytes(byte[] bytes)
        {
            string base64String = Convert.ToBase64String(bytes);
            return base64String;
        }

        /// <summary>
        /// Takes in a string that represents a base64 encoded dictionary and decodes it
        /// </summary>
        /// <param name="str">the base64 encoded json string</param>
        /// <returns>the decoded dictionary</returns>
        public static Dictionary<string, object> Base64DecodeDictionary(string str)
        {
            byte[] bytes = Convert.FromBase64String(str);
            string json = Encoding.UTF8.GetString(bytes);
            Dictionary<string, object> dictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
            return dictionary;
        }

        /// <summary>
        /// Gets the number of seconds since 1/1/1970 for a given date
        /// </summary>
        /// <param name="timeToConvert">the date to get the timestamp for</param>
        /// <returns>the timestamp between the provided date and 1/1/1970</returns>
        private static int GetUnixTimestamp(DateTime timeToConvert)
        {
            return (Int32)(timeToConvert.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
        }
    }
}
