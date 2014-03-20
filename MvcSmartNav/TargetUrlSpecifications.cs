using System;
using System.Web.Mvc;
using JetBrains.Annotations;

namespace MvcSmartNav
{
    public interface ITargetUrlSpecification
    {
        string EvaluateTargetUrl(ViewContext context);
    }

    public sealed class StaticUrlSpecification : ITargetUrlSpecification
    {
        private readonly string _targetUrl;

        public StaticUrlSpecification(string targetUrl)
        {
            if (targetUrl == null) throw new ArgumentNullException("targetUrl");
            _targetUrl = targetUrl;
        }

        public string EvaluateTargetUrl(ViewContext context)
        {
            return _targetUrl;
        }
    }

    public sealed class MvcActionUrlSpecification<TController> : ITargetUrlSpecification where TController : IController
    {
        private readonly string _actionName;
        private readonly object _routeValues;

        public MvcActionUrlSpecification([AspMvcActionSelector] string actionName, object routeValues = null)
        {
            if (actionName == null) throw new ArgumentNullException("actionName");
            _actionName = actionName;
            _routeValues = routeValues;
        }

        public string EvaluateTargetUrl(ViewContext context)
        {
            string controllerName = ControllerName;
            return new UrlHelper(context.RequestContext).Action(ActionName, controllerName, RouteValues);
        }

        public string ControllerName
        {
            get
            {
                var controllerTypeName = typeof(TController).Name;
                return controllerTypeName.Substring(0, controllerTypeName.Length - "Controller".Length);
            }
        }

        public string ActionName
        {
            get { return _actionName; }
        }

        public object RouteValues
        {
            get { return _routeValues; }
        }
    }
}