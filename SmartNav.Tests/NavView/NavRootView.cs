namespace SmartNav.Tests.NavView
{
    public sealed class NavRootView : NavItemViewBase
    {
        public NavRootView(string id, string name, INavNodeProperties props)
            : base(id, 0, name, props)
        {
        }
    }
}