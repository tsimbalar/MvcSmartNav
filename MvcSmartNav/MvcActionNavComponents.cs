using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MvcSmartNav.Activation;
using MvcSmartNav.Enablement;
using MvcSmartNav.Visibility;

namespace MvcSmartNav
{
    public abstract class MvcActionNavComponentBase : INavComponent
    {
        private readonly string _controllerName;
        private readonly string _actionName;
        private readonly object _routeValues;
        private readonly string _name;
        private INavItemActivationStrategy<MvcActionNavComponentBase> _activationStrategy;
        private INavItemVisibilityStrategy<MvcActionNavComponentBase> _visibilityStrategy;
        private INavItemEnabledStrategy<MvcActionNavComponentBase> _enabilityStrategy;
        private List<INavItem> _children;

        protected MvcActionNavComponentBase(string name, string controllerName, string actionName, object routeValues)
        {
            if (name == null) throw new ArgumentNullException("name");
            _controllerName = controllerName;
            _actionName = actionName;
            _routeValues = routeValues;
            _name = name;
            _children = new List<INavItem>();
        }


        public string EvaluateTargetUrl(ViewContext context)
        {
            return new UrlHelper(context.RequestContext).Action(_actionName, _controllerName, _routeValues);
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

        public INavItemEnabledStrategy<MvcActionNavComponentBase> EnabilityStrategy
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
    }
}
