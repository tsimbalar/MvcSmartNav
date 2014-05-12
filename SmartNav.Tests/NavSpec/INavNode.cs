using System.Collections.Generic;
using System.Web.Mvc;

namespace SmartNav.Tests.NavSpec
{
    public interface INavNode
    {
        string Name { get; }
        IEnumerable<INavNode> Children { get; }
        NodeVisibility EvaluateVisibility(ViewContext viewContext);
        NodeEnablement EvaluateEnablement(ViewContext viewContext);
        NodeActivation EvaluateActivation(ViewContext viewContext);
        string EvaluateTargetUrl(ViewContext viewContext);
        INavNodeProperties EvaluateNode(ViewContext viewContext);
    }
}