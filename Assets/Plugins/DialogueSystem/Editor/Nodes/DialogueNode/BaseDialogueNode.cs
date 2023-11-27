using DialogueSystem.Database.Save;
using DialogueSystem.TextFields;
using DialogueSystem.Utilities;
using DialogueSystem.Window;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace DialogueSystem.Nodes
{
    public abstract class BaseDialogueNode : BaseNode
    {
        private DSTextField textField; 

        internal override void Initialize(DSGraphView graphView, Vector2 position, List<object> portsContext)
        {
            base.Initialize(graphView, position, context: portsContext);

            if (portsContext == null)
            {
                Model.Text = "Dialogue text";
                Model.AddPort(new DSPortModel(DSConstants.DialogueTypes, Ports.PortSide.Input)
                {
                    PortText = DSConstants.Dialogue,
                    Cross = false,
                    IsField = false,
                    IsInput = true,
                    IsSingle = true,
                    PlusIf = false,
                    Type = DSConstants.DialogueTypes[0],
                    Value = null,
                });
            }
        }

        internal virtual TextField GetDialogueTextField() => textField;

        protected override void DrawExtensionContainer(VisualElement container)
        {
            VisualElement customDataContainer = new VisualElement();
            customDataContainer.AddToClassList("ds-node__custom-data-container");

            Foldout textFolout = DSUtilities.CreateFoldout("DialogueText", true);
            textField = DSUtilities.CreateDSTextArea(
                value: Model.Text.ToString(),
                onChange: callback =>
                {
                    var target = callback.target as DSTextField;

                    target.value = callback.newValue;
                    Model.Text = callback.newValue;
                    target.Value = callback.newValue;
                },
                styles: new string[]
                    {
                        "ds-node__textfield",
                        "ds-node__quote-textfield"
                    }
                );
            textField.Name = "DialogueText";

            textFolout.Add(textField);
            customDataContainer.Add(textFolout);
            container.Add(customDataContainer);
        }
    }
}
