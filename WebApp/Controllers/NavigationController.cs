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
            var root = new NavStaticRoot("NAVIGATION ROOT (home)", Url.Action("Index", "Home"))
                              {
                                  Tooltip = "Home of the web site"
                              };

            // ============== STATIC NAV ITEMS ==========================
            var staticNavItemCategory = new NavStaticItem("Static items", Url.Action("Index", "Home"))
                                 {
                                     Tooltip = "some links to static items",
                                 };
            root.AddChild(staticNavItemCategory);

            var staticNavItemChild1 = new NavStaticItem("Some Page", Url.Action("SomePage", "Home"))
                                  {
                                      Tooltip = "Some Page - should be active when browsing there !"
                                  };
            staticNavItemCategory.AddChild(staticNavItemChild1);

            var staticNavItemChild2 = new NavStaticItem("Not a link")
            {
                Tooltip = "I don't have a link !"
            };
            staticNavItemCategory.AddChild(staticNavItemChild2);

            var staticNavItemChild2SubItem = new NavStaticItem("Grand-son", Url.Action("Index", "Home"));
            staticNavItemChild2.AddChild(staticNavItemChild2SubItem);

            var staticNavItemChild2SubItemSubItem = new NavStaticItem("Great Grand-son", "http://www.perdu.com");
            staticNavItemChild2SubItem.AddChild(staticNavItemChild2SubItemSubItem);


            // ================= Visibility ===============================

            var visibilityNavCategory = new NavStaticItem("Visibility Features");
            root.AddChild(visibilityNavCategory);

            var visibitilyNavChild1 = new NavStaticItem("Hidden Home link", Url.Action("Index", "Home"))
            {
                Tooltip = "Tooltip of element",
                VisibilityStrategy = new NeverVisibleStrategy()
            };
            visibilityNavCategory.AddChild(visibitilyNavChild1);

            var visibilityNavChild2NeverVisible = new NavStaticItem("Not visible with children")
                                                      {
                                                          VisibilityStrategy = new NeverVisibleStrategy()
                                                      };
            visibilityNavCategory.AddChild(visibilityNavChild2NeverVisible);

            var childOfNeverVisibleItem = new NavStaticItem("My father is invisible .. ");
            visibilityNavChild2NeverVisible.AddChild(childOfNeverVisibleItem);

            var childDisabledOfNeverVisibleItem = new NavStaticItem("My father is invisible .. and I'm disabled")
                                                      {
                                                          EnablementStrategy = new AlwaysDisabledStrategy()
                                                      };
            visibilityNavChild2NeverVisible.AddChild(childDisabledOfNeverVisibleItem);

            var visibilityNavChild2Visible = new NavStaticItem("Visible with hidden children");
            visibilityNavCategory.AddChild(visibilityNavChild2Visible);


            var child1InvisibleOfVisibleItem = new NavStaticItem("I'm invisible")
                                                  {
                                                      VisibilityStrategy = new NeverVisibleStrategy()
                                                  };
            visibilityNavChild2Visible.AddChild(child1InvisibleOfVisibleItem);
            var child2InvisibleOfVisibleItem = new NavStaticItem("I'm invisible too")
            {
                VisibilityStrategy = new NeverVisibleStrategy()
            };
            visibilityNavChild2Visible.AddChild(child2InvisibleOfVisibleItem);


            // Enabled / disabled
            var enablementNavCategory = new NavStaticItem("Enablement Features");
            root.AddChild(enablementNavCategory);

            var enablementNavChild1 = new NavStaticItem("Always disabled")
                                          {
                                              EnablementStrategy = new AlwaysDisabledStrategy()
                                          };
            enablementNavCategory.AddChild(enablementNavChild1);

            var enablementNavChild2 = new NavStaticItem("I'am enabled");
            enablementNavCategory.AddChild(enablementNavChild2);

            var child1DisabledOfEnabledItem = new NavStaticItem("I am disabled")
                                                  {
                                                      EnablementStrategy = new AlwaysDisabledStrategy()
                                                  };
            enablementNavChild2.AddChild(child1DisabledOfEnabledItem);

            var child2DisabledOfEnabledItem = new NavStaticItem("I am disabled too")
            {
                EnablementStrategy = new AlwaysDisabledStrategy()
            };
            enablementNavChild2.AddChild(child2DisabledOfEnabledItem);

            // ================= MVC Nav Items =============================
            var mvcNavItemCategory = new NavStaticItem("MvcNav items", Url.Action("Index", "Home"))
            {
                Tooltip = "using Mvc specific stuff",
            };
            root.AddChild(mvcNavItemCategory);


            var mvcNavItemChild1 = new MvcActionNavItem<HomeController>("Restricted Page", "SomeRestrictedPage")
            {
                Tooltip = "Some super secret Page (will disable if not allowed)"
            };
            mvcNavItemCategory.AddChild(mvcNavItemChild1);

            var mvcNavItemChild2 = new MvcActionNavItem<HomeController>("Other Restricted Page", "OtherRestrictedPage")
            {
                Tooltip = "Some super secret Page (will hide if not allowed)"
            };
            mvcNavItemCategory.AddChild(mvcNavItemChild2);



            var model = new NavBuilder().Build(callingContext, root);

            return PartialView("Nav.partial", model);
        }
    }

}
