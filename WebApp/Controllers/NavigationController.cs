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
                                                          EnabilityStrategy = new AlwaysDisabledStrategy()
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



            //// =================== Other Visibility ==================

            //var child2 = new NavItem("Hidden Home link", Url.Action("Index", "Home"))
            //                      {
            //                          Tooltip = "Tooltip of secondElement",
            //                          VisibilityStrategy = new NeverVisibleStrategy()
            //                      };
            //root.AddChild(child2);

            //var child2_sub1 = new NavItem("subnav", Url.Action("Contact", "Home"))
            //                    {
            //                        Tooltip = "Tooltip of subnav",
            //                        VisibilityStrategy = new NeverVisibleStrategy()
            //                    };
            //child2.AddChild(child2_sub1);

            //var child2_sub2 = new NavItem("subnav", Url.Action("Contact", "Home"))
            //{
            //    Tooltip = "Tooltip of subnav",
            //    EnabilityStrategy = new AlwaysDisabledStrategy()
            //};

            //var child2sub2_sub1 = new NavItem("subsub", Url.Action("About", "Home"))
            //                          {
            //                              Tooltip = "rien",
            //                          };
            //child2_sub2.AddChild(child2sub2_sub1);

            //child2.AddChild(child2_sub2);

            //var child3 = new NavItem("About link", Url.Action("About", "Home"))
            //{
            //    Tooltip = "Tooltip of 3rd Element",
            //};
            //root.AddChild(child3);


            //var child4 = new NavItem("random link", "http://www.google.fr")
            //{
            //    Tooltip = "Tooltip of 4th Element",
            //    EnabilityStrategy = new AlwaysDisabledStrategy()
            //};

            //var child4_sub1 = new NavItem("Some link", "http://www.yahoo.com")
            //{
            //    Tooltip = "Tooltip of 4.1th Element",
            //};
            //child4.AddChild(child4_sub1);


            //var child4_sub2 = new NavItem("another link", Url.Action("Index", "Home"))
            //{
            //    Tooltip = "Tooltip of 4.2th Element",
            //    VisibilityStrategy = new NeverVisibleStrategy()
            //};
            //child4.AddChild(child4_sub2);


            //root.AddChild(child4);

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
