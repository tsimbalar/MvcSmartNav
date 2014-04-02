﻿using System;
using System.Web.Mvc;
using JetBrains.Annotations;
using MvcSmartNav;
using MvcSmartNav.Activation;
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
            var nav = Nav.Root("NAVIGATION ROOT (home)", cfg => cfg.LinkTo(Url.Action("Index", "Home"))
                                                                    .WithToolTip("Home of the web site"),
                Nav.Item("Static Items", cfg=> cfg.LinkTo(Url.Action("Index", "Home"))
                                                    .WithToolTip("some links to static items"),
                    Nav.Item("Some Page", cfg => cfg.LinkTo(Url.Action("SomePage", "Home"))
                                                    .WithToolTip("Some Page - should be active when browsing there !")),
                    Nav.Item("Not a link", cfg => cfg.LinkTo("")
                                                        .WithToolTip("I don't have a link"),
                        Nav.Item("Grand-son", cfg => cfg.LinkTo(Url.Action("Index", "Home", new { queryStringParam = "toto" }))
                                                        .WithToolTip("with a query string"),
                            Nav.Item("Great grandson", cfg => cfg.LinkTo("http://www.perdu.com"))
                        ),
                        Nav.Item("Grand-son 2 ... activation based on exact url and qs", cfg=> cfg.LinkTo(Url.Action("Index", "Home", new {queryString = "foo"}))
                                                                                                    .WithToolTip("with a query string")
                                                                                                    .ActiveWhen().MatchUrlPathAndQueryString())
                    )

                )
            );

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
                        .WithStaticChild("Grand-son", Url.Action("Index", "Home", new {queryStringParam="toto"}), gc =>
                            gc
                            .WithToolTip("with a query string")
                            .WithStaticChild("Great Grand-son", "http://www.perdu.com")
                        )
                        .WithStaticChild("Grand-son 2 ... activation based on exact url and qs", Url.Action("Index", "Home", new {queryString = "foo"}), gc => 
                            gc
                            .WithToolTip("with a query string")
                            .ActivatedForExactUrlAndQuerystring()
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
                    .WithMvcChild("Restricted Page", "Home", "SomeRestrictedPage", null, c=> 
                            c.WithToolTip("Some super secret Page (will disable if not allowed)")
                            .DisabledWhenNotAuthorized())
                    .WithMvcChild("Other Restricted Page", "Home", "OtherRestrictedPage", null, c => 
                            c.WithToolTip("Some super secret Page (will hide if not allowed)")
                            .HiddenWhenNotAuthorized()
                            .WithMvcChild("Non restricted page", "Home", "SomePage")
                            
                    )
            );


            var model = new NavBuilder().Build(callingContext, root);
            return PartialView("Nav.partial", model);
        }
    }

    public class Nav
    {
        public static INavRoot Root(string name, Func<INavConfiguration, INavConfiguration> configurationAction, params INavItem[] children)
        {
            INavConfiguration config = new StaticNavConfiguration("");
            if (configurationAction != null)
            {
                config = configurationAction(config);
            }

            var root = (NavComponentBase)config.CreateRoot(name);

            foreach (var navItem in children)
            {
                root.AddChild(navItem);
            }
            return (INavRoot) root;
        }

        public static INavRoot Root(string name, params INavItem[] children)
        {
            return Root(name, null, children);
        }

        public static INavItem Item(string name, Func<INavConfiguration, INavConfiguration> configurationAction, params INavItem[] children)
        {
            INavConfiguration config = new StaticNavConfiguration("");
            if (configurationAction != null)
            {
                config = configurationAction(config);
            }

            var root = (NavComponentBase)config.CreateNode(name);

            foreach (var navItem in children)
            {
                root.AddChild(navItem);
            }
            return (INavItem) root;
        }

        public static INavItem Item(string name, params INavItem[] children)
        {
            return Item(name, null, children);
        }
    }

    public interface INavConfiguration
    {
         INavItem CreateNode(string name);

         INavRoot CreateRoot(string name);
    }

    public abstract class NavConfiguration<TItemType, TRootType> : INavConfiguration where TItemType : INavItem where TRootType: INavRoot
    {
        INavItem INavConfiguration.CreateNode(string name)
        {
            return CreateNode(name);
        }

        INavRoot INavConfiguration.CreateRoot(string name)
        {
            return CreateRoot(name);
        }

        public abstract TItemType CreateNode(string name);

        public abstract TRootType CreateRoot(string name);
    }

    public static class NavConfigurationExtensions
    {
        public static NavConfiguration<NavStaticItem, NavStaticRoot> LinkTo(this INavConfiguration self, string url)
        {
            return new StaticNavConfiguration(url);
        }

        public static NavConfiguration<TNavItem, TNavRoot> WithToolTip<TNavItem, TNavRoot>(
            this NavConfiguration<TNavItem, TNavRoot> self, string tooltip)
            where TNavItem : INavItem where TNavRoot : INavRoot
        {
            return new ConfigWithTooltip<TNavItem, TNavRoot>(self, tooltip);
        }

        public static IPickActivationStrategy<TNavItem, TNavRoot> ActiveWhen<TNavItem, TNavRoot>(
            this NavConfiguration<TNavItem, TNavRoot> self) where TNavItem : INavItem where TNavRoot : INavRoot
        {
            return  new PickActivationStrategy<TNavItem, TNavRoot>(self);
        }

        public static NavConfiguration<TNavItem, TNavRoot> MatchUrlPathAndQueryString<TNavItem,TNavRoot>(
            this IPickActivationStrategy<TNavItem, TNavRoot> self) 
            where TNavRoot : INavRoot where TNavItem : INavItem
        {
            return self.WithStrategy(new ExactUrlPathAndQueryStringActivationStrategy<TNavItem>());
        }

    }

    public interface IPickActivationStrategy<TNavItem, TNavRoot>
        where TNavItem : INavItem
        where TNavRoot : INavRoot
    {
        NavConfiguration<TNavItem, TNavRoot> WithStrategy(INavItemActivationStrategy<TNavItem> activationStrategy);

    }

    class PickActivationStrategy<TNavItem, TNavRoot> : IPickActivationStrategy<TNavItem, TNavRoot> where TNavItem : INavItem where TNavRoot : INavRoot
    {
        private readonly NavConfiguration<TNavItem, TNavRoot> _config;

        public PickActivationStrategy([NotNull] NavConfiguration<TNavItem, TNavRoot> config)
        {
            if (config == null) throw new ArgumentNullException("config");
            _config = config;
        }

        public NavConfiguration<TNavItem, TNavRoot> WithStrategy(INavItemActivationStrategy<TNavItem> activationStrategy)
        {
            return  new ConfigWithActivationStrategy<TNavItem, TNavRoot>(_config, activationStrategy);
        }
    }

    internal class ConfigWithActivationStrategy<TNavItem, TNavRoot> : NavConfiguration<TNavItem, TNavRoot> 
        where TNavRoot : INavRoot where TNavItem : INavItem
    {
        private readonly NavConfiguration<TNavItem, TNavRoot> _wrappedConfig;
        private readonly INavItemActivationStrategy<TNavItem> _activationStrategy;

        public ConfigWithActivationStrategy([NotNull] NavConfiguration<TNavItem, TNavRoot> wrappedConfig,
            [NotNull] INavItemActivationStrategy<TNavItem> activationStrategy)
        {
            if (wrappedConfig == null) throw new ArgumentNullException("wrappedConfig");
            if (activationStrategy == null) throw new ArgumentNullException("activationStrategy");
            _wrappedConfig = wrappedConfig;
            _activationStrategy = activationStrategy;
        }

        public override TNavItem CreateNode(string name)
        {
            var node =  _wrappedConfig.CreateNode(name);
            //TODO : assign strategy !
            //node.ActivationStrategy = _activationStrategy;
            return node;
        }

        public override TNavRoot CreateRoot(string name)
        {
            var node = _wrappedConfig.CreateRoot(name);
            //TODO : assign strategy !
            //node.ActivationStrategy = _activationStrategy;
            return node;
        }
    }

    public class ConfigWithTooltip<TNavItem, TNavRoot> : NavConfiguration<TNavItem, TNavRoot> 
        where TNavItem : INavItem where TNavRoot : INavRoot
    {
        private readonly NavConfiguration<TNavItem, TNavRoot> _wrappedConfig;
        private readonly string _tooltip;

        public ConfigWithTooltip([NotNull] NavConfiguration<TNavItem, TNavRoot> wrappedConfig, [NotNull] string tooltip)
        {
            if (wrappedConfig == null) throw new ArgumentNullException("wrappedConfig");
            if (tooltip == null) throw new ArgumentNullException("tooltip");
            _wrappedConfig = wrappedConfig;
            _tooltip = tooltip;
        }

        public override TNavItem CreateNode(string name)
        {
            var node = _wrappedConfig.CreateNode(name);
            node.Tooltip = _tooltip;
            return node;
        }

        public override TNavRoot CreateRoot(string name)
        {
            var node = _wrappedConfig.CreateRoot(name);
            node.Tooltip = _tooltip;
            return node;
        }
    }

    public class StaticNavConfiguration : NavConfiguration<NavStaticItem, NavStaticRoot>
    {
        private readonly string _url;

        public StaticNavConfiguration([NotNull] string url)
        {
            if (url == null) throw new ArgumentNullException("url");
            _url = url;
        }

        public override NavStaticItem CreateNode(string name)
        {
            return new NavStaticItem(name, _url);
        }

        public override NavStaticRoot CreateRoot(string name)
        {
            return  new NavStaticRoot(name, _url);
        }
    }
}
