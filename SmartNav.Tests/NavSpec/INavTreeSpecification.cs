namespace SmartNav.Tests.NavSpec
{
    /// <summary>
    /// The specification for a dynamically generated navigation tree
    /// </summary>
    public interface INavTreeSpecification
    {
        INavNode Root { get; }
    }
}