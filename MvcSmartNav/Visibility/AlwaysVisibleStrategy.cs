using System.Web.Mvc;

namespace MvcSmartNav.Visibility
{
    public class AlwaysVisibleStrategy : INavItemVisibilityStrategy<INavComponent>
    {
        public NodeVisibility EvaluateVisibility(INavComponent navComponent, ViewContext context)
        {
            return new NodeVisibility(true, "Always visible");
        }
    }
}