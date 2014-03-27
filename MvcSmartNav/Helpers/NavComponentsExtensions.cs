using System;
using JetBrains.Annotations;
using MvcSmartNav.Enablement;
using MvcSmartNav.Visibility;

namespace MvcSmartNav.Helpers
{
    public static class NavComponentsExtensions
    {
        #region ToolTip

        public static NavStaticItem WithToolTip(this NavStaticItem self, string tooltip)
        {
            self.Tooltip = tooltip;
            return self;
        }

        public static NavStaticRoot WithToolTip(this NavStaticRoot self, string tooltip)
        {
            self.Tooltip = tooltip;
            return self;
        }

        public static MvcActionNavItem WithToolTip(this MvcActionNavItem self, string tooltip)
        {
            self.Tooltip = tooltip;
            return self;
        }

        #endregion


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


        public static NavStaticItem WithStaticChild(this NavStaticItem self, string name, string url = "",
            Func<NavStaticItem, NavStaticItem> configuration = null)
        {
            var child = new NavStaticItem(name, url);
            if (configuration != null)
            {
                child = configuration(child);
            }
            self.AddChild(child);
            return self;
        }

        public static MvcActionNavItem WithStaticChild(this MvcActionNavItem self, string name, string url = "",
            Func<NavStaticItem, NavStaticItem> configuration = null)
        {
            var child = new NavStaticItem(name, url);
            if (configuration != null)
            {
                child = configuration(child);
            }
            self.AddChild(child);
            return self;
        }


        public static NavStaticItem WithMvcChild(this NavStaticItem self, string name, [AspMvcController] string controllerName, [AspMvcAction] string actionName, object routeValues = null,
             Func<MvcActionNavItem, MvcActionNavItem> configuration = null)
        {
            var child = new MvcActionNavItem(name, controllerName, actionName, routeValues);
            if (configuration != null)
            {
                child = configuration(child);
            }
            self.AddChild(child);
            return self;
        }

        public static MvcActionNavItem WithMvcChild(this MvcActionNavItem self, string name, [AspMvcController] string controllerName, [AspMvcAction] string actionName, object routeValues = null,
            Func<MvcActionNavItem, MvcActionNavItem> configuration = null)
        {
            var child = new MvcActionNavItem(name, controllerName, actionName, routeValues);
            if (configuration != null)
            {
                child = configuration(child);
            }
            self.AddChild(child);
            return self;
        }



        public static TMvcNavComponent DisabledWhenNotAuthorized<TMvcNavComponent>(this TMvcNavComponent self)
            where TMvcNavComponent : MvcActionNavComponentBase
        {
            self.EnablementStrategy = new AuthorizeAttributeEnabledStrategy();
            return self;
        }

        public static TMvcNavComponent HiddenWhenNotAuthorized<TMvcNavComponent>(this TMvcNavComponent self)
            where TMvcNavComponent : MvcActionNavComponentBase
        {
            self.VisibilityStrategy = new AuthorizeAttributeVisibleStrategy();
            return self;
        }

    }
}
