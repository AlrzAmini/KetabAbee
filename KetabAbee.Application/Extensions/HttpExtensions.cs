using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KetabAbee.Application.Interfaces.User;
using Microsoft.AspNetCore.Http;

namespace KetabAbee.Application.Extensions
{
    public static class HttpExtensions
    {
        public static string GetUserIp(this HttpContext httpContext)
        {
            return httpContext.Connection.RemoteIpAddress.ToString();
        }

        public static string GetCurrentUrl(this HttpContext httpContext)
        {
            return $"{httpContext.Request.Scheme}://{httpContext.Request.Host}{httpContext.Request.Path}{httpContext.Request.QueryString}";
        }

        public static List<int> GetUserRoleIds(this HttpContext httpContext)
        {
           var userService = (IUserService)httpContext.RequestServices.GetService(typeof(IUserService));

           return userService.GetUserRoleIds(httpContext.User.GetUserId());
        }
    }
}
