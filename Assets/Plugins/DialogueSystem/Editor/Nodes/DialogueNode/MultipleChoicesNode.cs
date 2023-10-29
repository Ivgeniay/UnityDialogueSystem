using DialogueSystem.Database.Save;
using DialogueSystem.Ports;
using DialogueSystem.Utilities;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;
using DialogueSystem.Window;
using UnityEngine;

namespace DialogueSystem.Nodes
{
    internal class MultipleChoicesNode : BaseDialogueNode
    {
        internal override void Initialize(DialogueSystemGraphView graphView, Vector2 position)
        {
            base.Initialize(graphView, position);

            Outputs.Add(new DialogueSystemOutputModel(ID)
            {
                Value = "Next Choice"
            });
        }
        protected override void DrawMainContainer(VisualElement container)
        {
            base.DrawMainContainer(container);

            DialogueSystemOutputModel choiceData = new DialogueSystemOutputModel(ID)
            {
                Value = "Next Choice",
            };

            Button addChoiceBtn = DialogueSystemUtilities.CreateButton(
                "Add Choice", 
                () =>
                {
                    Port choicePort = CreateOutputPort(choiceData);
                    Outputs.Add(choiceData);
                    outputContainer.Add(choicePort);
                },
                styles: new string[]
                {
                    "ds-node__button"
                }
            );
            container.Insert(1, addChoiceBtn);
        }
        
    }
}
