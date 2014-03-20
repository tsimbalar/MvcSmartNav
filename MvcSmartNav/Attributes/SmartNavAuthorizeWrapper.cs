using System;
using System.Reflection;
using System.Web.Mvc;
using MvcSmartNav.Enablement;
using MvcSmartNav.Visibility;

namespace MvcSmartNav.Attributes
{

    internal class SmartNavAuthorizeWrapper
    {
        private readonly AuthorizeAttribute _wrapped;

        public SmartNavAuthorizeWrapper(AuthorizeAttribute wrapped)
        {
            if (wrapped == null) throw new ArgumentNullException("wrapped");
            _wrapped = wrapped;
        }


        private Tuple<bool, string> IsAuthorized(ViewContext callingViewContext)
        {
            // try to invoke AuthorizeCore on the AuthorizeAttribute ... Reflection, booooh
            
            var authorizeCoreMethod = typeof (AuthorizeAttribute).GetMethod("AuthorizeCore",
                BindingFlags.Instance | BindingFlags.NonPublic);
            if (authorizeCoreMethod == null)
            {
                throw new InvalidOperationException("Could not find the protected method AuthorizeCore in AuthorizeAttribute ... maybe the ASP.NET MVC team have changed something ....");
            }
            var authorizedObject = authorizeCoreMethod.Invoke(this._wrapped, new object[]{callingViewContext.HttpContext});
            var authorized = Convert.ToBoolean(authorizedObject);

            return Tuple.Create(authorized, String.Format(
                "User is {0} according to AuthorizeAttribute (Roles = {1}, Users = {2})",
                authorized ? "authorized" : "not authorized",
                _wrapped.Roles,
                _wrapped.Users
                ));
        }


        public NodeEnablement EvaluateEnablement(ViewContext callingViewContext)
        {
            var authorizedTuple = IsAuthorized(callingViewContext);
            bool authorized = authorizedTuple.Item1;
            string reason = authorizedTuple.Item2;
            return new NodeEnablement(disabled: !authorized, reason: reason);
        }

        public NodeVisibility EvaluateVisibility(ViewContext callingViewContext)
        {
            var authorizedTuple = IsAuthorized(callingViewContext);
            bool authorized = authorizedTuple.Item1;
            string reason = authorizedTuple.Item2;
            return new NodeVisibility(visible: authorized, reason: reason);
        }
    }
}