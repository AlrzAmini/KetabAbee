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
            return int.Parse(claim.FindFirst(ClaimTypes.NameIdentifier).Value);
        }
    }
}
