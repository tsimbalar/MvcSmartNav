using System;
using System.Web.Mvc;
using MvcSmartNav.Attributes;
using MvcSmartNav.Helpers;

namespace MvcSmartNav.Enablement
{

    public sealed class AuthorizeAttributeEnabledStrategy : INavItemEnabledStrategy<MvcActionNavComponentBase>
    {

        public NodeEnablement EvaluateEnablement(MvcActionNavComponentBase navComponent, ViewContext context)
        {
            var controllerName = navComponent.ControllerName;
            Type controllerType = ReflectionHelper.GetControllerTypeFromName(context.RequestContext, controllerName);
            var attribute = ReflectionHelper.GetActionAttribute<AuthorizeAttribute>(controllerType, navComponent.ActionName);
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