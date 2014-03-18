using System;

namespace MvcSmartNav.Activation
{
    public sealed class NodeActivation
    {
        private readonly bool _isActive;
        private readonly string _reason;

        public NodeActivation(bool isActive, string reason)
        {
            if (reason == null) throw new ArgumentNullException("reason");
            _isActive = isActive;
            _reason = reason;
        }

        public bool IsActive
        {
            get { return _isActive; }
        }

        public string Reason
        {
            get { return _reason; }
        }
    }
}