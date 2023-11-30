using DialogueSystem.Ports;
using DialogueSystem.Utilities;
using UnityEditor.Experimental.GraphView;

namespace DialogueSystem.Nodes
{
    internal abstract class BaseListNode : BaseCollectionsNode
    {

        public override void OnDestroyConnectionInput(BasePort port, Edge edge)
        {
            base.OnDestroyConnectionInput(port, edge);
            var outputs = GetOutputPorts();
            foreach (BasePort outputPort in outputs)
                outputPort.SetValue(DSUtilities.GetDefaultValue(outputPort.Type));
        }
    }
}
