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

        internal static Type GetControllerTypeFromName([NotNull] RequestContext context, [NotNull] string controllerName)
        {
            if (context == null) throw new ArgumentNullException("context");
            if (controllerName == null) throw new ArgumentNullException("controllerName");
            var controllerFactory =  DependencyResolver.Current.GetService(typeof (IControllerFactory)) as IControllerFactory;
            if (controllerFactory != null)
            {
                throw new InvalidOperationException(string.Format("A custom Controller Factory has been defined .. Controller Name resolution is likely to fail (registered type : {0})", controllerFactory.GetType()));
            }

            var defaultControllerFactory = new DefaultControllerFactory();

            // try to invoke GetControllerType(RequestContext requestContext, string controllerName) ... Reflection, booooh
            var controllerTypeObject = InvokeNonPublicInstanceMethod(defaultControllerFactory, "GetControllerType",
                new object[] {context, controllerName});

            var controllerType = (Type)controllerTypeObject;

            return controllerType;

        }

        internal static object InvokeNonPublicInstanceMethod<TClass>(TClass instance, [NotNull] string methodName,
            object[] parameters)
        {
            if (methodName == null) throw new ArgumentNullException("methodName");
            var theMethod = typeof(TClass).GetMethod(methodName, BindingFlags.Instance | BindingFlags.NonPublic);

            if (theMethod == null)
            {
                throw new InvalidOperationException(string.Format("Could not find the non-public method {0} in type {1} ... something, somewhere must have changed ... ....", methodName, typeof (TClass)));
            }

            var result = theMethod.Invoke(instance, parameters);

            return result;

        }
    }
}
