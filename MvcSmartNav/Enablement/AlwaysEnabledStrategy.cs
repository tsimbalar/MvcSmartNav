using System.Web.Mvc;
using MvcSmartNav.Visibility;

namespace MvcSmartNav.Enablement
{
    public sealed class AlwaysEnabledStrategy : INavItemEnabledStrategy<INavComponent>
    {
        public NodeEnablement EvaluateEnablement(INavComponent navComponent, ViewContext context)
        {
            return new NodeEnablement(false, "Always enabled");
        }
    }


    public sealed class AlwaysEnabledStrategy<TController> : INavItemEnabledStrategy<MvcActionNavComponentBase<TController>> where TController : IController
    {
        public NodeEnablement EvaluateEnablement(MvcActionNavComponentBase<TController> navComponent, ViewContext context)
        {
            return new NodeEnablement(false, "Always enabled");
        }
    }
}