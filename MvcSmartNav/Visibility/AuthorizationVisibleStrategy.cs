using System.Web.Mvc;
using MvcSmartNav.Enablement;
using MvcSmartNav.Helpers;

namespace MvcSmartNav.Visibility
{
    public class AuthorizationVisibleStrategy<TController> : INavItemVisibilityStrategy<MvcActionNavComponentBase<TController>> where TController : IController
    {
        public NodeVisibility EvaluateVisibility(MvcActionNavComponentBase<TController> navComponent, ViewContext context)
        {
            var attribute = ReflectionHelper.GetActionAttribute<TController, ISmartNavVisibleAttribute>(navComponent.ActionName);
            if (attribute == null)
            {
                return new NodeVisibility(visible: true, reason: "no ISmartNavVisibleAttribute attribute");
            }

            return attribute.EvaluateVisibility(context);
        }
    }
}