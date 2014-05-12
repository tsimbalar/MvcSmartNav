namespace SmartNav.Tests
{
    class NavNodeProperties : INavNodeProperties
    {
        public NodeActivation Activation { get; set; }
        public NodeVisibility Visibility { get; set; }
        public NodeEnablement Enablement { get; set; }
        public string TargetUrl { get; set; }
    }
}