namespace SmartNav.Tests
{
    public interface INavNodeProperties
    {
        NodeActivation Activation { get; }
        NodeVisibility Visibility { get; }
        NodeEnablement Enablement { get; }

        string TargetUrl { get; }
    }
}