using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KetabAbee.Web.Models;
using Microsoft.AspNetCore.Builder;

namespace KetabAbee.Web.PresentationExtensions
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseIpFilter(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<IpFilter>();
        }
    }
}
