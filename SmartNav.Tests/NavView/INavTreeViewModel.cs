using System.Web.Mvc;

namespace SmartNav.Tests.NavView
{
    public interface INavTreeViewModel
    {
        INavComponentViewModel Root { get; }
        ViewContext CallingViewContext { get; }
    }
}