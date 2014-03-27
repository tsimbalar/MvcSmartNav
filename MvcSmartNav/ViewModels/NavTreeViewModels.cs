using System;
using System.Web.Mvc;

namespace MvcSmartNav.ViewModels
{
    public abstract class NavTreeViewModelBase
    {
        private readonly ViewContext _callingContext;
        private readonly INavRootViewModel _root;

        protected NavTreeViewModelBase(ViewContext callingContext, INavRootViewModel root)
        {
            if (callingContext == null) throw new ArgumentNullException("callingContext");
            if (root == null) throw new ArgumentNullException("root");
            _callingContext = callingContext;
            _root = root;
        }

        public ViewContext CallingContext { get { return _callingContext; } }

        public INavRootViewModel NavigationRoot { get { return _root; } }

        public TimeSpan BuildDuration { get; set; }

    }

    public sealed class NavTreeViewModel : NavTreeViewModelBase
    {
        public NavTreeViewModel(ViewContext callingContext, INavRootViewModel root)
            : base(callingContext, root)
        {
        }

        
    }
}