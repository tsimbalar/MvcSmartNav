using System.Web.Mvc;

namespace MvcSmartNav.Enablement
{
    public interface INavItemEnabledStrategy<in TNavComponent> where TNavComponent : INavComponent
    {
        NodeEnablement EvaluateEnablement(TNavComponent navComponent, ViewContext context);
    }
}