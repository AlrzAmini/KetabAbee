using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using KetabAbee.Application.Extensions;
using KetabAbee.Application.Interfaces.User;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace KetabAbee.Web.Models
{
    public class IpFilter
    {
        private readonly RequestDelegate _next;
        private IUserService _userService;

        public IpFilter(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            _userService = (IUserService)context.RequestServices.GetService(typeof(IUserService));

            if (_userService != null)
            {
                var ipAddress = context.GetUserIp();
                var blackList = await _userService.GetBannedIps();
                var isInBlackListIpList = blackList
                    .Any(ip => ip == ipAddress);
                if (isInBlackListIpList)
                {
                    context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                    return;
                }
            }

            await _next.Invoke(context);
        }
    }
}
