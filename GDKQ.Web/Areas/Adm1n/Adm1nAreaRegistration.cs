using System.Web.Mvc;

namespace GDKQ.Web.Areas.Adm1n
{
    public class Adm1nAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Adm1n";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Adm1n_default",
                "Adm1n/{controller}/{action}/{id}",
                new { controller="Home", action = "Index", id = UrlParameter.Optional },
                new string[] { "GDKQ.Web.Areas.Adm1n.Controllers" }
            );
        }
    }
}