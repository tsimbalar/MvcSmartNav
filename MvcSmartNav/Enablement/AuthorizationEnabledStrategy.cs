using System.Web.Mvc;
using MvcSmartNav.Attributes;
using MvcSmartNav.Helpers;

namespace MvcSmartNav.Enablement
{

    public sealed class AuthorizationEnabledStrategy<TController> : INavItemEnabledStrategy<MvcActionNavComponentBase<TController>> where TController : IController
    {

        public NodeEnablement EvaluateEnablement(MvcActionNavComponentBase<TController> navComponent, ViewContext context)
        {
            var attribute = ReflectionHelper.GetActionAttribute<TController, ISmartNavEnabledAttribute>(navComponent.ActionName);
            if (attribute == null)
            {
                return new NodeEnablement(disabled: false, reason: "no ISmartNavEnabledAttribute attribute");
            }

            var enabled = attribute.EvaluateEnablement(context);
            return enabled;
        }
    }
}