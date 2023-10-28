using DialogueSystem.Utilities;
using DialogueSystem.Window;
using DialogueSystem.Database.Save;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace DialogueSystem.Nodes
{
    internal class SingleChoiceNode : BaseNode
    {
        internal override void Initialize(DialogueSystemGraphView graphView, Vector2 position)
        {
            DialogueType = Dialogue.DialogueType.SingleChoice;
            base.Initialize(graphView, position);
            Choises.Add(new DialogueSystemChoiceData()
            {
                Text = "Next Dialogue",

            });
        }

        protected override void DrawInputOutputContainer()
        {
            base.DrawInputOutputContainer();
            foreach (DialogueSystemChoiceData choice in Choises)
            {
                Port choicePort = this.CreatePort(
                    choice.Text, 
                    Orientation.Horizontal, 
                    Direction.Output, 
                    Port.Capacity.Single, 
                    type: typeof(bool));

                choicePort.userData = choice;

                outputContainer.Add(choicePort);
            }
        }

    }
}
