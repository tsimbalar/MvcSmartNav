using System.Web.Mvc;
using MvcSmartNav.Attributes;
using MvcSmartNav.Helpers;

namespace MvcSmartNav.Enablement
{

    public sealed class AuthorizeAttributeEnabledStrategy<TController> : INavItemEnabledStrategy<MvcActionNavComponentBase<TController>> where TController : IController
    {

        public NodeEnablement EvaluateEnablement(MvcActionNavComponentBase<TController> navComponent, ViewContext context)
        {

            var attribute = ReflectionHelper.GetActionAttribute<TController, AuthorizeAttribute>(navComponent.ActionName);
            if (attribute == null)
            {
                return new NodeEnablement(disabled: false, reason: "no AuthorizeAttribute attribute");
            }
            var wrapped = new SmartNavAuthorizeWrapper(attribute);
            var enabled = wrapped.EvaluateEnablement(context);
            return enabled;

        }
    }
}