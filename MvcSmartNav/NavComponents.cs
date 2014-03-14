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
        private readonly List<INavItem> _children;
        private INavItemActivationStrategy<NavComponentBase> _activationStrategy;
        private INavItemVisibilityStrategy<NavComponentBase> _visibilityStrategy;
        private INavItemEnabledStrategy<NavComponentBase> _enabilityStrategy;
        private string _targetUrl;

        protected NavComponentBase(string name, string targetUrl = "")
        {
            if (name == null) throw new ArgumentNullException("name");
            if (targetUrl == null) throw new ArgumentNullException("targetUrl");
            _name = name;
            _targetUrl = targetUrl;
            _children = new List<INavItem>();
            _activationStrategy = new ExactUrlActivationStrategy();
            _visibilityStrategy = new AlwaysVisibleStrategy();
            _enabilityStrategy = new AlwaysEnabledStrategy();

        }

        public string EvaluateTargetUrl(ViewContext context)
        {
            return _targetUrl;
        }

        public string Name { get { return _name; } }

        public string Tooltip { get; set; }

        public string TargetUrl
        {
            get { return _targetUrl; }
        }

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



        public void AddChild(INavItem child)
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

    public sealed class NavItem : NavComponentBase, INavItem
    {

        public NavItem(string name, string targetUrl = "")
            : base(name, targetUrl)
        {
        }
    }

    public sealed class NavRoot : NavComponentBase, INavRoot
    {
        public NavRoot(string name, string targetUrl = "")
            : base(name, targetUrl)
        {
        }
    }
}