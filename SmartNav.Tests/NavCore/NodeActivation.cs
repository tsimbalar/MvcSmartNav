namespace SmartNav.Tests
{
    public sealed class NodeActivation
    {
        private readonly bool _active;
        private readonly string _explanation;

        public NodeActivation(bool active, string explanation)
        {
            _active = active;
            _explanation = explanation;
        }

        public bool IsActive { get { return _active; } }
        public string Explanation { get { return _explanation; } }
    }
}