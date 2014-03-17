using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MvcSmartNav.Activation;
using MvcSmartNav.Enablement;
using MvcSmartNav.Visibility;

namespace MvcSmartNav
{
    public abstract class NavComponentBase<TUrl> : INavComponent where TUrl : ITargetUrlSpecification
    {
        private readonly string _name;
        private readonly List<INavItem> _children;
        private readonly TUrl _targetUrlSpecification;
        protected NavComponentBase(string name, TUrl targetUrlSpecification)
        {
            if (name == null) throw new ArgumentNullException("name");
            if (targetUrlSpecification == null) throw new ArgumentNullException("targetUrlSpecification");
            _name = name;
            _targetUrlSpecification = targetUrlSpecification;
            _children = new List<INavItem>();
        }

        public string EvaluateTargetUrl(ViewContext context)
        {
            return TargetUrlSpecification.EvaluateTargetUrl(context);
        }

        public string Name { get { return _name; } }

        public string Tooltip { get; set; }


        public bool HasChildren { get { return _children.Any(); } }

        public IEnumerable<INavItem> Children { get { return _children; } }

        protected TUrl TargetUrlSpecification
        {
            get { return _targetUrlSpecification; }
        }


        public void AddChild(INavItem child)
        {
            _children.Add(child);
        }


        public abstract NodeVisibility EvaluateVisibility(ViewContext context);

        public abstract NodeActivation EvaluateActivation(ViewContext context);

        public abstract NodeEnablement EvaluateEnablement(ViewContext context);
    }
}