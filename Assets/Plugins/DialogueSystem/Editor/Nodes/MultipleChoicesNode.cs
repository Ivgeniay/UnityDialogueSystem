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
            Choises.Add("New Choice");
        }

        protected override void DrawMainContainer()
        {
            base.DrawMainContainer();
            Button addChoiceBtn = DialogueSystemUtilities.CreateButton(
                "Add Choice", 
                () =>
                {
                    Port choicePort = CreateChoicePort("New Choice");
                    Choises.Add("New Choice");
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

            foreach (var choice in Choises)
            {
                Port choicePort = CreateChoicePort(choice);
                outputContainer.Add(choicePort);
            }

        }

        #region ElementsCreation
        private Port CreateChoicePort(string choice)
        {
            Port choicePort = this.CreatePort(
            "",
            Orientation.Horizontal,
            Direction.Output,
            Port.Capacity.Single,
            type: typeof(bool)
        );
            Button deleteChoiceBtn = DialogueSystemUtilities.CreateButton(
                "X",
                styles: new string[]
                {
                        "ds-node__button"
                }
            );
            TextField choiceText = DialogueSystemUtilities.CreateTextField(
                choice,
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
