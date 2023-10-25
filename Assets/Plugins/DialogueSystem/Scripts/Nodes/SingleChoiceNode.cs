using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace DialogueSystem.Nodes
{
    public class SingleChoiceNode : BaseNode
    {
        internal override void Initialize(Vector2 position)
        {
            DialogueType = Dialogue.DialogueType.SingleChoice;
            base.Initialize(position);
            Choises.Add("NextDialogue");
        }

        protected override void DrawInputOutputContainer()
        {
            base.DrawInputOutputContainer();
            foreach (var choice in Choises)
            {
                Port choicePort = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(bool));
                choicePort.portName = choice;
                outputContainer.Add(choicePort);
            }
        }

    }
}
