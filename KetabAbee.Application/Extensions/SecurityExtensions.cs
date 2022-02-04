using Ganss.XSS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
