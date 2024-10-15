using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sales_Order.Models
{
    public class CheckSessionTimeOut
    {
    }
    public class CheckSessionAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(System.Web.Mvc.ActionExecutingContext filterContext)
        {

            if (HttpContext.Current.Session["mode"] == null)
            {
                filterContext.Result = new RedirectResult("/Order/NewOrder?mode=DIGITS-LMD");
                return;
            }
            base.OnActionExecuting(filterContext);
        }
    }
}