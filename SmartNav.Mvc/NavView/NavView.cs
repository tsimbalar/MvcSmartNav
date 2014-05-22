using System;
using System.Web.Mvc;

namespace SmartNav.Mvc.NavView
{
    internal sealed class NavView : INavViewModel
    {
        private readonly NavRootView _navRootView;
        private readonly ViewContext _callingViewContext;

        public NavView(NavRootView navRootView, ViewContext callingViewContext)
        {
            if (navRootView == null) throw new ArgumentNullException("navRootView");
            if (callingViewContext == null) throw new ArgumentNullException("callingViewContext");
            _navRootView = navRootView;
            _callingViewContext = callingViewContext;
        }

        public INavItemViewModel Root { get { return _navRootView; } }
        public ViewContext CallingViewContext { get { return _callingViewContext; } }
    }
}