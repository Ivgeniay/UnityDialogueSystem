using DialogueSystem.Database.Save;
using UnityEditor.Experimental.GraphView;
using DialogueSystem.Utilities;
using DialogueSystem.Ports;

namespace DialogueSystem.Nodes
{
    public abstract class BaseNumbersNode : BaseMathNode
    {
        protected override BasePort CreateOutputPort(object userData)
        {
            DialogueSystemOutputModel choiceData = userData as DialogueSystemOutputModel;

            BasePort choicePort = this.CreatePort(
            "",
            Orientation.Horizontal,
            Direction.Output,
            Port.Capacity.Single,
            type: choiceData.PortType);
            choicePort.Value = choiceData.Value;

            return choicePort;
        }
    }
}
