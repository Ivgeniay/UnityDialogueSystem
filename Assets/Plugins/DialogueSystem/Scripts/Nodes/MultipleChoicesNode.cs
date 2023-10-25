using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace DialogueSystem.Nodes
{
    public class MultipleChoicesNode : BaseNode
    {
        internal override void Initialize(Vector2 position)
        {
            DialogueType = Dialogue.DialogueType.MultipleChoice;
            base.Initialize(position);
            Choises.Add("New Choice");
        }

        protected override void DrawInputOutputContainer()
        {
            base.DrawInputOutputContainer();

            Button addChoiceBtn = new Button()
            {
                text = "Add Choice"
            };
            mainContainer.Insert(1, addChoiceBtn);

            foreach (var choice in Choises)
            {
                Port choicePort = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(bool));
                choicePort.portName = "";

                Button deleteChoiceBtn = new Button()
                {
                    text = "X"
                };
                TextField choiceText = new TextField()
                {
                    value = choice
                };
                choicePort.Add(deleteChoiceBtn);
                choicePort.Add(choiceText);

                outputContainer.Add(choicePort);
            }
        }
    }
}
