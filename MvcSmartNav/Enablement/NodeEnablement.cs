using System;

namespace MvcSmartNav.Enablement
{
    public class NodeEnablement
    {
        private readonly bool _disabled;
        private readonly string _reason;

        public NodeEnablement(bool disabled, string reason)
        {
            if (reason == null) throw new ArgumentNullException("reason");
            _disabled = disabled;
            _reason = reason;
        }

        public bool IsDisabled { get { return _disabled; } }
        public string Reason { get { return _reason; } }
    }
}