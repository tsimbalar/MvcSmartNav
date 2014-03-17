using System;
using System.Web.Mvc;
using System.Web.Routing;

namespace MvcSmartNav
{
    public class DefaultControllerFactoryTypeResolver : DefaultControllerFactory, IControllerTypeResolver
    {
        public ControllerDescriptor GetControllerDescriptor(RequestContext requestContext, string controllerName)
        {
            var controllerFactory = DependencyResolver.Current.GetService<IControllerFactory>();
            if (controllerFactory!=null && !controllerFactory.GetType().IsInstanceOfType(typeof (DefaultControllerFactory)))
            {
                throw new InvalidOperationException("The registered ControllerFactory is not a DefaultControllerFactory ... so we don't know how to resolve controller types from controller names ... Define your own implementation of IControllerTypeResolver");
            }

            // no custom IControllerFactory defined ...
            
            var type = base.GetControllerType(requestContext, controllerName);
            return new ReflectedControllerDescriptor(type);
        }
    }
}
