using System;
using MvcSmartNav.Enablement;
using MvcSmartNav.Visibility;

namespace MvcSmartNav.Helpers
{
    public static class NavComponentsExtensions
    {
        #region ToolTip

        public static TNavComponent WithToolTip<TNavComponent>(this TNavComponent self, string tooltip) where TNavComponent : NavStaticComponentBase
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

        public static TNavComponent WithChild<TNavComponent, TUrl>(this TNavComponent self, INavItem child )
            where TNavComponent : NavComponentBase<TUrl> 
            where TUrl : ITargetUrlSpecification
        {
            self.AddChild(child);
            return self;
        }

        

        public static TNavComponent WithStaticChild<TNavComponent>(this TNavComponent self, string name, string url = "",
            Func<NavStaticItem, NavStaticItem> configuration = null)
            where TNavComponent : NavStaticComponentBase
        {
            var child = new NavStaticItem(name, url);
            if (configuration != null)
            {
                child = configuration(child);
            }
            self.AddChild(child);
            return self;
        }



        public static TNavComponent WithMvcChild<TNavComponent, TUrl>(this TNavComponent self, string name, string controllerName, string actionName, object routeValues = null, 
            Func<MvcActionNavItem, MvcActionNavItem> configuration = null)
            where TNavComponent : NavComponentBase<TUrl> where TUrl : ITargetUrlSpecification
        {
            var child = new MvcActionNavItem(name, controllerName, actionName, routeValues);
            if (configuration != null)
            {
                child = configuration(child);
            }
            self.AddChild(child);
            return self;
        }



        //public static MvcActionNavItem WithChild(this MvcActionNavItem self, INavItem child)
        //{
        //    self.AddChild(child);
        //    return self;
        //}


        public static MvcActionNavItem DisabledWhenNotAuthorized(this MvcActionNavItem self)
        {
            self.EnablementStrategy = new AuthorizeAttributeEnabledStrategy();
            return self;
        }

        public static MvcActionNavItem HiddenWhenNotAuthorized(this MvcActionNavItem self)
        {
            self.VisibilityStrategy = new AuthorizeAttributeVisibleStrategy();
            return self;
        }
        
    }
}
