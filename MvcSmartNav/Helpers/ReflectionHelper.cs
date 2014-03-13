using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace MvcSmartNav.Helpers
{
    internal class ReflectionHelper
    {

        internal static TAttribute GetActionAttribute<TController, TAttribute>(string actionName)
        {
            var controllerDescriptor = new ReflectedControllerDescriptor(typeof(TController));
            var action = controllerDescriptor.FindAction(new ControllerContext(), actionName);

            if (action == null)
            {
                throw new InvalidOperationException(string.Format("No action found with name {0} for controller of type {1}", actionName, typeof(TController).FullName));
            }

            var attribute =
                action.GetCustomAttributes(true)
                .OfType<TAttribute>()
                .SingleOrDefault();

            return attribute;

        }
    }
}
