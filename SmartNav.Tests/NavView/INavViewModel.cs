using System.Web.Mvc;

namespace SmartNav.Tests.NavView
{
    public interface INavViewModel
    {
        INavItemViewModel Root { get; }
        ViewContext CallingViewContext { get; }
    }
}