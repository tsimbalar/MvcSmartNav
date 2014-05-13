using System.Collections.Generic;
using System.Web.Mvc;

namespace SmartNav.Tests.NavSpec
{
    public interface INavNode
    {
        string Name { get; }
        IEnumerable<INavNode> Children { get; }
        INavNodeProperties EvaluateNode(ViewContext viewContext);
    }
}