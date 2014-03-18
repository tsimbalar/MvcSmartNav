using System.Web.Mvc;

namespace MvcSmartNav.Activation
{
    public interface INavItemActivationStrategy<in TNavComponent> where TNavComponent : INavComponent
    {
        NodeActivation EvaluateActivation(TNavComponent navComponent, ViewContext context);
    }
}