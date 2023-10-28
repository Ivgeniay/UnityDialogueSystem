using DialogueSystem.Database.Save;
using DialogueSystem.Utilities;
using DialogueSystem.Window;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace DialogueSystem.Nodes
{
    internal class MultipleChoicesNode : BaseNode
    {
        internal override void Initialize(DialogueSystemGraphView graphView, Vector2 position)
        {
            base.Initialize(graphView, position);

            DialogueType = Dialogue.DialogueType.MultipleChoice;
            Choises.Add(new DialogueSystemChoiceData()
            {
                NodeID = ID,
                Text = "Next Choice"
            });
        }

        protected override void DrawMainContainer()
        {
            base.DrawMainContainer();
            DialogueSystemChoiceData choiceData = new DialogueSystemChoiceData()
            {
                NodeID = ID,
                Text = "Next Choice"
            };
            Button addChoiceBtn = DialogueSystemUtilities.CreateButton(
                "Add Choice", 
                () =>
                {
                    Port choicePort = CreateChoicePort(choiceData);
                    Choises.Add(choiceData);
                    outputContainer.Add(choicePort);
                },
                styles: new string[]
                {
                    "ds-node__button"
                }
            );

            mainContainer.Insert(1, addChoiceBtn);
        }
        protected override void DrawInputOutputContainer()
        {
            base.DrawInputOutputContainer();

            foreach (DialogueSystemChoiceData choice in Choises)
            {
                Port choicePort = CreateChoicePort(choice);
                outputContainer.Add(choicePort);
            }

        }

        #region ElementsCreation
        private Port CreateChoicePort(object userData)
        {
            Port choicePort = this.CreatePort(
            "",
            Orientation.Horizontal,
            Direction.Output,
            Port.Capacity.Single,
            type: typeof(bool));

            choicePort.userData = userData;
            DialogueSystemChoiceData choiceData = userData as DialogueSystemChoiceData;

            Button deleteChoiceBtn = DialogueSystemUtilities.CreateButton(
                "X",
                () =>
                {
                    if (Choises.Count == 1) return;
                    if (choicePort.connected)
                    {
                        var edges = choicePort.connections;
                        foreach (Edge edge in edges)
                        {
                            var input = edge.input.node as BaseNode;
                            var ouptut = edge.output.node as BaseNode;
                            input?.OnDestroyConnectionInput(edge.input, edge);
                            ouptut?.OnDestroyConnectionOutput(edge.output, edge);
                        }
                        graphView.DeleteElements(choicePort.connections);
                    }

                    Choises.Remove(choiceData);
                    graphView.RemoveElement(choicePort);
                },
                styles: new string[]
                {
                    "ds-node__button"
                }
            );
            TextField choiceText = DialogueSystemUtilities.CreateTextField(
                choiceData.Text,
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
        #endregion

    }
}
