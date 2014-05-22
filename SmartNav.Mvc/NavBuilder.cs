using System;
using System.Collections.Generic;
using System.Web.Mvc;
using SmartNav.Mvc.NavView;

namespace SmartNav.Mvc
{
    internal sealed class NavBuilder
    {
        public INavViewModel Build(ViewContext viewContext, INavSpecification navSpec)
        {
            if (viewContext == null) throw new ArgumentNullException("viewContext");
            if (navSpec == null) throw new ArgumentNullException("navSpec");

            var rootNode = navSpec.Root;
            var rootProperties = rootNode.Evaluate(viewContext);
            var rootView = new NavRootView(rootNode.Id, rootNode.Name, rootProperties);

            AddChildrenRecursively(rootView, rootNode.Children, viewContext);

            return new NavView.NavView(rootView, viewContext);
        }

        private static void AddChildrenRecursively(NavItemViewBase rootToAddTo, IEnumerable<INavNodeSpecification> childrenSpecs, ViewContext context)
        {
            foreach (var navNode in childrenSpecs)
            {
                var nodeProperties = navNode.Evaluate(context);
                var child = new NavItemView(navNode.Id, rootToAddTo.Level + 1, navNode.Name, nodeProperties);
                AddChildrenRecursively(child, navNode.Children, context);
                rootToAddTo.AddChild(child);
            }
        }


    }
}