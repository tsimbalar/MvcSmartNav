using System;
using System.Web.Mvc;
using MvcSmartNav.Attributes;
using MvcSmartNav.Helpers;

namespace MvcSmartNav.Visibility
{
    public sealed class AuthorizeAttributeVisibleStrategy : INavItemVisibilityStrategy<MvcActionNavComponentBase> 
    {
        public NodeVisibility EvaluateVisibility(MvcActionNavComponentBase navComponent, ViewContext context)
        {
            // find the controller type from the name ..
            var controllerName = navComponent.ControllerName;

            Type controllerType = ReflectionHelper.GetControllerTypeFromName(context.RequestContext, controllerName);
            var attribute = ReflectionHelper.GetActionAttribute<AuthorizeAttribute>(controllerType, navComponent.ActionName);
            if (attribute == null)
            {
                return new NodeVisibility(visible: true, reason: "no AuthorizeAttribute attribute");
            }

            var wrapped = new SmartNavAuthorizeWrapper(attribute);
            return wrapped.EvaluateVisibility(context);
        }
    }
}