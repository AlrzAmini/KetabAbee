using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using KetabAbee.Domain.Models.User;

namespace KetabAbee.Application.Extensions
{
    public static class CommonExtensions
    {
        public static string GetEnumName(this Enum value)
        {
            var enumName = value.GetType().GetMember(value.ToString()).FirstOrDefault();

            return enumName != null ? enumName.GetCustomAttribute<DisplayAttribute>()?.GetName() : "";
        }

        public static int GetUserId(this ClaimsPrincipal claim)
        {
            var data = claim?.Claims.SingleOrDefault(s => s.Type == ClaimTypes.NameIdentifier);
            return data != null ? Convert.ToInt32(data.Value) : default;
        }

        public static string GetUserEmail(this ClaimsPrincipal claim)
        {
            var data = claim?.Claims.SingleOrDefault(s => s.Type == ClaimTypes.Email);
            return data != null ? Convert.ToString(data.Value) : default;
        }

        public static string GetUserName(this ClaimsPrincipal claim)
        {
            var data = claim?.Claims.SingleOrDefault(s => s.Type == ClaimTypes.Name);
            return data != null ? Convert.ToString(data.Value) : default;
        }

        public static string ToTooman(this long price)
        {
            return price.ToString("#,0") + " تومان ";
        }

        public static string ToTooman(this float price)
        {
            return price.ToString("#,0") + " تومان ";
        }

        public static int? GetAgeByDateTime(this DateTime? birthDate)
        {
            if (birthDate == null) return null;

            var dateNow = DateTime.Now;
            var age = dateNow.Year - birthDate.Value.Year;

            if (dateNow.Month < birthDate.Value.Month || dateNow.Month == birthDate.Value.Month && dateNow.Day < birthDate.Value.Day)
                age--;

            return age;

        }

        public static string TruncateLongString(this string str, int maxLength)
        {
            var res = str?[..Math.Min(str.Length, maxLength)];
            return res + " ... ";
        }

        public static string NameFixerForUrl(this string str)
        {
            return str.Replace(" ", "-");
        }

        public static bool IsAllCharEnglish(this string input)
        {
            return input.ToCharArray()
                .All(item => char.IsLower(item) ||
                             char.IsUpper(item) ||
                             char.IsDigit(item) ||
                             char.IsWhiteSpace(item));
        }

        public static string ToMegaByteForm(this float value)
        {
            return value + " مگابایت ";
        }

        public static string DomainAddress = "https://localhost:44338";
    }
}
