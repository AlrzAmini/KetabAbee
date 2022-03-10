using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KetabAbee.Application.Extensions;
using KetabAbee.Application.Interfaces.Permission;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace KetabAbee.Application.Security
{
    public class RoleCheckerAttribute : AuthorizeAttribute, IAuthorizationFilter
    {
        private IPermissionService _permissionService;
        private readonly int _roleId;

        public RoleCheckerAttribute(int roleId)
        {
            _roleId = roleId;
        }
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            _permissionService = (IPermissionService)context.HttpContext.RequestServices.GetService(typeof(IPermissionService));

            if (_permissionService != null)
            {
                if (context.HttpContext.User.Identity.IsAuthenticated)
                {
                    var userId = context.HttpContext.User.GetUserId();

                    if (!_permissionService.IsUserHaveRoleForCheckAttribute(userId, _roleId))
                    {
                        context.Result = new RedirectResult("/");
                    }
                }
                else
                {
                    context.Result = new RedirectResult("/Login");
                }
            }
            else
            {
                context.Result = new RedirectResult("/Login");
            }
        }
    }
}
