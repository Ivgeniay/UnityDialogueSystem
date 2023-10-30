using DialogueSystem.Database.Save;
using DialogueSystem.Ports;
using DialogueSystem.Utilities;
using UnityEditor.Experimental.GraphView;

namespace DialogueSystem.Nodes
{
    internal abstract class BaseConvertNode : BaseMathNode
    {
        protected override BasePort CreateInputPort(object userData)
        {
            DialogueSystemInputModel choiceData = userData as DialogueSystemInputModel;

            BasePort inputPort = this.CreatePort(
                choiceData.PortText,
                Orientation.Horizontal,
                Direction.Input,
                Port.Capacity.Single,
                type: choiceData.Type);
            inputPort.Value = choiceData.Value;

            return inputPort;
        }
        protected override BasePort CreateOutputPort(object userData)
        {
            DialogueSystemOutputModel choiceData = userData as DialogueSystemOutputModel;

            BasePort outputPort = this.CreatePort(
                choiceData.PortType.Name,
                orientation: Orientation.Horizontal,
                direction: Direction.Output,
                capacity: Port.Capacity.Multi,
                type: choiceData.PortType);
            outputPort.Value = choiceData.Value;

            return outputPort;
        }
    }
}
