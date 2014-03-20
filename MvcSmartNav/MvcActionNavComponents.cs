using System;
using System.Web.Mvc;
using JetBrains.Annotations;
using MvcSmartNav.Activation;
using MvcSmartNav.Enablement;
using MvcSmartNav.Visibility;

namespace MvcSmartNav
{
    public abstract class MvcActionNavComponentBase<TController> : NavComponentBase<MvcActionUrlSpecification<TController>> where TController : IController
    {
        private INavItemActivationStrategy<MvcActionNavComponentBase<TController>> _activationStrategy;
        private INavItemVisibilityStrategy<MvcActionNavComponentBase<TController>> _visibilityStrategy;
        private INavItemEnabledStrategy<MvcActionNavComponentBase<TController>> _enablementStrategy;

        protected MvcActionNavComponentBase(string name, [AspMvcActionSelector] string actionName, object routeValues = null)
            : base(name, new MvcActionUrlSpecification<TController>(actionName, routeValues))
        {
            VisibilityStrategy = new AlwaysVisibleStrategy<TController>();
            EnablementStrategy = new AlwaysEnabledStrategy<TController>();
            ActivationStrategy = new ExactUrlActivationStrategy();
        }

        public INavItemActivationStrategy<MvcActionNavComponentBase<TController>> ActivationStrategy
        {
            get { return _activationStrategy; }
            set
            {
                if (value == null) throw new ArgumentNullException("ActivationStrategy");
                _activationStrategy = value;
            }
        }

        public INavItemVisibilityStrategy<MvcActionNavComponentBase<TController>> VisibilityStrategy
        {
            get { return _visibilityStrategy; }
            set
            {
                if (value == null) throw new ArgumentNullException("VisibilityStrategy");
                _visibilityStrategy = value;
            }
        }

        public INavItemEnabledStrategy<MvcActionNavComponentBase<TController>> EnablementStrategy
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
    }


    public sealed class MvcActionNavItem<TController> : MvcActionNavComponentBase<TController>, INavItem where TController : IController
    {
        public MvcActionNavItem(string name, [AspMvcActionSelector] string actionName, object routeValues = null)
            : base(name, actionName, routeValues)
        {

        }
    }

    public sealed class MvcActionNavRoot<TController> : MvcActionNavComponentBase<TController>, INavRoot where TController : IController
    {
        public MvcActionNavRoot(string name, [AspMvcActionSelector] string actionName, object routeValues = null)
            : base(name, actionName, routeValues)
        {
        }
    }
}
