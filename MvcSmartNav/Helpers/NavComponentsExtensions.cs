using System.Web.Mvc;
using MvcSmartNav.Enablement;
using MvcSmartNav.Visibility;

namespace MvcSmartNav.Helpers
{
    public static class NavComponentsExtensions
    {
        public static TNavComponent WithToolTip<TNavComponent>(this TNavComponent self, string tooltip) where TNavComponent : NavStaticComponentBase
        {
            self.Tooltip = tooltip;
            return self;
        }

        public static TNavComponent ShowNever<TNavComponent>(this TNavComponent self)
            where TNavComponent : NavStaticComponentBase
        {
            self.VisibilityStrategy = new NeverVisibleStrategy();
            return self;
        }

        public static TNavComponent EnabledNever<TNavComponent>(this TNavComponent self)
            where TNavComponent : NavStaticComponentBase
        {
            self.EnablementStrategy = new AlwaysDisabledStrategy();
            return self;
        }

        public static TNavComponent WithChild<TNavComponent>(this TNavComponent self, INavItem child )
            where TNavComponent : NavStaticComponentBase
        {
            self.AddChild(child);
            return self;
        }

        public static MvcActionNavItem<TController> WithToolTip<TController>(this MvcActionNavItem<TController> self, string tooltip)
            where TController : IController
        {
            self.Tooltip = tooltip;
            return self;
        }


        
    }
}
