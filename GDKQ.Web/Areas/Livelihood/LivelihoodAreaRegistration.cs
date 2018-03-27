using System.Web.Mvc;

namespace GDKQ.Web.Areas.Livelihood
{
    public class LivelihoodAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Livelihood";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Livelihood_default",
                "Livelihood/{controller}/{action}/{id}",
                new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                new string[] { "GDKQ.Web.Areas.Livelihood.Controllers" }
            );
        }
    }
}