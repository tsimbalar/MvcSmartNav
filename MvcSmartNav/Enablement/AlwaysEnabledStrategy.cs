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
}