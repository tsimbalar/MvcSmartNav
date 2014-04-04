using System;
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
                // ============== STATIC NAV ITEMS ==========================
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


            var model = new NavBuilder().Build(callingContext, nav);
            return PartialView("Nav.partial", model);
        }
    }

    public class Nav
    {
        public static INavRoot Root(string name, Func<NavRootConfigBuilder, INavRootConfiguration> configurationAction, params INavItem[] children)
        {
            var builder = new NavRootConfigBuilder();

            INavRootConfiguration config = configurationAction != null ? configurationAction(builder) : builder;

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

        public static INavItem Item(string name, Func<NavItemConfigBuilder, INavItemConfiguration> configurationAction, params INavItem[] children)
        {
            var builder = new NavItemConfigBuilder();

            INavItemConfiguration config = configurationAction != null ? configurationAction(builder) : builder;


            var node = (NavComponentBase)config.CreateNode(name);

            foreach (var navItem in children)
            {
                node.AddChild(navItem);
            }
            return (INavItem) node;
        }

        public static INavItem Item<TNavItem>(string name, Func<NavItemConfigBuilder, INavItemConfiguration<TNavItem>> configurationAction,
            params INavItem[] children) where TNavItem : INavItem
        {
            return Item(name, cfg => (INavItemConfiguration ) configurationAction(cfg), children);
        }

        public static INavItem Item(string name, params INavItem[] children)
        {
            return Item(name, null, children);
        }
    }

    public class NavItemConfigBuilder : INavItemConfiguration
    {
        public INavItem CreateNode(string name)
        {
            return new NavStaticItem(name);
        }

        public INavItemConfiguration<NavStaticItem> LinkTo(string url)
        {
            return new StaticNavConfiguration(url);
        } 
    }

    public class NavRootConfigBuilder : INavRootConfiguration
    {
        public INavRoot CreateRoot(string name)
        {
            return new NavStaticRoot(name);
        }

        public INavRootConfiguration<NavStaticRoot> LinkTo(string url)
        {
            return new StaticNavConfiguration(url);
        } 
    }

    public interface INavItemConfiguration<out TItemType> where TItemType : INavItem
    {
         TItemType CreateNode(string name);
    }

    public interface INavRootConfiguration<out TItemType> where TItemType : INavRoot
    {
        TItemType CreateRoot(string name);
    }

    public interface INavRootConfiguration
    {
        INavRoot CreateRoot(string name);
    }

    public interface INavItemConfiguration
    {
        INavItem CreateNode(string name);
    }

    

    

    public abstract class NavConfiguration<TItemType, TRootType> : INavItemConfiguration, INavRootConfiguration, INavRootConfiguration<TRootType>, INavItemConfiguration<TItemType> where TItemType : INavItem where TRootType: INavRoot
    {
        
        INavItem INavItemConfiguration.CreateNode(string name)
        {
            return CreateNode(name);
        }

        INavRoot INavRootConfiguration.CreateRoot(string name)
        {
            return CreateRoot(name);
        }

        public abstract TRootType CreateRoot(string name);

        public abstract TItemType CreateNode(string name);
    }

    public class NavRootConfiguration<TNavRoot> : NavConfiguration<INavItem, TNavRoot> where TNavRoot : INavRoot
    {
        private readonly INavRootConfiguration<TNavRoot> _wrappedRootConfig;

        public NavRootConfiguration([NotNull] INavRootConfiguration<TNavRoot> wrappedRootConfig )
        {
            if (wrappedRootConfig == null) throw new ArgumentNullException("wrappedRootConfig");
            _wrappedRootConfig = wrappedRootConfig;
        }

        public override TNavRoot CreateRoot(string name)
        {
            return _wrappedRootConfig.CreateRoot(name);
        }

        public override INavItem CreateNode(string name)
        {
            throw new NotImplementedException();
        }
    }

    public class NavItemConfiguration<TNavItem> : NavConfiguration<TNavItem, INavRoot> where TNavItem : INavItem
    {
        private readonly INavItemConfiguration<TNavItem> _wrappedItemConfig;

        public NavItemConfiguration([NotNull] INavItemConfiguration<TNavItem> wrappedItemConfig)
        {
            if (_wrappedItemConfig == null) throw new ArgumentNullException("wrappedItemConfig");
            _wrappedItemConfig = wrappedItemConfig;
        }

        public override INavRoot CreateRoot(string name)
        {
            throw new NotImplementedException();
        }

        public override TNavItem CreateNode(string name)
        {
            return _wrappedItemConfig.CreateNode(name);
        }
    }

    public static class NavConfigurationExtensions
    {
        public static NavConfiguration<NavStaticItem, NavStaticRoot> LinkTo(this INavItemConfiguration self, string url)
        {
            return new StaticNavConfiguration(url);
        }

        public static NavConfiguration<NavStaticItem, NavStaticRoot> LinkTo(this INavRootConfiguration self, string url)
        {
            return new StaticNavConfiguration(url);
        }

        public static INavRootConfiguration WithToolTip<TNavRoot>(this INavRootConfiguration<TNavRoot> self, string tooltip) 
            where TNavRoot : INavRoot

        {
            return new ConfigWithTooltip<INavItem, TNavRoot>(new NavRootConfiguration<TNavRoot>(self), tooltip);
        }

        public static INavItemConfiguration<TNavItem> WithToolTip<TNavItem>(this INavItemConfiguration<TNavItem> self, string tooltip)
            where TNavItem : INavItem
        {
            return new ConfigWithTooltip<TNavItem, INavRoot>(new NavItemConfiguration<TNavItem>(self), tooltip);
        }

        public static IPickItemActivationStrategy<TNavItem> ActiveWhen<TNavItem>(
            this INavItemConfiguration<TNavItem> self) where TNavItem : INavItem
        {
            return  new PickActivationStrategy<TNavItem, INavRoot>(new NavItemConfiguration<TNavItem>(self));
        }

        public static INavItemConfiguration<TNavItem> MatchUrlPathAndQueryString<TNavItem>(
            this IPickItemActivationStrategy<TNavItem> self) 
            where TNavItem : INavItem
        {
            return self.WithStrategy(new ExactUrlPathAndQueryStringActivationStrategy<TNavItem>());
        }

    }

    public interface IPickItemActivationStrategy<TNavItem>
        where TNavItem : INavItem
    {
        INavItemConfiguration<TNavItem> WithStrategy(INavItemActivationStrategy<TNavItem> activationStrategy);
    }

    class PickActivationStrategy<TNavItem, TNavRoot> : IPickItemActivationStrategy<TNavItem> where TNavItem : INavItem where TNavRoot : INavRoot
    {
        private readonly NavConfiguration<TNavItem, TNavRoot> _config;

        public PickActivationStrategy([NotNull] NavConfiguration<TNavItem, TNavRoot> config)
        {
            if (config == null) throw new ArgumentNullException("config");
            _config = config;
        }

        public INavItemConfiguration<TNavItem> WithStrategy(INavItemActivationStrategy<TNavItem> activationStrategy)
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
