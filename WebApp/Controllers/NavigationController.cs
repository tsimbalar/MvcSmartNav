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

            var child1 = new NavItem("Home link", Url.Action("Index", "Home"))
                                 {
                                     Tooltip = "Tooltip of firstElement",
                                 };
            var child1_sub1 = new NavItem("Some Page", Url.Action("SomePage", "Home"))
                                  {
                                      Tooltip = "Some Page"
                                  };
            child1.AddChild(child1_sub1);

            var child1_sub2 = new MvcActionNavItem<HomeController>("Restricted Page", "Home", "SomeRestrictedPage")
            {
                Tooltip = "Some super secret Page (will disable if not allowed)",
                EnabilityStrategy = new AuthorizationEnabledStrategy<HomeController>()
            };
            child1.AddChild(child1_sub2);

            var child1_sub3 = new MvcActionNavItem<HomeController>("Restricted Page", "Home", "SomeRestrictedPage")
            {
                Tooltip = "Some super secret Page (will hide if not allowed)",
                VisibilityStrategy = new AuthorizationVisibleStrategy<HomeController>()
            };
            child1.AddChild(child1_sub3);

            root.AddChild(child1);

            var child2 = new NavItem("Hidden Home link", Url.Action("Index", "Home"))
                                  {
                                      Tooltip = "Tooltip of secondElement",
                                      VisibilityStrategy = new NeverVisibleStrategy()
                                  };
            root.AddChild(child2);

            var child2_sub1 = new NavItem("subnav", Url.Action("Contact", "Home"))
                                {
                                    Tooltip = "Tooltip of subnav",
                                    VisibilityStrategy = new NeverVisibleStrategy()
                                };
            child2.AddChild(child2_sub1);

            var child2_sub2 = new NavItem("subnav", Url.Action("Contact", "Home"))
            {
                Tooltip = "Tooltip of subnav",
                EnabilityStrategy = new AlwaysDisabledStrategy()
            };

            var child2sub2_sub1 = new NavItem("subsub", Url.Action("About", "Home"))
                                      {
                                          Tooltip = "rien",
                                      };
            child2_sub2.AddChild(child2sub2_sub1);

            child2.AddChild(child2_sub2);

            var child3 = new NavItem("About link", Url.Action("About", "Home"))
            {
                Tooltip = "Tooltip of 3rd Element",
            };
            root.AddChild(child3);


            var child4 = new NavItem("random link", "http://www.google.fr")
            {
                Tooltip = "Tooltip of 4th Element",
                EnabilityStrategy = new AlwaysDisabledStrategy()
            };

            var child4_sub1 = new NavItem("Some link", "http://www.yahoo.com")
            {
                Tooltip = "Tooltip of 4.1th Element",
            };
            child4.AddChild(child4_sub1);


            var child4_sub2 = new NavItem("another link", Url.Action("Index", "Home"))
            {
                Tooltip = "Tooltip of 4.2th Element",
                VisibilityStrategy = new NeverVisibleStrategy()
            };
            child4.AddChild(child4_sub2);


            root.AddChild(child4);

            var model = new NavBuilder().Build(callingContext, root);

            return PartialView("Nav.partial", model);
        }
    }

}
