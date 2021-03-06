using System;
using System.Collections.Generic;
using System.Linq;

namespace MvcSmartNav.ViewModels
{
    public abstract class NavComponentViewModelBase : INavComponentViewModel
    {
        private readonly string _name;
        private readonly string _targetUrl;
        private readonly int _level;
        private readonly List<NavItemViewModel> _children;

        protected NavComponentViewModelBase(string name, string targetUrl, int level)
        {
            if (name == null) throw new ArgumentNullException("name");
            if (targetUrl == null) throw new ArgumentNullException("targetUrl");
            _name = name;
            _targetUrl = targetUrl;
            _level = level;
            _children = new List<NavItemViewModel>();
        }

        public string Name { get { return _name; } }
        public string TargetUrl { get { return _targetUrl; } }
        public string ToolTip { get; set; }

        public bool IsActive { get; private set; }
        public string ActivationReason { get; private set; }
        public bool IsDisabled { get; private set; }
        public string DisabledReason { get; private set; }
        public bool IsVisible { get; private set; }
        public string VisibilityReason { get; private set; }

        public bool HasChildren { get { return _children.Any(); } }
        public IEnumerable<INavItemViewModel> Children { get { return _children; } }

        public int Level
        {
            get { return _level; }
        }

        public void SetDisabled(bool isDisabled, string reason)
        {
            if (reason == null) throw new ArgumentNullException("reason");
            IsDisabled = isDisabled;
            DisabledReason = reason;
        }

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



    }

    public sealed class NavRootViewModel : NavComponentViewModelBase, INavRootViewModel
    {
        public NavRootViewModel(string name, string targetUrl)
            : base(name, targetUrl, level : 0)
        {
        }
    }

    public sealed class NavItemViewModel : NavComponentViewModelBase, INavItemViewModel
    {
        public NavItemViewModel(string name, string targetUrl, int level)
            : base(name, targetUrl, level)
        {
        }
    }
}