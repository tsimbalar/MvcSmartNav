namespace SmartNav.Tests.NavView
{
    public class NavRootView : NavItemViewBase
    {
        public NavRootView(string name, INavNodeProperties props)
            : base(name, props)
        {
        }

        public NavRootView AddChild(NavItemView navItemView)
        {
            base.AddChild(navItemView);
            return this;
        }
    }
}