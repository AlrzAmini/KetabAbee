using Ganss.XSS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KetabAbee.Application.Security;

namespace KetabAbee.Application.Extensions
{
    public static class SecurityExtensions
    {
        public static string Sanitizer(this string value)
        {
            var htmlSanitizer = new HtmlSanitizer
            {
                AllowDataAttributes = true,
                KeepChildNodes = true
            };
            return htmlSanitizer.Sanitize(value);
        }

        public static string CheckPasswordStrength(this string password)
        {
            return PasswordStrengthChecker.CheckStrength(password).GetEnumName();
        }
    }
}
