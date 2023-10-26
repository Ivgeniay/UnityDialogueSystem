using DialogueSystem.Dialogue;
using DialogueSystem.Utilities;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace DialogueSystem.Nodes
{
    public class BaseNode : Node
    {
        public string DialogueName { get; set; }
        public List<string> Choises { get; set; }
        public string Text { get; set; }
        public DialogueType DialogueType { get; set; }

        internal virtual void Initialize(Vector2 position)
        {
            DialogueName = "Dialogue Name";
            Choises = new List<string>();
            Text = "Dialogue text";

            this.SetPosition(new Rect(position, Vector2.zero));
            AddStyles();
        }

        private void AddStyles()
        {
            mainContainer.AddToClassList("ds-node__main-container");
            extensionContainer.AddToClassList("ds-node__extension-container");
        }

        protected virtual void DrawTitleContainer()
        {
            TextField dialogueNameTF = DialogueSystemUtilities.CreateTextField(
                DialogueName,
                styles: new string[]
                    {
                        "ds-node__textfield",
                        "ds-node__filename-textfield",
                        "ds-node__textfield__hidden"
                    }
                );

            titleContainer.Insert(0, dialogueNameTF);
        }
        protected virtual void DrawInputOutputContainer()
        {
            Port inputPort = this.CreatePort(
                "Dialogue Connection", 
                Orientation.Horizontal,
                Direction.Input,
                Port.Capacity.Multi,
                type: typeof(bool)
                );
            inputContainer.Add(inputPort);
        }
        protected virtual void DrawMainContainer()
        {

        }
        protected virtual void DrawExtensionContainer()
        {
            VisualElement customDataContainer = new VisualElement();
            customDataContainer.AddToClassList("ds-node__custom-data-container");

            Foldout textFolout = DialogueSystemUtilities.CreateFoldout("DialogueText", true);
            TextField textField = DialogueSystemUtilities.CreateTextArea(
                Text,
                styles: new string[]
                    {
                        "ds-node__textfield",
                        "ds-node__quote-textfield"
                    }
                );

            textFolout.Add(textField);
            customDataContainer.Add(textFolout);
            extensionContainer.Add(customDataContainer);
        }

        internal virtual void Draw()
        {
            DrawTitleContainer();
            DrawMainContainer();
            DrawInputOutputContainer();
            DrawExtensionContainer();

            RefreshExpandedState();
        }


    }
}
