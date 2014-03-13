using System;
using System.Web.Mvc;
using System.Web.Routing;

namespace MvcSmartNav
{
    public interface IControllerTypeResolver
    {
        ControllerDescriptor GetControllerDescriptor(RequestContext requestContext, string controllerName);
    }

    public class ControllerTypeResolver
    {
        private static Lazy<IControllerTypeResolver> _resolver = new Lazy<IControllerTypeResolver>(() => new DefaultControllerFactoryTypeResolver());

    }
}