using System;
using System.Collections.Generic;

namespace SmartNav.Mvc.NavView
{
    public abstract class NavItemViewBase : INavItemViewModel
    {
        private readonly string _id;
        private readonly int _level;
        private readonly string _name;
        private readonly INavNodeProperties _props;
        private readonly List<INavItemViewModel> _children;

        protected NavItemViewBase(string id, int level, string name, INavNodeProperties props)
        {
            if (id == null) throw new ArgumentNullException("id");
            if (name == null) throw new ArgumentNullException("name");
            if (props == null) throw new ArgumentNullException("props");
            _id = id;
            _level = level;
            _name = name;
            _props = props;
            _children = new List<INavItemViewModel>();
        }

        public string Name { get { return _name; } }

        public bool IsVisible
        {
            get { return _props.Visibility.IsVisible; }
        }

        public string VisibilityReason
        {
            get { return _props.Visibility.Explanation; }
        }

        public bool IsEnabled
        {
            get { return _props.Enablement.IsEnabled; }
        }

        public string EnablementReason
        {
            get { return _props.Enablement.Explanation; }
        }

        public bool IsActive
        {
            get { return _props.Activation.IsActive; }
        }

        public string ActivationReason
        {
            get { return _props.Activation.Explanation; }
        }

        public string Url
        {
            get { return _props.TargetUrl; }
        }

        public IEnumerable<INavItemViewModel> Children { get { return _children; } }

        public string Id
        {
            get { return _id; }
        }

        public int Level { get { return _level; } }

        public void AddChild(INavItemViewModel child)
        {
            _children.Add(child);
        }
    }
}