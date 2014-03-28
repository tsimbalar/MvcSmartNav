using System;
using System.Web.Mvc;

namespace MvcSmartNav.Activation
{
    public sealed class ExactUrlPathAndQueryStringActivationStrategy : INavItemActivationStrategy<INavComponent>
    {
        public NodeActivation EvaluateActivation(INavComponent navComponent, ViewContext context)
        {
            var currentRelativeUrl = context.RequestContext.HttpContext.Request.Url.PathAndQuery;
            var targetUrl = navComponent.EvaluateTargetUrl(context);
            var isActive = currentRelativeUrl == targetUrl;
            string reason = "current Url (" + currentRelativeUrl + ") " + (isActive ? "matches" : "does not match") +
                            " targetUrl (" + targetUrl + ")";
            return new NodeActivation(isActive, reason);
        }
    }
}