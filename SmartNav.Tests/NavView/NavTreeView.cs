using System;
using System.Web.Mvc;

namespace SmartNav.Tests.NavView
{
    internal class NavTreeView : INavTreeViewModel
    {
        private readonly NavRootView _navRootView;
        private readonly ViewContext _callingViewContext;

        public NavTreeView(NavRootView navRootView, ViewContext callingViewContext)
        {
            if (navRootView == null) throw new ArgumentNullException("navRootView");
            if (callingViewContext == null) throw new ArgumentNullException("callingViewContext");
            _navRootView = navRootView;
            _callingViewContext = callingViewContext;
        }

        public INavComponentViewModel Root { get { return _navRootView; } }
        public ViewContext CallingViewContext { get { return _callingViewContext; } }
    }
}