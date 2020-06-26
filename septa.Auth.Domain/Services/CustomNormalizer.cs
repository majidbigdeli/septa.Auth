using Microsoft.AspNetCore.Identity;
using septa.Auth.Domain.Hellper;
using System;
using System.Linq;
using DNTPersianUtils.Core;

namespace septa.Auth.Domain.Services
{
    public class CustomNormalizer : ILookupNormalizer
    {
        private static string fixGmailDots(string email)
        {
            email = email.ToLowerInvariant().Trim();
            var emailParts = email.Split('@');
            var name = emailParts[0].Replace(".", string.Empty);

            var plusIndex = name.IndexOf("+", StringComparison.OrdinalIgnoreCase);
            if (plusIndex != -1)
            {
                name = name.Substring(0, plusIndex);
            }

            var emailDomain = emailParts[1];
            emailDomain = emailDomain.Replace("googlemail.com", "gmail.com");

            string[] domainsAllowedDots =
            {
                "gmail.com",
                "facebook.com"
            };

            var isFromDomainsAllowedDots = domainsAllowedDots.Any(domain => emailDomain.Equals(domain));
            return !isFromDomainsAllowedDots ? email : string.Format("{0}@{1}", name, emailDomain);
        }

        public string NormalizeName(string name)
        {
           return this.NormalizeKey(name);
        }

        public string NormalizeEmail(string email)
        {
            return this.NormalizeKey(email);
        }

        private string NormalizeKey(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                return null;
            }

            key = key.Trim();

            if (key.IsEmailAddress())
            {
                key = fixGmailDots(key);
            }
            else
            {
                key = key.ApplyCorrectYeKe()
                     .RemoveDiacritics()
                     .CleanUnderLines()
                     .RemovePunctuation();
                key = key.Trim().Replace(" ", "");
            }

            key = key.ToUpperInvariant();
            return key;
        }
    }


}
