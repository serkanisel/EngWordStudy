using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace EWS.Web
{
    public class AuthenticationAction : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            HttpContext context = HttpContext.Current;

            if (context.Session["CurrentUser"] == null)
            {
                if (!HttpContext.Current.Request.Path.EndsWith("Account/Login", StringComparison.InvariantCultureIgnoreCase))
                    context.Response.Redirect("~/Account/Login");
            }
        }
    }
}