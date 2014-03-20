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

    public sealed class MvcActionUrlSpecification : ITargetUrlSpecification
    {
        private readonly string _controllerName;
        private readonly string _actionName;
        private readonly object _routeValues;

        public MvcActionUrlSpecification([NotNull, AspMvcController] string controllerName, [AspMvcActionSelector] string actionName, object routeValues = null)
        {
            if (controllerName == null) throw new ArgumentNullException("controllerName");
            if (actionName == null) throw new ArgumentNullException("actionName");
            _controllerName = controllerName;
            _actionName = actionName;
            _routeValues = routeValues;
        }

        public string EvaluateTargetUrl(ViewContext context)
        {
            return new UrlHelper(context.RequestContext).Action(ActionName, _controllerName, RouteValues);
        }

        public string ActionName
        {
            get { return _actionName; }
        }

        public object RouteValues
        {
            get { return _routeValues; }
        }

        public string ControllerName
        {
            get { return _controllerName; }
        }
    }
}