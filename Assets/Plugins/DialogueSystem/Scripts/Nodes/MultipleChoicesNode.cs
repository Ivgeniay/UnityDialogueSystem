using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace DialogueSystem.Nodes
{
    public class MultipleChoicesNode : BaseNode
    {
        internal override void Initialize(Vector2 position)
        {
            base.Initialize(position);

            DialogueType = Dialogue.DialogueType.MultipleChoice;
            Choises.Add("New Choice");
        }

        protected override void DrawMainContainer()
        {
            base.DrawMainContainer();
            Button addChoiceBtn = new Button()
            {
                text = "Add Choice"
            };

            addChoiceBtn.AddToClassList("ds-node__button");

            mainContainer.Insert(1, addChoiceBtn);
        }

        protected override void DrawInputOutputContainer()
        {
            base.DrawInputOutputContainer();

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
                    value = choice,
                    //style =
                    //{
                        //width = choicePort.contentContainer.layout.width - 50,
                        //height = 12
                    //}
                };

                choiceText.AddToClassList("ds-node__textfield");
                choiceText.AddToClassList("ds-node__choice-textfield");
                choiceText.AddToClassList("ds-node__textfield__hidden");

                deleteChoiceBtn.AddToClassList("ds-node__button");

                choicePort.Add(choiceText);
                choicePort.Add(deleteChoiceBtn);

                outputContainer.Add(choicePort);
            }

        }

        //internal override void Draw()
        //{
        //    //base.DrawTitleContainer();
        //    //DrawInputOutputContainer();
        //    //base.DrawExtensionContainer();
        //    base.Draw();
        //    RefreshExpandedState();
        //}
    }
}
