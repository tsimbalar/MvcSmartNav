using System.Web.Mvc;

namespace MvcSmartNav.Visibility
{
    public interface INavItemVisibilityStrategy<in TNavComponent> where TNavComponent : INavComponent
    {
        NodeVisibility EvaluateVisibility(TNavComponent navComponent, ViewContext context);
    }
}