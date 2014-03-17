using System.Web.Mvc;

namespace MvcSmartNav
{
    public interface ITargetUrlSpecification
    {
        string EvaluateTargetUrl(ViewContext context);
    }
}