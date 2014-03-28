using System;
using System.Web.Mvc;
using MvcSmartNav.Activation;
using MvcSmartNav.Enablement;
using MvcSmartNav.Visibility;

namespace MvcSmartNav
{
    public abstract class NavStaticComponentBase : NavComponentBase<StaticUrlSpecification>
    {
        private INavItemActivationStrategy<NavStaticComponentBase> _activationStrategy;
        private INavItemVisibilityStrategy<NavStaticComponentBase> _visibilityStrategy;
        private INavItemEnabledStrategy<NavStaticComponentBase> _enablementStrategy;
        protected NavStaticComponentBase(string name, string url)
            :base(name, new StaticUrlSpecification(url))
        {
            _activationStrategy = new ExactUrlPathActivationStrategy();
            _visibilityStrategy = new AlwaysVisibleStrategy();
            _enablementStrategy = new AlwaysEnabledStrategy();

        }

        public INavItemActivationStrategy<NavStaticComponentBase> ActivationStrategy
        {
            get { return _activationStrategy; }
            set
            {
                if (value == null) throw new ArgumentNullException("ActivationStrategy");
                _activationStrategy = value;
            }
        }

        public INavItemVisibilityStrategy<NavStaticComponentBase> VisibilityStrategy
        {
            get { return _visibilityStrategy; }
            set
            {
                if (value == null) throw new ArgumentNullException("VisibilityStrategy");
                _visibilityStrategy = value;
            }
        }

        public INavItemEnabledStrategy<NavStaticComponentBase> EnablementStrategy
        {
            get { return _enablementStrategy; }
            set
            {
                if (value == null) throw new ArgumentNullException("EnablementStrategy");
                _enablementStrategy = value;
            }
        }

        public override NodeVisibility EvaluateVisibility(ViewContext context)
        {
            return VisibilityStrategy.EvaluateVisibility(this, context);
        }

        public override NodeActivation EvaluateActivation(ViewContext context)
        {
            return ActivationStrategy.EvaluateActivation(this, context);
        }

        public override NodeEnablement EvaluateEnablement(ViewContext context)
        {
            return EnablementStrategy.EvaluateEnablement(this, context);
        }
    }

    public sealed class NavStaticItem : NavStaticComponentBase, INavItem
    {

        public NavStaticItem(string name, string targetUrl = "")
            : base(name, targetUrl)
        {
        }
    }

    public sealed class NavStaticRoot : NavStaticComponentBase, INavRoot
    {
        public NavStaticRoot(string name, string targetUrl = "")
            : base(name, targetUrl)
        {
        }
    }
}