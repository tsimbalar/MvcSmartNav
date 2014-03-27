using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using MvcSmartNav.ViewModels;

namespace MvcSmartNav.Helpers
{
    public static class HtmlHelperNavDebugExtensions
    {
        public static MvcHtmlString SmartNavDebug(this HtmlHelper helper, NavTreeViewModelBase treeModel)
        {
            var result = new StringBuilder();

            var openingDiv = new TagBuilder("div");
            openingDiv.Attributes["id"] = "smartNavDebugNav";
            openingDiv.AddCssClass("smartnav");
            result.Append(openingDiv.ToString(TagRenderMode.StartTag));

            // stuff to see it's DEBUG
            var extraInfo = new TagBuilder("div");
            extraInfo.AddCssClass("debug-info");
            result.Append(extraInfo.ToString(TagRenderMode.StartTag));

            var debugMode = new TagBuilder("h3");
            debugMode.SetInnerText("SmartNav Debug Nav");
            result.Append(debugMode.ToString(TagRenderMode.Normal));

            var debugHelpText = new TagBuilder("p");
            debugHelpText.SetInnerText("Hover over the links to see why links are invisible/disabled/active");
            result.Append(debugHelpText.ToString(TagRenderMode.Normal));

            var extraInfoEnd = new TagBuilder("div");
            result.Append(extraInfoEnd.ToString(TagRenderMode.EndTag));


            // real tree

            var rootLink = RenderNavLink(treeModel.NavigationRoot, "nav-item", "nav-root");
            result.Append(rootLink);

            var childList = RenderChildren(treeModel.NavigationRoot);
            result.Append(childList);


            // footer with timing information
            var footerDiv = new TagBuilder("div");
            footerDiv.AddCssClass("footer");
            result.Append(footerDiv.ToString(TagRenderMode.StartTag));

            var timingSpan = new TagBuilder("span");
            timingSpan.AddCssClass("time-info");
            timingSpan.SetInnerText(string.Format("Built in {0} ms", treeModel.BuildDuration.TotalMilliseconds));
            result.Append(timingSpan.ToString(TagRenderMode.Normal));

            var footerDivEnd = new TagBuilder("div");
            result.Append(footerDivEnd.ToString(TagRenderMode.EndTag));

            var closingDiv = new TagBuilder("div");
            result.Append(closingDiv.ToString(TagRenderMode.EndTag));

            return new MvcHtmlString(result.ToString());
        }

        private static string RenderChildren(INavComponentViewModel node)
        {
            if (!node.Children.Any())
            {
                return string.Empty;
            }
            var result = new StringBuilder();
            var openingUl = new TagBuilder("ul");
            openingUl.AddCssClass("nav-menu");
            var levelCssCLass = String.Format("nav-level-{0}", node.Level + 1);
            openingUl.AddCssClass(levelCssCLass);

            result.Append(openingUl.ToString(TagRenderMode.StartTag));

            foreach (var child in node.Children)
            {
                var liToAppend = new TagBuilder("li");
                liToAppend.AddCssClass("nav-item");
                result.Append(liToAppend.ToString(TagRenderMode.StartTag));
                result.Append(RenderNavLink(child, "nav-item"));

                result.Append(RenderChildren(child));

                var closingLi = new TagBuilder("li");
                result.Append(closingLi.ToString(TagRenderMode.EndTag));
            }

            var closingUl = new TagBuilder("ul");
            result.Append(closingUl.ToString(TagRenderMode.EndTag));

            return result.ToString();


        }

        private static string RenderNavLink(INavComponentViewModel node, params string[] cssClasses)
        {
            var rootLink = new TagBuilder("a");
            foreach (var cssClass in cssClasses)
            {
                rootLink.AddCssClass(cssClass);
            }

            AddCssClassesForItemStatus(rootLink, node);
            AddToolTipForNavItem(rootLink, node);
            rootLink.Attributes["href"] = node.TargetUrl;
            rootLink.SetInnerText(node.Name);

            return rootLink.ToString();
        }

        private static void AddToolTipForNavItem(TagBuilder tagBuilder, INavComponentViewModel node)
        {
            var tooltipParts = new List<String>
                                   {
                                       node.ToolTip
                                   };

            tooltipParts.Add("LEVEL " + node.Level);

            tooltipParts.Add((node.IsVisible ? "VISIBLE" : "INVISIBLE") + " because " + node.VisibilityReason);

            tooltipParts.Add((node.IsActive ? "ACTIVE" : "INACTIVE") + " because " + node.ActivationReason);

            tooltipParts.Add((node.IsDisabled ? "DISABLED" : "ENABLED") + " because " + node.DisabledReason);

            tagBuilder.Attributes["title"] = String.Join("\n- ", tooltipParts);
        }

        private static void AddCssClassesForItemStatus(TagBuilder builder, INavComponentViewModel node)
        {
            if (node.IsActive)
            {
                builder.AddCssClass("active");
            }
            if (node.IsDisabled)
            {
                builder.AddCssClass("disabled");
            }
            if (!node.IsVisible)
            {
                builder.AddCssClass("invisible");
            }
        }
    }
}
