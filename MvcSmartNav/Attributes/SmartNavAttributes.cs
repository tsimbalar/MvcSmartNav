using System;
using System.Web.Mvc;
using MvcSmartNav.Enablement;
using MvcSmartNav.Visibility;

namespace MvcSmartNav.Attributes
{

    public interface ISmartNavEnabledAttribute
    {
        NodeEnablement EvaluateEnablement(ViewContext callingViewContext);

    }

    public interface ISmartNavVisibleAttribute
    {
        NodeVisibility EvaluateVisibility(ViewContext callingViewContext);

    }

    /// <summary>
    /// Trying to look like the MVC AuthorizeAttribute
    /// </summary>
    public sealed class SmartNavAuthorize : AuthorizeAttribute, ISmartNavEnabledAttribute, ISmartNavVisibleAttribute
    {
        public SmartNavAuthorize()
        {
            WhenUnauthorized = SmartNavAttributeMode.Disable;
        }

        public SmartNavAttributeMode WhenUnauthorized { get; set; }

        private Tuple<bool, string> IsAuthorized(ViewContext callingViewContext)
        {
            var authorized = this.AuthorizeCore(callingViewContext.HttpContext);
            if (authorized)
            {
                return Tuple.Create(true, "User is authorized");
            }
            else
            {
                return Tuple.Create(false,
                                    string.Format(
                                        "not authorized according to SmartNavAuthorize (Roles = {0}, Users = {1})",
                                        Roles, Users));
            }
        }


        public NodeEnablement EvaluateEnablement(ViewContext callingViewContext)
        {
            if (WhenUnauthorized != SmartNavAttributeMode.Disable)
            {
                return new NodeEnablement(false, "SmartNavAuthorize(WhenUnauthorized <> Disable)");
            }
            var authorizedTuple = IsAuthorized(callingViewContext);
            bool authorized = authorizedTuple.Item1;
            string reason = authorizedTuple.Item2;
            return new NodeEnablement(disabled: !authorized, reason: reason);
        }

        public NodeVisibility EvaluateVisibility(ViewContext callingViewContext)
        {
            if (WhenUnauthorized != SmartNavAttributeMode.Hide)
            {
                return new NodeVisibility(true, "SmartNavAuthorize(WhenUnauthorized <> Hide)");
            }
            var authorizedTuple = IsAuthorized(callingViewContext);
            bool authorized = authorizedTuple.Item1;
            string reason = authorizedTuple.Item2;
            return new NodeVisibility(visible: authorized, reason: reason);
        }
    }

    public enum SmartNavAttributeMode
    {
        Disable = 1,
        Hide = 2
    }
}