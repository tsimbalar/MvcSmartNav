using System.Collections.Generic;
using System.Web.Mvc;
using MvcSmartNav.Activation;
using MvcSmartNav.Enablement;
using MvcSmartNav.Visibility;

namespace MvcSmartNav
{
    public interface INavComponent
    {
        string EvaluateTargetUrl(ViewContext context);

        string Name { get; }

        string Tooltip { get; set; }

        NodeVisibility EvaluateVisibility(ViewContext context);

        NodeActivation EvaluateActivation(ViewContext context);

        NodeEnablement EvaluateEnablement(ViewContext context);

        bool HasChildren { get; }

        IEnumerable<INavItem> Children { get; }
    }

    public interface INavRoot : INavComponent
    {
    }

    public interface INavItem : INavComponent
    {

    }
}