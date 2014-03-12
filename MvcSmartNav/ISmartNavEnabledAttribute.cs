using System.Web.Mvc;
using MvcSmartNav.Enablement;

namespace MvcSmartNav
{
    public interface ISmartNavEnabledAttribute
    {
        NodeEnablement EvaluateEnablement(ViewContext callingViewContext);

    }
}