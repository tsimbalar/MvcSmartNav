using System.Collections;
using System.Web;
using System.Web.Mvc;
using MvcSmartNav;
using MvcSmartNav.Enablement;
using MvcSmartNav.Visibility;

namespace WebApp.Controllers
{
    /// <summary>
    /// Controller used to generate the contents of navigation for the pages
    /// </summary>
    public class NavigationController : Controller
    {
        [ChildActionOnly] // not called directly !
        public ActionResult GenerateNav(ViewContext callingContext)
        {
            var root = new NavRoot("Home page", Url.Action("Index", "Home"))
                              {
                                  Tooltip = "Home"
                              };

            var child1 = new NavItem("Home link")
                                 {
                                     Tooltip = "Tooltip of firstElement",
                                     TargetUrl = Url.Action("Index", "Home")
                                 };
            var child1_sub1 = new NavItem("Some Page")
                                  {
                                      Tooltip = "Some Page",
                                      TargetUrl = Url.Action("SomePage", "Home")
                                  };
            child1.AddChild(child1_sub1);

            var child1_sub2 = new NavItem("Restricted Page")
            {
                Tooltip = "Some super secret Page",
                TargetUrl = Url.Action("SomeRestrictedPage", "Home"),
                EnabilityStrategy = new AuthorizationEnabledStrategy<HomeController,ViewResult>(c=> c.SomeRestrictedPage())
            };
            child1.AddChild(child1_sub2);

            root.AddChild(child1);

            var child2 = new NavItem("Hidden Home link")
                                  {
                                      Tooltip = "Tooltip of secondElement",
                                      TargetUrl = Url.Action("Index", "Home"),
                                      VisibilityStrategy = new NeverVisibleStrategy()
                                  };
            root.AddChild(child2);

            var child2_sub1 = new NavItem("subnav")
                                {
                                    Tooltip = "Tooltip of subnav",
                                    TargetUrl = Url.Action("Contact", "Home"),
                                    VisibilityStrategy = new NeverVisibleStrategy()
                                };
            child2.AddChild(child2_sub1);

            var child2_sub2 = new NavItem("subnav")
            {
                Tooltip = "Tooltip of subnav",
                TargetUrl = Url.Action("Contact", "Home"),
                EnabilityStrategy = new AlwaysDisabledStrategy()
            };

            var child2sub2_sub1 = new NavItem("subsub")
                                      {
                                          Tooltip = "rien",
                                          TargetUrl = Url.Action("About", "Home")
                                      };
            child2_sub2.AddChild(child2sub2_sub1);

            child2.AddChild(child2_sub2);

            var child3 = new NavItem("About link")
            {
                Tooltip = "Tooltip of 3rd Element",
                TargetUrl = Url.Action("About", "Home")
            };
            root.AddChild(child3);


            var child4 = new NavItem("random link")
            {
                Tooltip = "Tooltip of 4th Element",
                TargetUrl = "http://www.google.fr",
                EnabilityStrategy = new AlwaysDisabledStrategy()
            };

            var child4_sub1 = new NavItem("Some link")
            {
                Tooltip = "Tooltip of 4.1th Element",
                TargetUrl = "http://www.yahoo.com"
            };
            child4.AddChild(child4_sub1);


            var child4_sub2 = new NavItem("another link")
            {
                Tooltip = "Tooltip of 4.2th Element",
                TargetUrl = Url.Action("Index", "Home"),
                VisibilityStrategy = new NeverVisibleStrategy()
            };
            child4.AddChild(child4_sub2);


            root.AddChild(child4);

            var model = new NavBuilder().Build(callingContext, root);

            return PartialView("Nav.partial", model);
        }
    }
}
