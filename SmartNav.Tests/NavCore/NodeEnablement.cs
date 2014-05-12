namespace SmartNav.Tests
{
    public sealed class NodeEnablement
    {
        private readonly bool _enabled;
        private readonly string _explanation;

        public NodeEnablement(bool enabled, string explanation)
        {
            _enabled = enabled;
            _explanation = explanation;
        }

        public bool IsEnabled { get { return _enabled; } }
        public string Explanation { get { return _explanation; } }
    }
}