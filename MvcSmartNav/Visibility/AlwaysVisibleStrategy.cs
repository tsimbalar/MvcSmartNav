using System.Web.Mvc;

namespace MvcSmartNav.Visibility
{
    public sealed class AlwaysVisibleStrategy : INavItemVisibilityStrategy<INavComponent>
    {
        public NodeVisibility EvaluateVisibility(INavComponent navComponent, ViewContext context)
        {
            return new NodeVisibility(true, "Always visible");
        }
    }

    public sealed class AlwaysVisibleStrategy<TController> : INavItemVisibilityStrategy<MvcActionNavComponentBase<TController>> where TController : IController
    {
        public NodeVisibility EvaluateVisibility(MvcActionNavComponentBase<TController> navComponent, ViewContext context)
        {
            return new NodeVisibility(true, "Always visible");
        }
    }
}