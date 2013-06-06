using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MvcSmartNav.Activation;
using MvcSmartNav.Enablement;
using MvcSmartNav.Visibility;

namespace MvcSmartNav
{
    public abstract class NavComponentBase : INavComponent
    {
        private readonly string _name;
        private readonly List<NavItem> _children;
        private INavItemActivationStrategy<NavComponentBase> _activationStrategy;
        private INavItemVisibilityStrategy<NavComponentBase> _visibilityStrategy;
        private INavItemEnabledStrategy<NavComponentBase> _enabilityStrategy;

        public NavComponentBase(string name)
        {
            if (name == null) throw new ArgumentNullException("name");
            _name = name;
            _children = new List<NavItem>();
            _activationStrategy = new ExactUrlActivationStrategy();
            _visibilityStrategy = new AlwaysVisibleStrategy();
            _enabilityStrategy = new AlwaysEnabledStrategy();

        }

        public string Name { get { return _name; } }

        public string Tooltip { get; set; }

        public string TargetUrl { get; set; }

        public INavItemActivationStrategy<NavComponentBase> ActivationStrategy
        {
            get { return _activationStrategy; }
            set
            {
                if (value == null) throw new ArgumentNullException("ActivationStrategy");
                _activationStrategy = value;
            }
        }

        public INavItemVisibilityStrategy<NavComponentBase> VisibilityStrategy
        {
            get { return _visibilityStrategy; }
            set
            {
                if (value == null) throw new ArgumentNullException("VisibilityStrategy");
                _visibilityStrategy = value;
            }
        }

        public INavItemEnabledStrategy<NavComponentBase> EnabilityStrategy
        {
            get { return _enabilityStrategy; }
            set
            {
                if (value == null) throw new ArgumentNullException("EnabilityStrategy");
                _enabilityStrategy = value;
            }
        }

        public bool HasChildren { get { return _children.Any(); } }

        public IEnumerable<INavItem> Children { get { return _children; } }

        

        public void AddChild(NavItem child)
        {
            _children.Add(child);
        }


        public NodeVisibility EvaluateVisibility(ViewContext context)
        {
            return VisibilityStrategy.EvaluateVisibility(this, context);
        }

        public NodeActivation EvaluateActivation(ViewContext context)
        {
            return ActivationStrategy.EvaluateActivation(this, context);
        }

        public NodeEnablement EvaluateEnablement(ViewContext context)
        {
            return EnabilityStrategy.EvaluateEnablement(this, context);
        }
    }

    public class NavItem : NavComponentBase, INavItem
    {
        public NavItem(string name)
            : base(name)
        {
        }
    }

    public class NavRoot : NavComponentBase, INavRoot
    {
        public NavRoot(string name, string targetUrl)
            : base(name)
        {
            TargetUrl = targetUrl;
        }
    }
}