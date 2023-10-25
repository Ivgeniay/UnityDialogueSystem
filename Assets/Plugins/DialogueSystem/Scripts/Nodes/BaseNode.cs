using DialogueSystem.Dialogue;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace DialogueSystem.Nodes
{
    internal class BaseNode : Node
    {
        //[field: SerializeField]
        public string DialogueName { get; set; }
        public List<string> Choises { get; set; }
        public string Text { get; set; }
        public DialogueType DialogueType { get; set; }

        public void Initialize()
        {
            DialogueName = "Dialogue Name";
            Choises = new List<string>();
            Text = "Dialogue text";

        }

        public void Draw()
        {
            //Title container
            TextField dialogueNameTF = new TextField()
            {
                value = DialogueName,
            };

            titleContainer.Insert(0, dialogueNameTF);

            //Input conteiner
            Port inputPort = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Multi, typeof(bool));
            inputPort.portName = "Dialogue Connection";
            inputContainer.Add(inputPort);

            //Extension Container
            VisualElement customDataContainer = new VisualElement();
            Foldout textFolout = new()
            {
                text = "DialogueText",
                value = true
            };
            TextField textField = new()
            {
                value = Text,
            };

            textFolout.Add(textField);
            customDataContainer.Add(textFolout);
            extensionContainer.Add(customDataContainer);

            RefreshExpandedState();
        }


    }
}
