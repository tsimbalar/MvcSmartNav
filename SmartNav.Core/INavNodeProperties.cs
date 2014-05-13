namespace SmartNav
{
    public interface INavNodeProperties
    {
        NodeActivation Activation { get; }
        NodeVisibility Visibility { get; }
        NodeEnablement Enablement { get; }

        string TargetUrl { get; }
    }
}