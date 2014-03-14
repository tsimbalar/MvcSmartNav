using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MvcSmartNav.Activation;
using MvcSmartNav.Enablement;
using MvcSmartNav.Visibility;

namespace MvcSmartNav
{
    public abstract class MvcActionNavComponentBase<TController> : INavComponent where TController : IController
    {
        private readonly string _controllerName;
        private readonly string _actionName;
        private readonly object _routeValues;
        private readonly string _name;
        private INavItemActivationStrategy<MvcActionNavComponentBase<TController>> _activationStrategy;
        private INavItemVisibilityStrategy<MvcActionNavComponentBase<TController>> _visibilityStrategy;
        private INavItemEnabledStrategy<MvcActionNavComponentBase<TController>> _enabilityStrategy;
        private readonly List<INavItem> _children;

        protected MvcActionNavComponentBase(string name, string controllerName, string actionName, object routeValues = null)
        {
            if (name == null) throw new ArgumentNullException("name");
            _controllerName = controllerName;
            _actionName = actionName;
            _routeValues = routeValues;
            _name = name;
            _children = new List<INavItem>();

            _activationStrategy = new ExactUrlActivationStrategy();
            _visibilityStrategy = new AuthorizationVisibleStrategy<TController>();
            _enabilityStrategy = new AuthorizationEnabledStrategy<TController>();
        }


        public string EvaluateTargetUrl(ViewContext context)
        {
            return new UrlHelper(context.RequestContext).Action(ActionName, ControllerName, RouteValues);
        }

        public string Name { get { return _name; } }
        public string Tooltip { get; set; }

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

        public INavItemEnabledStrategy<MvcActionNavComponentBase<TController>> EnabilityStrategy
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

        public string ControllerName
        {
            get { return _controllerName; }
        }

        public string ActionName
        {
            get { return _actionName; }
        }

        public object RouteValues
        {
            get { return _routeValues; }
        }


        public void AddChild(INavItem child)
        {
            _children.Add(child);
        }
    }


    public sealed class MvcActionNavItem<TController> : MvcActionNavComponentBase<TController>, INavItem where TController : IController
    {
        public MvcActionNavItem(string name, string controllerName, string actionName, object routeValues = null) : base(name, controllerName, actionName, routeValues)
        {
        }
    }

    public sealed class MvcActionNavRoot<TController> : MvcActionNavComponentBase<TController>, INavRoot where TController : IController
    {
        public MvcActionNavRoot(string name, string controllerName, string actionName, object routeValues = null) : base(name, controllerName, actionName, routeValues)
        {
        }
    }
}
