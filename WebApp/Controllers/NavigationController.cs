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
            var root = new NavRoot("NAVIGATION ROOT (home)", Url.Action("Index", "Home"))
                              {
                                  Tooltip = "Home of the web site"
                              };

            // ============== STATIC NAV ITEMS ==========================
            var staticNavItemCategory = new NavItem("Static items", Url.Action("Index", "Home"))
                                 {
                                     Tooltip = "soem links to static items",
                                 };
            root.AddChild(staticNavItemCategory);

            var staticNavItemChild1 = new NavItem("Some Page", Url.Action("SomePage", "Home"))
                                  {
                                      Tooltip = "Some Page - should be active when browsing there !"
                                  };
            staticNavItemCategory.AddChild(staticNavItemChild1);

            var staticNavItemChild2 = new NavItem("Not a link")
            {
                Tooltip = "I don't have a link !"
            };
            staticNavItemCategory.AddChild(staticNavItemChild2);

            var staticNavItemChild2SubItem = new NavItem("Grand-son", Url.Action("Index", "Home"));
            staticNavItemChild2.AddChild(staticNavItemChild2SubItem);

            var staticNavItemChild2SubItemSubItem = new NavItem("Great Grand-son", "http://www.perdu.com");
            staticNavItemChild2SubItem.AddChild(staticNavItemChild2SubItemSubItem);


            // ================= Visibility ===============================

            var visibilityNavCategory = new NavItem("Visibility Features");
            root.AddChild(visibilityNavCategory);

            var visibitilyNavChild1 = new NavItem("Hidden Home link", Url.Action("Index", "Home"))
            {
                Tooltip = "Tooltip of element",
                VisibilityStrategy = new NeverVisibleStrategy()
            };
            visibilityNavCategory.AddChild(visibitilyNavChild1);

            var visibilityNavChild2NeverVisible = new NavItem("Not visible with children")
                                                      {
                                                          VisibilityStrategy = new NeverVisibleStrategy()
                                                      };
            visibilityNavCategory.AddChild(visibilityNavChild2NeverVisible);

            var childOfNeverVisibleItem = new NavItem("My father is invisible .. ");
            visibilityNavChild2NeverVisible.AddChild(childOfNeverVisibleItem);

            var childDisabledOfNeverVisibleItem = new NavItem("My father is invisible .. and I'm disabled")
                                                      {
                                                          EnablementStrategy = new AlwaysDisabledStrategy()
                                                      };
            visibilityNavChild2NeverVisible.AddChild(childDisabledOfNeverVisibleItem);

            var visibilityNavChild2Visible = new NavItem("Visible with hidden children");
            visibilityNavCategory.AddChild(visibilityNavChild2Visible);


            var child1InvisibleOfVisibleItem = new NavItem("I'm invisible")
                                                  {
                                                      VisibilityStrategy = new NeverVisibleStrategy()
                                                  };
            visibilityNavChild2Visible.AddChild(child1InvisibleOfVisibleItem);
            var child2InvisibleOfVisibleItem = new NavItem("I'm invisible too")
            {
                VisibilityStrategy = new NeverVisibleStrategy()
            };
            visibilityNavChild2Visible.AddChild(child2InvisibleOfVisibleItem);


            // Enabled / disabled
            var enablementNavCategory = new NavItem("Enablement Features");
            root.AddChild(enablementNavCategory);

            var enablementNavChild1 = new NavItem("Always disabled")
                                          {
                                              EnablementStrategy = new AlwaysDisabledStrategy()
                                          };
            enablementNavCategory.AddChild(enablementNavChild1);

            var enablementNavChild2 = new NavItem("I'am enabled");
            enablementNavCategory.AddChild(enablementNavChild2);

            var child1DisabledOfEnabledItem = new NavItem("I am disabled")
                                                  {
                                                      EnablementStrategy = new AlwaysDisabledStrategy()
                                                  };
            enablementNavChild2.AddChild(child1DisabledOfEnabledItem);

            var child2DisabledOfEnabledItem = new NavItem("I am disabled too")
            {
                EnablementStrategy = new AlwaysDisabledStrategy()
            };
            enablementNavChild2.AddChild(child2DisabledOfEnabledItem);

            // ================= MVC Nav Items =============================
            var mvcNavItemCategory = new NavItem("MvcNav items", Url.Action("Index", "Home"))
            {
                Tooltip = "using Mvc specific stuff",
            };
            root.AddChild(mvcNavItemCategory);


            var mvcNavItemChild1 = new MvcActionNavItem<HomeController>("Restricted Page", "Home", "SomeRestrictedPage")
            {
                Tooltip = "Some super secret Page (will disable if not allowed)"
            };
            mvcNavItemCategory.AddChild(mvcNavItemChild1);

            var mvcNavItemChild2 = new MvcActionNavItem<HomeController>("Other Restricted Page", "Home", "OtherRestrictedPage")
            {
                Tooltip = "Some super secret Page (will hide if not allowed)"
            };
            mvcNavItemCategory.AddChild(mvcNavItemChild2);



            var model = new NavBuilder().Build(callingContext, root);

            return PartialView("Nav.partial", model);
        }
    }

}
