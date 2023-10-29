using DialogueSystem.Database.Save;
using DialogueSystem.Ports;
using DialogueSystem.Utilities;
using DialogueSystem.Window;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace DialogueSystem.Nodes
{
    public abstract class BaseDialogueNode : BaseNode
    {
        internal override void Initialize(DialogueSystemGraphView graphView, Vector2 position)
        {
            base.Initialize(graphView, position);
            Model.Text = "Dialogue text";
            Inputs.Add(new DialogueSystemInputModel(ID)
            {
                Text = "Connection",
            });
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

        protected override Port CreateOutputPort(object userData)
        {
            DialogueSystemOutputModel choiceData = userData as DialogueSystemOutputModel;

            BasePort choicePort = this.CreatePort(
                "",
                Orientation.Horizontal,
                Direction.Output,
                Port.Capacity.Single,
                type: typeof(string));

            Button deleteChoiceBtn = DialogueSystemUtilities.CreateButton(
                "X",
                () =>
                {
                    if (Outputs.Count == 1) return;
                    if (choicePort.connected)
                    {
                        var edges = choicePort.connections;
                        foreach (Edge edge in edges)
                        {
                            var input = edge.input.node as BaseNode;
                            var ouptut = edge.output.node as BaseNode;
                            input?.OnDestroyConnectionInput(edge.input as BasePort, edge);
                            ouptut?.OnDestroyConnectionOutput(edge.output as BasePort, edge);
                        }
                        graphView.DeleteElements(choicePort.connections);
                    }

                    Outputs.Remove(choiceData);
                    graphView.RemoveElement(choicePort);
                },
                styles: new string[]
                {
                    "ds-node__button"
                }
            );

            TextField choiceText = DialogueSystemUtilities.CreateTextField(
                (string)choiceData.Value,
                onChange: callback =>
                {
                    choiceData.Value = callback.newValue.ToString();
                },
                styles: new string[]
                    {
                        "ds-node__textfield",
                        "ds-node__choice-textfield",
                        "ds-node__textfield__hidden"
                    }
                );

            choicePort.Add(choiceText);
            choicePort.Add(deleteChoiceBtn);
            return choicePort;
        }
    }
}
