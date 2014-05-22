using System.Web.Mvc;

namespace SmartNav.Mvc.NavView
{
    public interface INavViewModel
    {
        INavItemViewModel Root { get; }
        ViewContext CallingViewContext { get; }
    }
}