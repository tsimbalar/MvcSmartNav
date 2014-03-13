using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcSmartNav;
using MvcSmartNav.Enablement;

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

        [SmartNavEnabledAuthorize()]
        [SmartNavVisibleAuthorize()]
        public ViewResult SomeRestrictedPage()
        {
            ViewBag.Message = "Some VIP page";
            return View();
        }
    }


    public abstract class SmartNavDisplayFilter : Attribute, IAuthorizationFilter
    {
        private readonly IAuthorizationFilter _wrapped;

        public SmartNavDisplayFilter(IAuthorizationFilter filter)
        {
            _wrapped = filter;
        }

        protected IAuthorizationFilter InnerAttribute
        {
            get { return _wrapped; }
        }

        /// <summary>
        /// Called when calling method directly
        /// </summary>
        /// <param name="filterContext"></param>
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            InnerAttribute.OnAuthorization(filterContext);
        }


    }

    /// <summary>
    /// Trying to look like the MVC AuthorizeAttribute
    /// </summary>
    public class SmartNavEnabledAuthorize : AuthorizeAttribute, ISmartNavEnabledAttribute
    {

        // Methods when in another action, but asking if this action should be visible in the menu
        // TODO : !!!!


        public NodeEnablement EvaluateEnablement(ViewContext callingViewContext)
        {
            var authorized = this.AuthorizeCore(callingViewContext.HttpContext);
            if (authorized)
            {
                return new NodeEnablement(disabled: false, reason: "User is authorized");
            }
            else
            {
                return new NodeEnablement(disabled: true,
                                          reason:
                                              string.Format(
                                                  "not authorized according to SmartNavEnabledAuthorize (Roles = {0}, Users = {1})",
                                                  Roles, Users));
            }
        }
    }
}
