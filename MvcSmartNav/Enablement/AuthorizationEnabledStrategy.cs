using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Web.Mvc;
using MvcSmartNav.Helpers;

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
        }
    }

    //public class AuthorizationEnabledStrategy : INavItemEnabledStrategy<MvcActionNavComponentBase>
    //{
    //    public NodeEnablement EvaluateEnablement(MvcActionNavComponentBase navComponent, ViewContext context)
    //    {
    //        //ControllerBuilder.Current.GetControllerFactory().
    //        //var controllerFactory = DependencyResolver.Current.GetService<IControllerFactory>();
    //        //controllerFactory.CreateController()
    //        throw new NotImplementedException();
    //    }
    //}

    public class AuthorizationEnabledStrategy<TController> : INavItemEnabledStrategy<MvcActionNavComponentBase<TController>> where TController : IController
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