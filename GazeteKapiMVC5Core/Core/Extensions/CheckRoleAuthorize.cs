using GazeteKapiMVC5Core.Models.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GazeteKapiMVC5Core.Core.Extensions
{
    public class CheckRoleAuthorize : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.HttpContext.Session.GetString("user") == null)
            {
                //context.HttpContext.Response.Redirect("/Yonetici/GirisYap");
                context.Result = new RedirectResult("/Yonetici/GirisYap");
            }
            base.OnActionExecuting(context);
        }
    }

    public class RoleAuthorizeAttribute : AuthorizeAttribute, IAuthorizationFilter
    {
        private string[] Permission;

        public RoleAuthorizeAttribute(params string[] permission)
        {
            Permission = permission;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            UserBaseViewModel model = SessionExtensionMethod.GetObject<UserBaseViewModel>(context.HttpContext.Session, "user");
            
            context.Result = new ForbidResult();
        }
    }
}
