using System.Web.Mvc;

namespace MvcSmartNav.Enablement
{
    public class AlwaysEnabledStrategy : INavItemEnabledStrategy<INavComponent>
    {
        public NodeEnablement EvaluateEnablement(INavComponent navComponent, ViewContext context)
        {
            return new NodeEnablement(false, "Always enabled");
        }
    }
}