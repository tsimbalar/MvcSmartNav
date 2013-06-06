using System.Web.Mvc;

namespace MvcSmartNav.ViewModels
{
    public class NavTreeViewModel : NavTreeViewModelBase
    {
        public NavTreeViewModel(ViewContext callingContext, INavRootViewModel root) : base(callingContext, root)
        {
        }
    }
}