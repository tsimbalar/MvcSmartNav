using System.Diagnostics;
using System.Web.Mvc;
using System.Xml;
using MvcSmartNav.ViewModels;

namespace MvcSmartNav
{
    public class NavBuilder
    {
        public NavTreeViewModelBase Build(ViewContext context, INavRoot tree)
        {
            var timer = new Stopwatch();
            timer.Start();
 
            var root = new NavRootViewModel(tree.Name, tree.EvaluateTargetUrl(context))
                           {
                               ToolTip = tree.Tooltip
                           };
            SetDisplayOptionsAndChildren(context, source: tree, target: root);

            var result = new NavTreeViewModel(context, root);
            timer.Stop();
            result.BuildDuration = timer.Elapsed;
            return result;
        }


        private static NavItemViewModel BuildNavItem<TNavItem>(ViewContext context, TNavItem navItem) where TNavItem : INavItem
        {
            var result = new NavItemViewModel(navItem.Name, navItem.EvaluateTargetUrl(context))
                             {
                                 ToolTip = navItem.Tooltip
                             };

            SetDisplayOptionsAndChildren(context, source:navItem, target:result);
            return result;
        }


        private static void SetDisplayOptionsAndChildren(ViewContext context, INavComponent source, NavComponentViewModelBase target)
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