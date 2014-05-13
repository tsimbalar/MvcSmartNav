using System.Collections.Generic;
using System.Web.Mvc;

namespace SmartNav
{
    public interface INavNodeSpecification
    {
        /// <summary>
        /// Unique identifier for this item in the tree
        /// </summary>
        string Id { get; }

        /// <summary>
        /// Display name for this item in the tree
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Node located right under this node 
        /// </summary>
        IEnumerable<INavNodeSpecification> Children { get; }
        
        /// <summary>
        /// Evaluate properties for this node when rendering in the specified viewContext 
        /// </summary>
        /// <param name="viewContext">the context in which the navigation is being rendered</param>
        /// <returns></returns>
        INavNodeProperties Evaluate(ViewContext viewContext);
    }
}