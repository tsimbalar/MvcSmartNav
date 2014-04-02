using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using JetBrains.Annotations;
using MvcSmartNav.Activation;
using MvcSmartNav.Enablement;
using MvcSmartNav.Visibility;

namespace MvcSmartNav
{
    public abstract class NavComponentBase : INavComponent
    {
        private readonly string _name;
        private readonly List<INavItem> _children = new List<INavItem>();

        protected NavComponentBase([NotNull] string name)
        {
            if (name == null) throw new ArgumentNullException("name");
            _name = name;
        }

        public abstract string EvaluateTargetUrl(ViewContext context);

        public string Name { get { return _name; } }
        public string Tooltip { get; set; }
        public abstract NodeVisibility EvaluateVisibility(ViewContext context);

        public abstract NodeActivation EvaluateActivation(ViewContext context);

        public abstract NodeEnablement EvaluateEnablement(ViewContext context);

        public bool HasChildren
        {
            get { return _children.Any(); }
        }

        public IEnumerable<INavItem> Children
        {
            get { return _children.AsReadOnly(); }
        }

        public void AddChild(INavItem child)
        {
            _children.Add(child);
        }
    }

    public abstract class NavComponentBase<TUrl> : NavComponentBase where TUrl : ITargetUrlSpecification
    {
        private readonly TUrl _targetUrlSpecification;
        protected NavComponentBase(string name, TUrl targetUrlSpecification)
            :base(name)
        {
            if (targetUrlSpecification == null) throw new ArgumentNullException("targetUrlSpecification");
            _targetUrlSpecification = targetUrlSpecification;
        }

        public override string EvaluateTargetUrl(ViewContext context)
        {
            return TargetUrlSpecification.EvaluateTargetUrl(context);
        }

        protected TUrl TargetUrlSpecification
        {
            get { return _targetUrlSpecification; }
        }
    }
}