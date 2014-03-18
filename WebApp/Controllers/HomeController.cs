using System.Web.Mvc;
using MvcSmartNav.Attributes;

namespace WebApp.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ViewResult SomePage()
        {
            ViewBag.Message = "Some other page";
            return View();
        }

        [SmartNavAuthorize(WhenUnauthorized = SmartNavAttributeMode.Disable)]
        public ViewResult SomeRestrictedPage()
        {
            ViewBag.Message = "Some VIP page";
            return View();
        }

        [SmartNavAuthorize(WhenUnauthorized = SmartNavAttributeMode.Hide)]
        public ViewResult OtherRestrictedPage()
        {
            ViewBag.Message = "Some VIP page 2";
            return View();
        }
    }
}
