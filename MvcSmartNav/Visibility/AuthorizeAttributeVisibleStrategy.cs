using System.Web.Mvc;
using MvcSmartNav.Attributes;
using MvcSmartNav.Helpers;

namespace MvcSmartNav.Visibility
{
    public sealed class AuthorizeAttributeVisibleStrategy<TController> : INavItemVisibilityStrategy<MvcActionNavComponentBase<TController>> where TController : IController
    {
        public NodeVisibility EvaluateVisibility(MvcActionNavComponentBase<TController> navComponent, ViewContext context)
        {
            var attribute = ReflectionHelper.GetActionAttribute<TController, AuthorizeAttribute>(navComponent.ActionName);
            if (attribute == null)
            {
                return new NodeVisibility(visible: true, reason: "no AuthorizeAttribute attribute");
            }

            var wrapped = new SmartNavAuthorizeWrapper(attribute);
            return wrapped.EvaluateVisibility(context);
        }
    }
}