using DialogueSystem.Database.Save;
using DialogueSystem.UIElement;
using DialogueSystem.Utilities;
using DialogueSystem.Window;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace DialogueSystem.Nodes
{
    internal abstract class BaseDialogueNode : BaseNode
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
                    IsInput = true,
                    IsSingle = true,
                    Type = DSConstants.DialogueTypes[0],
                    Value = null,
                });
            }
        }

        protected override void DrawExtensionContainer(VisualElement container)
        {
            VisualElement customDataContainer = new VisualElement();
            customDataContainer.AddToClassList("ds-node__custom-data-container");

            Foldout textFolout = DSUtilities.CreateFoldout("DialogueText", true);
            textField = DSUtilities.CreateDSTextArea(
                graphView,
                value: Model.Text.ToString(),
                onChange: callback =>
                {
                    DSTextField target = callback.target as DSTextField;

                    if (target != null)
                    {
                        target.value = callback.newValue;
                        //target.Value = callback.newValue;
                    }
                    Model.Text = callback.newValue;
                },
                styles: new string[]
                    {
                        "ds-node__textfield",
                        "ds-node__quote-textfield"
                    }
                );
            textField.Name = "DialogueText";
            textField.anchors.OnDictionaryChangedEvent += (sender, args) => { SetEnableSetting(!textField.IsAnchored); };

            textFolout.Add(textField);
            customDataContainer.Add(textFolout);
            container.Add(customDataContainer);

            base.DrawExtensionContainer(container);

            InitializeSettingElement(container);
        }
    }
}
