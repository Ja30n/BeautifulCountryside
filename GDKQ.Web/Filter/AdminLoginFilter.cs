using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GDKQ.Web.Filter
{
    public class AdminLoginFilter: ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //HttpContext.Current.Response.Write("OnActionExecuting:正要准备执行Action的时候但还未执行时执行<br />");
            if (HttpContext.Current.Session["admin"] == null)
            {
                HttpContext.Current.Response.Write("<script>alert('您无权访问该页面，请重新登录');location.href='/Login'</script>");
                HttpContext.Current.Response.End();
                return;
            }
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            //HttpContext.Current.Response.Write("OnActionExecuted:Action执行时但还未返回结果时执行<br />");
        }

        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            // HttpContext.Current.Response.Write("OnResultExecuting:OnResultExecuting也和OnActionExecuted一样，但前者是在后者执行完后才执行<br />");
        }

        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            // HttpContext.Current.Response.Write("OnResultExecuted:是Action执行完后将要返回ActionResult的时候执行<br />");
        }
    }
}