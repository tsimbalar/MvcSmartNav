using System;
using System.Collections.Generic;
using System.Linq;

namespace MvcSmartNav.ViewModels
{
    public abstract class NavComponentViewModelBase : INavComponentViewModel
    {
        private readonly string _name;
        private readonly string _targetUrl;
        private readonly List<NavItemViewModel> _children;

        protected NavComponentViewModelBase(string name, string targetUrl)
        {
            if (name == null) throw new ArgumentNullException("name");
            if (targetUrl == null) throw new ArgumentNullException("targetUrl");
            _name = name;
            _targetUrl = targetUrl;
            _children = new List<NavItemViewModel>();
        }

        public string Name { get { return _name; } }
        public string TargetUrl { get { return _targetUrl; } }
        public string ToolTip { get; set; }
        public bool IsActive { get; private set; }
        public bool IsDisabled { get; private set; }
        public bool IsVisible { get; private set; }
        public string VisibilityReason { get; private set; }
        public string ActivationReason { get; private set; }
        public bool HasChildren { get { return _children.Any(); } }
        public IEnumerable<INavItemViewModel> Children { get { return _children; } }

        public void SetVisibility(bool visible, string reason)
        {
            IsVisible = visible;
            VisibilityReason = reason;
        }

        public void SetActivation(bool isActive, string reason)
        {
            if (reason == null) throw new ArgumentNullException("reason");
            IsActive = isActive;
            ActivationReason = reason;
        }


        public void AddChild(NavItemViewModel childToAdd)
        {
            _children.Add(childToAdd);
        }

        public void SetDisabled(bool isDisabled, string reason)
        {
            if (reason == null) throw new ArgumentNullException("reason");
            IsDisabled = isDisabled;
            DisabledReason = reason;
        }

        public string DisabledReason { get; private set; }
    }
}