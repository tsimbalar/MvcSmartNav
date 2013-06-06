namespace MvcSmartNav.ViewModels
{
    public class NavRootViewModel : NavComponentViewModelBase, INavRootViewModel
    {
        public NavRootViewModel(string name, string targetUrl) : base(name, targetUrl)
        {
        }
    }
}