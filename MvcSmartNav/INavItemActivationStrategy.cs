using System.Web.Mvc;
using MvcSmartNav.Activation;

namespace MvcSmartNav
{
    public interface INavItemActivationStrategy<in TNavComponent> where TNavComponent : INavComponent
    {
        NodeActivation EvaluateActivation(TNavComponent navComponent, ViewContext context);
    }
}