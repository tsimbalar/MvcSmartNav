using System.Web.Mvc;
using MvcSmartNav.Visibility;

namespace MvcSmartNav
{
    public interface ISmartNavVisibleAttribute
    {
        NodeVisibility EvaluateVisibility(ViewContext callingViewContext);

    }


    /// <summary>
    /// Trying to look like the MVC AuthorizeAttribute
    /// </summary>
    public class SmartNavVisibleAuthorize : AuthorizeAttribute, ISmartNavVisibleAttribute
    {

        // Methods when in another action, but asking if this action should be visible in the menu
        // TODO : !!!!


        public NodeVisibility EvaluateVisibility(ViewContext callingViewContext)
        {
            var authorized = this.AuthorizeCore(callingViewContext.HttpContext);
            if (authorized)
            {
                return new NodeVisibility(visible: true, reason: "User is authorized");
            }
            else
            {
                return new NodeVisibility(visible: false,
                                          reason:
                                              string.Format(
                                                  "not authorized according to SmartNavVisibleAuthorize (Roles = {0}, Users = {1})",
                                                  Roles, Users));
            }
        }
    }
}