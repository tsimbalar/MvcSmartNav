using System.Collections.Generic;

namespace SmartNav.Tests.NavView
{
    public interface INavComponentViewModel
    {
        string Name { get; }
        bool IsVisible { get; }
        string VisibilityReason { get; }
        bool IsEnabled { get; }
        string EnablementReason { get; }
        bool IsActive { get; }
        string ActivationReason { get; }
        string Url { get; }
        IEnumerable<INavComponentViewModel> Children { get; }
    }
}