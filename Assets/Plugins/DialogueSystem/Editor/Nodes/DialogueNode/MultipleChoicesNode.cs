using DialogueSystem.Database.Save;
using DialogueSystem.Utilities;
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

            Outputs.Add(new DialogueSystemPortModel(ID)
            {
                Value = "Next Choice",
                Cross = false,
                IsField = true,
                IsInput = false,
                IsSingle = false,
                PortText = string.Empty,
                Type = typeof(string),
            });
        }
        protected override void DrawMainContainer(VisualElement container)
        {
            base.DrawMainContainer(container);

            DialogueSystemPortModel choiceData = new DialogueSystemPortModel(ID)
            {
                Value = "Next Choice",
            };

            Button addChoiceBtn = DialogueSystemUtilities.CreateButton(
                "Add Choice", 
                () =>
                {
                    var t = AddPortByType(
                        portText: "",
                        type: typeof(string),
                        value: "Next Choice",
                        isInput: false,
                        isSingle: false,
                        isField: true,
                        cross: true);
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
