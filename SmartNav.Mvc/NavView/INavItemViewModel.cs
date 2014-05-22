using System.Collections.Generic;

namespace SmartNav.Mvc.NavView
{
    public interface INavItemViewModel
    {
        string Name { get; }
        bool IsVisible { get; }
        string VisibilityReason { get; }
        bool IsEnabled { get; }
        string EnablementReason { get; }
        bool IsActive { get; }
        string ActivationReason { get; }
        string Url { get; }
        IEnumerable<INavItemViewModel> Children { get; }
        string Id { get; }
        int Level { get; }
    }
}