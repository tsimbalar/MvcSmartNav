using System;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using System.Web.Routing;
using JetBrains.Annotations;

namespace MvcSmartNav.Helpers
{
    internal class ReflectionHelper
    {

        internal static TAttribute GetActionAttribute<TAttribute>([NotNull] Type controllerType,
            [NotNull] string actionName)
        {
            if (controllerType == null) throw new ArgumentNullException("controllerType");
            if (actionName == null) throw new ArgumentNullException("actionName");
            var controllerDescriptor = new ReflectedControllerDescriptor(controllerType);
            var action = controllerDescriptor.FindAction(new ControllerContext(), actionName);

            if (action == null)
            {
                throw new InvalidOperationException(string.Format("No action found with name {0} for controller of type {1}", actionName, controllerType.FullName));
            }

            var attribute =
                action.GetCustomAttributes(true)
                .OfType<TAttribute>()
                .SingleOrDefault();

            return attribute;

        }

        internal static Type GetControllerTypeFromName(RequestContext context, string controllerName)
        {
            var controllerFactory =  DependencyResolver.Current.GetService(typeof (IControllerFactory)) as IControllerFactory;
            if (controllerFactory != null)
            {
                throw new InvalidOperationException(string.Format("A custom Controller Factory has been defined .. Controller Name resolution is likely to fail (registered type : {0})", controllerFactory.GetType()));
            }

            controllerFactory = new DefaultControllerFactory();

            // try to invoke GetControllerType(RequestContext requestContext, string controllerName) ... Reflection, booooh

            var getControllerTypeMethod = typeof(DefaultControllerFactory).GetMethod("GetControllerType",
                BindingFlags.Instance | BindingFlags.NonPublic);
            if (getControllerTypeMethod == null)
            {
                throw new InvalidOperationException("Could not find the protected method GetControllerType in DefaultControllerFactory ... maybe the ASP.NET MVC team have changed something ....");
            }
            var typeObject = getControllerTypeMethod.Invoke(controllerFactory, new object[] { context, controllerName });
            var controllerType = (Type) typeObject;

            return controllerType;

        }
    }
}
