using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebProject.Controllers
{
    public abstract class AbstractController : Controller
    {
        protected override void OnActionExecuting(ActionExecutingContext ctx)
        {
            if (Request.IsAuthenticated)
            {
                base.OnActionExecuting(ctx);
            }
            else
            {
                ctx.Result = base.RedirectToAction("Login", "Account");
            }
        }
    }
}