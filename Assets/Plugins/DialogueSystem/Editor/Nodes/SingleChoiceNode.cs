using DialogueSystem.Utilities;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace DialogueSystem.Nodes
{
    internal class SingleChoiceNode : BaseNode
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
                Port choicePort = this.CreatePort(
                    choice, 
                    Orientation.Horizontal, 
                    Direction.Output, 
                    Port.Capacity.Single, 
                    type: typeof(bool));
                outputContainer.Add(choicePort);
            }
        }

    }
}
