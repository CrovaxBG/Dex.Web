using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;

namespace Dex.Common.Extensions
{
    public static class StringExtensions
    {
        public static string ToLowerFirstChar(this string input)
        {
            var newString = input;
            if (!string.IsNullOrEmpty(newString) && char.IsUpper(newString[0]))
            {
                newString = char.ToLower(newString[0]) + newString.Substring(1);
            }

            return newString;
        }

        public static bool IsValidEmailAddress(this string emailAddress)
        {
            const string pattern = @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z";
            return Regex.IsMatch(emailAddress, pattern, RegexOptions.IgnoreCase);
        }
    }
}
