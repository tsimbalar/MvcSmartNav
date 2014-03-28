using System.Web.Mvc;

namespace MvcSmartNav.Activation
{
    public sealed class ExactUrlPathActivationStrategy : INavItemActivationStrategy<INavComponent>
    {
        public NodeActivation EvaluateActivation(INavComponent navComponent, ViewContext context)
        {
            var currentRelativeUrl = context.RequestContext.HttpContext.Request.Url.PathAndQuery;
            var currentRelativeUrlWithoutQueryString = currentRelativeUrl.Split('?')[0];
            var targetUrl = navComponent.EvaluateTargetUrl(context);
            var targetUrlWithoutQueryString = targetUrl.Split('?')[0];
            var isActive = currentRelativeUrlWithoutQueryString == targetUrlWithoutQueryString;
            string reason = "current Url (" + currentRelativeUrl + ") " + (isActive ? "matches" : "does not match") +
                            " targetUrl (" + targetUrl + ") ignoring the query string";
            return new NodeActivation(isActive, reason);
        }
    }
}