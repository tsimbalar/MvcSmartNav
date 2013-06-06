using System.Web.Mvc;

namespace MvcSmartNav.Visibility
{
    public class NeverVisibleStrategy : INavItemVisibilityStrategy<INavComponent>
    {
        public NodeVisibility EvaluateVisibility(INavComponent navComponent, ViewContext context)
        {
            return new NodeVisibility(false, "Never visible");
        }
    }
}