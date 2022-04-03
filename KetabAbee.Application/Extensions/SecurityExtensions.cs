using Ganss.XSS;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KetabAbee.Application.Convertors;
using KetabAbee.Application.Security;
using Microsoft.AspNetCore.Http;

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

        public static bool IsValidEmail(this string email)
        {
            var trimmedEmail = email.Trim();

            if (trimmedEmail.EndsWith("."))
            {
                return false;
            }

            email = FixText.EmailFixer(email);
            try
            {
                var address = new System.Net.Mail.MailAddress(email);
                return address.Address == trimmedEmail;
            }
            catch
            {
                return false;
            }
        }

        public static bool IsValidAudio(this IFormFile file)
        {
            string[] extensions = { "m4a", "flac", "mp3", "mp4", "wav", "wma", "aac" };

            var extension = Path.GetExtension(file.FileName)?.ToLower();

            return extensions.Any(e => e == extension);
        }
    }
}
