using DialogueSystem.Database.Save;
using DialogueSystem.Utilities;
using DialogueSystem.Window;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace DialogueSystem.Nodes
{
    public abstract class BaseDialogueNode : BaseNode
    {
        internal override void Initialize(DialogueSystemGraphView graphView, Vector2 position, List<object> portsContext)
        {
            base.Initialize(graphView, position, portsContext: portsContext);

            if (portsContext == null )
            {
                Model.Text = "Dialogue text";
                Inputs.Add(new DialogueSystemPortModel(new System.Type[]
                {
                    typeof(string),
                    typeof(int),
                    typeof(float),
                    typeof(double),
                    typeof(bool),
                })
                {
                    PortText = "Connection",
                    Cross = false,
                    IsField = false,
                    IsInput = true,
                    IsSingle = false,
                    Type = typeof(string),
                    Value = false,
                });
            }
        }

        protected override void DrawExtensionContainer(VisualElement container)
        {
            VisualElement customDataContainer = new VisualElement();
            customDataContainer.AddToClassList("ds-node__custom-data-container");

            Foldout textFolout = DialogueSystemUtilities.CreateFoldout("DialogueText", true);
            TextField textField = DialogueSystemUtilities.CreateTextArea(
                Model.Text.ToString(),
                styles: new string[]
                    {
                        "ds-node__textfield",
                        "ds-node__quote-textfield"
                    }
                );

            textFolout.Add(textField);
            customDataContainer.Add(textFolout);
            container.Add(customDataContainer);
        }
    }
}
