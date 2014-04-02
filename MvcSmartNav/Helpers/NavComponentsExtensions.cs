using System;
using JetBrains.Annotations;
using MvcSmartNav.Activation;
using MvcSmartNav.Enablement;
using MvcSmartNav.Visibility;

namespace MvcSmartNav.Helpers
{
    public static class NavComponentsExtensions
    {
        #region ToolTip

        public static TNavComponent WithToolTip<TNavComponent>(this TNavComponent self, string tooltip)
            where TNavComponent : NavComponentBase
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

        public static TNavComponent ActivatedForExactUrlAndQuerystring<TNavComponent>(this TNavComponent self)
            where TNavComponent : NavStaticComponentBase
        {
            self.ActivationStrategy = new ExactUrlPathAndQueryStringActivationStrategy<NavStaticComponentBase>();
            return self;
        }


        #region With*Child

        public static TNavComponent WithStaticChild<TNavComponent>(this TNavComponent self, string name, string url = "",
            Func<NavStaticItem, NavStaticItem> configuration = null)
            where TNavComponent:NavComponentBase
        {
            var child = new NavStaticItem(name, url);
            if (configuration != null)
            {
                child = configuration(child);
            }
            self.AddChild(child);
            return self;
        }


        public static TNavComponent WithMvcChild<TNavComponent>(this TNavComponent self, string name, [AspMvcController] string controllerName, [AspMvcAction] string actionName, object routeValues = null,
             Func<MvcActionNavItem, MvcActionNavItem> configuration = null)
            where TNavComponent: NavComponentBase
        {
            var child = new MvcActionNavItem(name, controllerName, actionName, routeValues);
            if (configuration != null)
            {
                child = configuration(child);
            }
            self.AddChild(child);
            return self;
        }

        #endregion

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
