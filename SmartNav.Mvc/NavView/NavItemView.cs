namespace SmartNav.Mvc.NavView
{
    public sealed class NavItemView : NavItemViewBase
    {
        public NavItemView(string id, int level, string name, INavNodeProperties props)
            : base(id, level, name, props)
        {
        }
    }
}