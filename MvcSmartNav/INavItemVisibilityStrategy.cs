using System.Web.Mvc;
using MvcSmartNav.Visibility;

namespace MvcSmartNav
{
    public interface INavItemVisibilityStrategy<in TNavComponent> where TNavComponent : INavComponent
    {
        NodeVisibility EvaluateVisibility(TNavComponent navComponent, ViewContext context);
    }
}