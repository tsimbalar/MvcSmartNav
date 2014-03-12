using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Web.Mvc;

namespace MvcSmartNav.Enablement
{
    public class AuthorizationEnabledStrategy<TController, TResult> : INavItemEnabledStrategy<INavComponent> where TController : Controller
    {
        private readonly Expression<Func<TController, TResult>> _controllerActionSelector;

        public AuthorizationEnabledStrategy(Expression<Func<TController, TResult>> controllerActionSelector)
        {
            _controllerActionSelector = controllerActionSelector;
        }

        public NodeEnablement EvaluateEnablement(INavComponent navComponent, ViewContext context)
        {
            // TODO : there should be a way to find the matching controller and action once you know the target URL ....
            var action = (MethodCallExpression)_controllerActionSelector.Body;
            var method = action.Method;

            var attribute =
                method.GetCustomAttributes()
                .OfType<ISmartNavEnabledAttribute>()
                .SingleOrDefault();

            if (attribute == null)
            {
                return new NodeEnablement(disabled: false, reason: "no ISmartNavEnabledAttribute attribute");
            }

            var enabled = attribute.EvaluateEnablement(context);
            return enabled;
            //return null;
        }
    }
}