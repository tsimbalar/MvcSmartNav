using System;
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
        public PartialViewResult GenerateNav(ViewContext callingContext)
        {
            var root = new NavStaticRoot("NAVIGATION ROOT (home)", Url.Action("Index", "Home"))
                .WithToolTip("Home of the web site");

            // ============== STATIC NAV ITEMS ==========================
            root.AddChild(
                new NavStaticItem("Static items", Url.Action("Index", "Home"))
                    .WithToolTip("some links to static items")
                    .WithStaticChild("Some Page", Url.Action("SomePage", "Home"), c =>
                        c.WithToolTip("Some Page - should be active when browsing there !"))
                    .WithStaticChild("Not a link", "", c =>
                        c.WithToolTip("I don't have a link !")
                        .WithStaticChild("Grand-son", Url.Action("Index", "Home"), gc =>
                            gc.WithStaticChild("Great Grand-son", "http://www.perdu.com")
                        )
                    )
            );


            // ================= Visibility ===============================
            root.AddChild(
                new NavStaticItem("Visibility Features")
                    .WithStaticChild("Hidden Home link", Url.Action("Index", "Home"), c =>
                        c.WithToolTip("Tooltip of element")
                        .ShowNever()
                    )
                    .WithStaticChild("Not visible with children", "", c =>
                        c.ShowNever()
                        .WithStaticChild("My father is invisible .. ")
                        .WithStaticChild("My father is invisible .. and I'm disabled", "", gc =>
                            gc.EnabledNever()
                       )
                    )
                    .WithStaticChild("Visible with hidden children", "", c =>
                        c.WithStaticChild("I'm invisible", "", gc =>
                            gc.ShowNever()
                        )
                        .WithStaticChild("I'm invisible too", "", gc =>
                            gc.ShowNever()
                        )
                    )
            );

            // Enabled / disabled
            root.AddChild(
                new NavStaticItem("Enablement Features")
                    .WithStaticChild("Always disabled", "", c=> 
                        c.EnabledNever()
                    )
                    .WithStaticChild("I'am enabled", "", c=> 
                        c.WithStaticChild("I am disabled", "", gc => 
                            gc.EnabledNever()
                        )
                        .WithStaticChild("I am disabled too", "", gc=> 
                            gc.EnabledNever()
                        )
                    )
            );

            // ================= MVC Nav Items =============================
            root.AddChild(
                new NavStaticItem("MvcNav items", Url.Action("Index", "Home"))
                    .WithToolTip("using Mvc specific stuff")
                    .WithMvcChild<NavStaticItem, StaticUrlSpecification>("Restricted Page", "Home", "SomeRestrictedPage", null, c=> 
                            c.WithToolTip("Some super secret Page (will disable if not allowed)")
                            .DisabledWhenNotAuthorized())
                    .WithMvcChild<NavStaticItem, StaticUrlSpecification>("Other Restricted Page", "Home", "OtherRestrictedPage", null, c => 
                            c.WithToolTip("Some super secret Page (will hide if not allowed)")
                            .HiddenWhenNotAuthorized()
                            .WithMvcChild<MvcActionNavItem, MvcActionUrlSpecification>("Non restricted page", "Home", "SomePage")
                            
                    )
            );


            var model = new NavBuilder().Build(callingContext, root);
            return PartialView("Nav.partial", model);
        }
    }
}
