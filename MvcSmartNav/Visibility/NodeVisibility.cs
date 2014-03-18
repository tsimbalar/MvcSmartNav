using System;

namespace MvcSmartNav.Visibility
{
    public sealed class NodeVisibility
    {
        private readonly bool _visible;
        private readonly string _reason;

        public NodeVisibility(bool visible, string reason)
        {
            if (reason == null) throw new ArgumentNullException("reason");
            _visible = visible;
            _reason = reason;
        }

        public bool IsVisible { get { return _visible; } }
        public string Reason { get { return _reason; } }
    }
}