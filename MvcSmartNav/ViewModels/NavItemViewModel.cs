namespace MvcSmartNav.ViewModels
{
    public class NavItemViewModel : NavComponentViewModelBase, INavItemViewModel
    {
        public NavItemViewModel(string name, string targetUrl) : base(name, targetUrl)
        {
        }
    }
}