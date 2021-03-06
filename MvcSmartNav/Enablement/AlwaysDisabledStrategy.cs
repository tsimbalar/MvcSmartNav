using System.Web.Mvc;

namespace MvcSmartNav.Enablement
{
    public sealed class AlwaysDisabledStrategy : INavItemEnabledStrategy<INavComponent>
    {
        public NodeEnablement EvaluateEnablement(INavComponent navComponent, ViewContext context)
        {
            return new NodeEnablement(disabled: true, reason: "always disabled");
        }
    }
}