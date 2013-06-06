using System.Collections.Generic;

namespace MvcSmartNav.ViewModels
{
    public interface INavComponentViewModel
    {
        bool HasChildren { get; }
        IEnumerable<INavItemViewModel> Children { get; }

        string Name { get; }
        string TargetUrl { get; }

        bool IsActive { get; }
        bool IsDisabled { get; }
        bool IsVisible { get; }
        string VisibilityReason { get; }
        string ActivationReason { get; }
        string DisabledReason { get; }

        string ToolTip { get; }
    }
}
