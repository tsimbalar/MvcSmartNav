namespace SmartNav
{
    /// <summary>
    /// The specification for a dynamically generated navigation tree
    /// </summary>
    public interface INavSpecification
    {
        INavNodeSpecification Root { get; }
    }
}