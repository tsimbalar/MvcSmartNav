using System;
using System.Web.Mvc;
using JetBrains.Annotations;
using MvcSmartNav.Activation;
using MvcSmartNav.Enablement;
using MvcSmartNav.Visibility;

namespace MvcSmartNav
{
    public abstract class MvcActionNavComponentBase : NavComponentBase<MvcActionUrlSpecification>
    {
        private INavItemActivationStrategy<MvcActionNavComponentBase> _activationStrategy;
        private INavItemVisibilityStrategy<MvcActionNavComponentBase> _visibilityStrategy;
        private INavItemEnabledStrategy<MvcActionNavComponentBase> _enablementStrategy;

        protected MvcActionNavComponentBase(string name, [AspMvcController] string controllerName, [AspMvcActionSelector] string actionName, object routeValues = null)
            : base(name, new MvcActionUrlSpecification(controllerName, actionName, routeValues))
        {
            VisibilityStrategy = new AlwaysVisibleStrategy();
            EnablementStrategy = new AlwaysEnabledStrategy();
            ActivationStrategy = new ExactUrlActivationStrategy();
        }

        public INavItemActivationStrategy<MvcActionNavComponentBase> ActivationStrategy
        {
            get { return _activationStrategy; }
            set
            {
                if (value == null) throw new ArgumentNullException("ActivationStrategy");
                _activationStrategy = value;
            }
        }

        public INavItemVisibilityStrategy<MvcActionNavComponentBase> VisibilityStrategy
        {
            get { return _visibilityStrategy; }
            set
            {
                if (value == null) throw new ArgumentNullException("VisibilityStrategy");
                _visibilityStrategy = value;
            }
        }

        public INavItemEnabledStrategy<MvcActionNavComponentBase> EnablementStrategy
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

        public string ActionName
        {
            get { return TargetUrlSpecification.ActionName; }
        }

        public string ControllerName
        {
            get { return TargetUrlSpecification.ControllerName; }
        }
    }


    public sealed class MvcActionNavItem : MvcActionNavComponentBase, INavItem
    {
        public MvcActionNavItem(string name, [AspMvcController] string controllerName, [AspMvcActionSelector] string actionName, object routeValues = null) : base(name, controllerName, actionName, routeValues)
        {
        }
    }

    public sealed class MvcActionNavRoot : MvcActionNavComponentBase, INavRoot
    {
        public MvcActionNavRoot(string name, [AspMvcController] string controllerName, [AspMvcActionSelector] string actionName, object routeValues = null)
            : base(name, controllerName, actionName, routeValues)
        {
        }
    }
}
