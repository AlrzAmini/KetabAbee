using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
//using ShoraaDahak.Core.Services.Interfaces;

namespace KetabAbee.Application.Security
{
    //public class PermissionCheckerAttribute : AuthorizeAttribute, IAuthorizationFilter
    //{
    //    private IPermissionService _permissionService;
    //    private int _permissionId = 0;

    //    public PermissionCheckerAttribute(int permissionId)
    //    {
    //        _permissionId = permissionId;
    //    }
    //    public void OnAuthorization(AuthorizationFilterContext context)
    //    {
    //        _permissionService = (IPermissionService)context.HttpContext.RequestServices.GetService(typeof(IPermissionService));


    //        if (context.HttpContext.User.Identity.IsAuthenticated)
    //        {
    //            string email = context.HttpContext.User.FindFirst(ClaimTypes.Email).Value;

    //            if (!_permissionService.CheckPermission(_permissionId, email))
    //            {
    //                context.Result = new RedirectResult("/Login");
    //            }
    //        }
    //        else
    //        {
    //            context.Result = new RedirectResult("/Login");
    //        }
    //    }
    //}
}
