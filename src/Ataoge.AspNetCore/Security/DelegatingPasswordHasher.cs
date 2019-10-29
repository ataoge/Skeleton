using System;
using System.Collections.Generic;

namespace Microsoft.AspNetCore.Identity
{
    public class DelegatingPasswordHasher<TUser> : IPasswordHasher<TUser> where TUser : class
    {
        private const string PREFIX = "{";
        private const string SUFFIX = "}";

        private string defaultName = null;
        private IPasswordHasher<TUser> defaultPasswordHasher;
        private IDictionary<string, IPasswordHasher<TUser>> dicts = null;
        public DelegatingPasswordHasher(string name, IDictionary<string, IPasswordHasher<TUser>> dicts)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException();

            this.defaultName = name;
            this.defaultPasswordHasher = dicts[name];
            this.dicts = new Dictionary<string, IPasswordHasher<TUser>>(dicts);

        }

        public string HashPassword(TUser user, string password)
        {
            return "{" + this.defaultPasswordHasher + "}" + defaultPasswordHasher.HashPassword(user, password);
        }

        public PasswordVerificationResult VerifyHashedPassword(TUser user, string hashedPassword, string providedPassword)
        {
            if (providedPassword == null && hashedPassword == null)
            {
                return PasswordVerificationResult.Success;
            }
            else
            {
                string name = this.ExtractName(hashedPassword);
                var passwordHasher = dicts[name];
                if (passwordHasher == null)
                {
                    throw new Exception("cannot find PasswordHasher");
                }
                else
                {
                    string password = this.ExtractEncodedPassword(hashedPassword);
                    return passwordHasher.VerifyHashedPassword(user, password, providedPassword);
                }
            }
            
        }

        private string ExtractName(string prefixEncodedPassword)
        {
            if (prefixEncodedPassword == null)
            {
                return null;
            }
            else
            {
                int start = prefixEncodedPassword.IndexOf(PREFIX);
                if (start != 0)
                {
                    return null;
                }
                else
                {
                    int end = prefixEncodedPassword.IndexOf(SUFFIX, start);
                    return end < 0 ? null : prefixEncodedPassword.Substring(start + 1, end);
                }
            }
        }
        
        private string ExtractEncodedPassword(string prefixEncodedPassword)
        {
            int start = prefixEncodedPassword.IndexOf(SUFFIX);
            return prefixEncodedPassword.Substring(start + 1);
        }

        public bool UpgradeEncoding(string encodedPassword)
        {
            string name = this.ExtractName(encodedPassword);
            return !string.Equals(this.defaultName, name, StringComparison.InvariantCultureIgnoreCase);
        }
    }
}