using DialogueSystem.Ports;
using System;

namespace DialogueSystem.Anchor
{
    internal interface IAnchorObserver
    {
        public void OnAnchorUpdate(BasePort port, string newRegex);
        public void OnAnchorDelete(BasePort port);
    }
}
