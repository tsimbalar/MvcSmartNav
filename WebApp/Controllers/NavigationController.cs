using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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
            var navTree = new NavRoot();

            var child1 = new NavItem
                                 {
                                     Name = "Home link",
                                     Tooltip = "Tooltip of firstElement",
                                     TargetUrl = Url.Action("Index", "Home"),
                                     ActivationStrategy = new ExactUrlActivationStrategy(),
                                     VisibilityStrategy = new AlwaysVisibleStrategy(),
                                     EnabilityStrategy = new AlwaysEnabledStrategy()
                                 };
            navTree.AddChild(child1);

            var child2 = new NavItem
                                  {
                                      Name = "hidden Home link",
                                      Tooltip = "Tooltip of secondElement",
                                      TargetUrl = Url.Action("Index", "Home"),
                                      ActivationStrategy = new ExactUrlActivationStrategy(),
                                      VisibilityStrategy = new NeverVisibleStrategy(),
                                      EnabilityStrategy = new AlwaysEnabledStrategy()
                                  };
            navTree.AddChild(child2);

            var child2_sub1 = new NavItem
                                       {
                                           Name = "subnav",
                                           Tooltip = "Tooltip of subnav",
                                           TargetUrl = Url.Action("Contact", "Home"),
                                           ActivationStrategy = new ExactUrlActivationStrategy(),
                                           VisibilityStrategy = new NeverVisibleStrategy(),
                                           EnabilityStrategy = new AlwaysEnabledStrategy()
                                       };
            child2.AddChild(child2_sub1);

            var child2_sub2 = new NavItem
            {
                Name = "subnav",
                Tooltip = "Tooltip of subnav",
                TargetUrl = Url.Action("Contact", "Home"),
                ActivationStrategy = new ExactUrlActivationStrategy(),
                VisibilityStrategy = new AlwaysVisibleStrategy(),
                EnabilityStrategy = new AlwaysDisabledStrategy()
            };
            child2.AddChild(child2_sub2);

            var child3 = new NavItem
            {
                Name = "About link",
                Tooltip = "Tooltip of 3rd Element",
                TargetUrl = Url.Action("About", "Home"),
                ActivationStrategy = new ExactUrlActivationStrategy(),
                VisibilityStrategy = new AlwaysVisibleStrategy(),
                EnabilityStrategy = new AlwaysEnabledStrategy()
            };
            navTree.AddChild(child3);


            var child4 = new NavItem
            {
                Name = "random link",
                Tooltip = "Tooltip of 4th Element",
                TargetUrl = "http://www.google.fr",
                ActivationStrategy = new ExactUrlActivationStrategy(),
                VisibilityStrategy = new AlwaysVisibleStrategy(),
                EnabilityStrategy = new AlwaysDisabledStrategy()
            };

            var child4_sub1 = new NavItem
            {
                Name = "some link",
                Tooltip = "Tooltip of 4.1th Element",
                TargetUrl = "http://www.yahoo.com",
                ActivationStrategy = new ExactUrlActivationStrategy(),
                VisibilityStrategy = new AlwaysVisibleStrategy(),
                EnabilityStrategy = new AlwaysEnabledStrategy()
            };
            child4.AddChild(child4_sub1);


            var child4_sub2 = new NavItem
            {
                Name = "another link",
                Tooltip = "Tooltip of 4.2th Element",
                TargetUrl = Url.Action("Index", "Home"),
                ActivationStrategy = new ExactUrlActivationStrategy(),
                VisibilityStrategy = new NeverVisibleStrategy(),
                EnabilityStrategy = new AlwaysEnabledStrategy()
            };
            child4.AddChild(child4_sub2);


            navTree.AddChild(child4);

            var model = new NavigationModelBuilder().Build(callingContext, navTree);

            return PartialView("Nav.partial", model);
        }
    }

    public class AlwaysDisabledStrategy : INavItemEnabledStrategy
    {
        public NodeEnablement EvaluateEnablement(NavItem navItem, ViewContext context)
        {
            return new NodeEnablement(disabled: true, reason: "always disabled");
        }
    }

    public class NeverVisibleStrategy : INavItemVisibilityStrategy
    {
        public NodeVisibility EvaluateVisibility(NavItem navItem, ViewContext context)
        {
            return new NodeVisibility(false, "Never visible");
        }
    }

    public class AlwaysEnabledStrategy : INavItemEnabledStrategy
    {
        public NodeEnablement EvaluateEnablement(NavItem navItem, ViewContext context)
        {
            return new NodeEnablement(false, "Always enabled");
        }
    }

    public class AlwaysVisibleStrategy : INavItemVisibilityStrategy
    {
        public NodeVisibility EvaluateVisibility(NavItem navItem, ViewContext context)
        {
            return new NodeVisibility(true, "Always visible");
        }
    }

    public class ExactUrlActivationStrategy : INavItemActivationStrategy
    {
        public NodeActivation EvaluateActivation(NavItem navItem, ViewContext context)
        {
            var currentRelativeUrl = context.RequestContext.HttpContext.Request.Url.PathAndQuery;
            var targetUrl = navItem.TargetUrl;
            var isActive = currentRelativeUrl == targetUrl;
            string reason = "current Url (" + currentRelativeUrl + ") " + (isActive ? "matches" : "does not match") +
                            " targetUrl (" + targetUrl + ")";
            return new NodeActivation(isActive, reason);
        }
    }

    public class NavItem
    {
        private readonly List<NavItem> _children;

        public NavItem()
        {
            _children = new List<NavItem>();
        }

        public string Name { get; set; }

        public string Tooltip { get; set; }

        public string TargetUrl { get; set; }

        public INavItemActivationStrategy ActivationStrategy { get; set; }

        public INavItemVisibilityStrategy VisibilityStrategy { get; set; }

        public INavItemEnabledStrategy EnabilityStrategy { get; set; }

        public bool HasChildren { get { return _children.Any(); } }

        public IEnumerable<NavItem> Children { get { return _children; } }

        public NodeVisibility EvaluateVisibility(ViewContext context)
        {
            return VisibilityStrategy.EvaluateVisibility(this, context);
        }

        public NodeActivation EvaluateActivation(ViewContext context)
        {
            return ActivationStrategy.EvaluateActivation(this, context);
        }

        public void AddChild(NavItem child)
        {
            _children.Add(child);
        }

        public NodeEnablement EvaluateEnablement(ViewContext context)
        {
            return EnabilityStrategy.EvaluateEnablement(this, context);
        }
    }

    public class NodeActivation
    {
        private readonly bool _isActive;
        private readonly string _reason;

        public NodeActivation(bool isActive, string reason)
        {
            if (reason == null) throw new ArgumentNullException("reason");
            _isActive = isActive;
            _reason = reason;
        }

        public bool IsActive
        {
            get { return _isActive; }
        }

        public string Reason
        {
            get { return _reason; }
        }
    }

    public class NodeVisibility
    {
        private readonly bool _visible;
        private readonly string _reason;

        public NodeVisibility(bool visible, string reason)
        {
            if (reason == null) throw new ArgumentNullException("reason");
            _visible = visible;
            _reason = reason;
        }

        public bool IsVisible { get { return _visible; } }
        public string Reason { get { return _reason; } }
    }

    public interface INavItemEnabledStrategy
    {
        NodeEnablement EvaluateEnablement(NavItem navItem, ViewContext context);
    }

    public class NodeEnablement
    {
        private readonly bool _disabled;
        private readonly string _reason;

        public NodeEnablement(bool disabled, string reason)
        {
            if (reason == null) throw new ArgumentNullException("reason");
            _disabled = disabled;
            _reason = reason;
        }

        public bool IsDisabled { get { return _disabled; } }
        public string Reason { get { return _reason; } }
    }

    public interface INavItemVisibilityStrategy
    {
        NodeVisibility EvaluateVisibility(NavItem navItem, ViewContext context);
    }

    public interface INavItemActivationStrategy
    {
        NodeActivation EvaluateActivation(NavItem navItem, ViewContext context);
    }

    public class NavRoot
    {
        private readonly List<NavItem> _children;

        public NavRoot()
        {
            _children = new List<NavItem>();
        }

        public void AddChild(NavItem child)
        {
            _children.Add(child);
        }

        public IEnumerable<NavItem> Children { get { return _children; } }
    }

    public class NavigationModelBuilder
    {
        public NavigationModelBase Build(ViewContext context, NavRoot tree)
        {
            var result = new DefaultNavigationModel(context);


            var root = new DefaultNavigationRootModel();

            foreach (var navItem in tree.Children)
            {
                var itemToAdd = this.BuildNavItem(context, navItem);

                root.AddChild(itemToAdd);
            }


            result.SetNavigationRoot(root);

            return result;
        }

        private DefaultNavigationItemModel BuildNavItem(ViewContext context, NavItem navItem)
        {
            var result = new DefaultNavigationItemModel
                             {
                                 Name = navItem.Name,
                                 TargetUrl = navItem.TargetUrl,
                                 ToolTip = navItem.Tooltip
                             };

            var visibility = navItem.EvaluateVisibility(context);
            result.SetVisibility(visibility.IsVisible, visibility.Reason);

            var activation = navItem.EvaluateActivation(context);
            result.SetActivation(activation.IsActive, activation.Reason);

            var enablement = navItem.EvaluateEnablement(context);
            result.SetDisabled(enablement.IsDisabled, enablement.Reason);

            // any children ?
            if (navItem.HasChildren)
            {
                foreach (var child in navItem.Children)
                {
                    var childToAdd = BuildNavItem(context, child);
                    result.AddChild(childToAdd);
                }
            }

            return result;
        }
    }

    public class DefaultNavigationItemModel : INavigationNodeModel
    {
        private readonly List<DefaultNavigationItemModel> _children;

        public DefaultNavigationItemModel()
        {
            _children = new List<DefaultNavigationItemModel>();
        }

        public string Name { get; set; }
        public string TargetUrl { get; set; }
        public string ToolTip { get; set; }
        public bool IsActive { get; private set; }
        public bool IsDisabled { get; private set; }
        public bool IsVisible { get; private set; }
        public string VisibilityReason { get; private set; }
        public string ActivationReason { get; private set; }
        public bool HasChildren { get { return _children.Any(); } }
        public IEnumerable<INavigationNodeModel> Children { get { return _children; } }

        public void SetVisibility(bool visible, string reason)
        {
            IsVisible = visible;
            VisibilityReason = reason;
        }

        public void SetActivation(bool isActive, string reason)
        {
            if (reason == null) throw new ArgumentNullException("reason");
            IsActive = isActive;
            ActivationReason = reason;
        }


        public void AddChild(DefaultNavigationItemModel childToAdd)
        {
            _children.Add(childToAdd);
        }

        public void SetDisabled(bool isDisabled, string reason)
        {
            if (reason == null) throw new ArgumentNullException("reason");
            IsDisabled = isDisabled;
            DisabledReason = reason;
        }

        public string DisabledReason { get; private set; }
    }

    public abstract class NavigationModelBase
    {
        private readonly ViewContext _callingContext;

        public NavigationModelBase(ViewContext callingContext)
        {
            if (callingContext == null) throw new ArgumentNullException("callingContext");
            _callingContext = callingContext;
        }

        public ViewContext CallingContext { get { return _callingContext; } }

        public abstract INavigationRootModel NavigationRoot { get; }

    }

    public class DefaultNavigationModel : NavigationModelBase
    {
        private DefaultNavigationRootModel _root = null;

        public DefaultNavigationModel(ViewContext callingContext)
            : base(callingContext)
        {
        }

        public override INavigationRootModel NavigationRoot
        {
            get { return _root; }
        }

        public void SetNavigationRoot(DefaultNavigationRootModel rootModel)
        {
            if (rootModel == null) throw new ArgumentNullException("rootModel");
            _root = rootModel;
        }
    }

    public class DefaultNavigationRootModel : INavigationRootModel
    {
        private readonly List<INavigationNodeModel> _children;

        public DefaultNavigationRootModel()
        {
            _children = new List<INavigationNodeModel>();
        }

        public IEnumerable<INavigationNodeModel> Children { get { return _children; } }

        public void AddChild(DefaultNavigationItemModel itemToAdd)
        {
            _children.Add(itemToAdd);
        }
    }

    public interface INavigationRootModel
    {
        IEnumerable<INavigationNodeModel> Children { get; }
    }

    public interface INavigationNodeModel
    {
        string Name { get; }
        string TargetUrl { get; }
        string ToolTip { get; }
        bool IsActive { get; }
        bool IsDisabled { get; }

        bool IsVisible { get; }
        string VisibilityReason { get; }
        string ActivationReason { get; }
        string DisabledReason { get; }
        bool HasChildren { get; }
        IEnumerable<INavigationNodeModel> Children { get; }
    }
}
