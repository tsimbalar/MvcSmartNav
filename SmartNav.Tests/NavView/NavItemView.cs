namespace SmartNav.Tests.NavView
{
    public class NavItemView : NavItemViewBase
    {
        public NavItemView(string name, INavNodeProperties props)
            : base(name, props)
        {
        }

        public NavItemView AddChild(NavItemView navItemView)
        {
            base.AddChild(navItemView);
            return this;
        }
    }
}