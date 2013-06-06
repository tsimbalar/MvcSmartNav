using System.Web.Mvc;
using MvcSmartNav.Enablement;

namespace MvcSmartNav
{
    public interface INavItemEnabledStrategy<in TNavComponent> where TNavComponent : INavComponent
    {
        NodeEnablement EvaluateEnablement(TNavComponent navComponent, ViewContext context);
    }
}