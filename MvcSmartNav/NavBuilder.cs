using System.Web.Mvc;
using MvcSmartNav.ViewModels;

namespace MvcSmartNav
{
    public class NavBuilder
    {
        public NavTreeViewModelBase Build(ViewContext context, INavRoot tree)
        {
            var root = new NavRootViewModel(tree.Name, tree.TargetUrl)
                           {
                               ToolTip = tree.Tooltip
                           };
            SetDisplayOptionsAndChildren(context, source: tree, target: root);

            var result = new NavTreeViewModel(context, root);

            return result;
        }


        private NavItemViewModel BuildNavItem<TNavItem>(ViewContext context, TNavItem navItem) where TNavItem : INavItem
        {
            var result = new NavItemViewModel(navItem.Name, navItem.TargetUrl)
                             {
                                 ToolTip = navItem.Tooltip
                             };

            SetDisplayOptionsAndChildren(context, source:navItem, target:result);
            return result;
        }


        private void SetDisplayOptionsAndChildren(ViewContext context, INavComponent source, NavComponentViewModelBase target)
        {
            var visibility = source.EvaluateVisibility(context);
            target.SetVisibility(visibility.IsVisible, visibility.Reason);

            var activation = source.EvaluateActivation(context);
            target.SetActivation(activation.IsActive, activation.Reason);

            var enablement = source.EvaluateEnablement(context);
            target.SetDisabled(enablement.IsDisabled, enablement.Reason);

            // any children ?
            if (source.HasChildren)
            {
                foreach (var child in source.Children)
                {
                    var childToAdd = BuildNavItem(context, child);
                    target.AddChild(childToAdd);
                }
            }
        }

    }
}