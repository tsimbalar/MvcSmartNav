using System.Web.Mvc;
using MvcSmartNav;
using MvcSmartNav.Helpers;

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
                .WithToolTip("Home of the web site");
               
            // ============== STATIC NAV ITEMS ==========================
            root.AddChild(
                new NavStaticItem("Static items", Url.Action("Index", "Home"))
                    .WithToolTip("some links to static items")
                    .WithChild(
                        new NavStaticItem("Some Page", Url.Action("SomePage", "Home"))
                            .WithToolTip("Some Page - should be active when browsing there !"))
                    .WithChild(
                        new NavStaticItem("Not a link")
                            .WithToolTip("I don't have a link !")
                            .WithChild(
                                new NavStaticItem("Grand-son", Url.Action("Index", "Home"))
                                    .WithChild(
                                        new NavStaticItem("Great Grand-son", "http://www.perdu.com")
                                    )
                            )
                    )
            );


            // ================= Visibility ===============================
            root.AddChild(
                new NavStaticItem("Visibility Features")
                    .WithChild(
                        new NavStaticItem("Hidden Home link", Url.Action("Index", "Home"))
                            .WithToolTip("Tooltip of element")
                            .ShowNever()
                    )
                    .WithChild(
                        new NavStaticItem("Not visible with children")
                            .ShowNever()
                            .WithChild(
                                new NavStaticItem("My father is invisible .. ")
                            )
                            .WithChild(
                                new NavStaticItem("My father is invisible .. and I'm disabled")
                                .EnabledNever()
                            )
                    )
                    .WithChild(
                        new NavStaticItem("Visible with hidden children")
                            .WithChild(
                                new NavStaticItem("I'm invisible")
                                    .ShowNever()
                            )
                            .WithChild(
                                new NavStaticItem("I'm invisible too")
                                    .ShowNever()
                            )
                    )
            );

            // Enabled / disabled
            root.AddChild(
                new NavStaticItem("Enablement Features")
                    .WithChild(
                        new NavStaticItem("Always disabled")
                            .EnabledNever()
                    )
                    .WithChild(
                        new NavStaticItem("I'am enabled")
                            .WithChild(
                                 new NavStaticItem("I am disabled")
                                    .EnabledNever()
                            )
                            .WithChild(
                                new NavStaticItem("I am disabled too")
                                    .EnabledNever()
                            )
                    )
            );

            // ================= MVC Nav Items =============================
            root.AddChild(
                new NavStaticItem("MvcNav items", Url.Action("Index", "Home"))
                    .WithToolTip("using Mvc specific stuff")
                    .WithChild(
                        new MvcActionNavItem("Restricted Page", "Home", "SomeRestrictedPage")
                            .WithToolTip("Some super secret Page (will disable if not allowed)")
                            .DisabledWhenNotAuthorized())
                    .WithChild(
                        new MvcActionNavItem("Other Restricted Page", "Home", "OtherRestrictedPage")
                            .WithToolTip("Some super secret Page (will hide if not allowed)")
                            .HiddenWhenNotAuthorized()
                            .WithChild(
                                new MvcActionNavItem("Non restricted page", "Home", "SomePage")
                            )
                    )
            );


            var model = new NavBuilder().Build(callingContext, root);
            return PartialView("Nav.partial", model);
        }
    }
}
