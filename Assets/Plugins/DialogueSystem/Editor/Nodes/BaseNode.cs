using UnityEditor.Experimental.GraphView;
using System.Collections.Generic;
using DialogueSystem.Utilities;
using DialogueSystem.Dialogue;
using UnityEngine.UIElements;
using DialogueSystem.Groups;
using DialogueSystem.Window;
using UnityEngine;
using System.Linq;
using DialogueSystem.Text;
using System;
using DialogueSystem.Database.Save;

namespace DialogueSystem.Nodes
{
    public class BaseNode : Node
    {
        public string ID { get; set; }
        public string DialogueName { get; set; }
        public List<DialogueSystemChoiceData> Choises { get; set; }
        public string Text { get; set; }
        public DialogueType DialogueType { get; set; }

        private Color defaultbackgroundColor;
        protected DialogueSystemGraphView graphView;
        public BaseGroup Group { get; set; }

        internal virtual void Initialize(DialogueSystemGraphView graphView, Vector2 position)
        {
            ID = Guid.NewGuid().ToString();

            this.graphView = graphView;
            DialogueName = "DialogueName";
            Choises = new List<DialogueSystemChoiceData>();
            Text = "Dialogue text";

            defaultbackgroundColor = new Color(29f/255f, 29f/255f, 30f/255f);

            this.SetPosition(new Rect(position, Vector2.zero));
            AddStyles();
        }

        private void AddStyles()
        {
            mainContainer.AddToClassList("ds-node__main-container");
            extensionContainer.AddToClassList("ds-node__extension-container");
        }

        #region Draw
        protected virtual void DrawTitleContainer()
        {
            TextField dialogueNameTF = DialogueSystemUtilities.CreateTextField(
                DialogueName,
                null,
                callback =>
                {
                    TextField target = callback.target as TextField;
                    target.value = callback.newValue.RemoveWhitespaces().RemoveSpecialCharacters();

                    if (Group is null)
                    {
                        graphView.RemoveUngroupedNode(this);
                        DialogueName = target.value;
                        graphView.AddUngroupedNode(this);
                    }
                    else
                    {
                        BaseGroup currentGroup = Group;
                        graphView.RemoveGroupedNode(Group, this);
                        DialogueName = target.value;
                        graphView.AddGroupNode(currentGroup, this);
                    }
                },
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
        #endregion

        #region Overrided
        public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
        {
            evt.menu.AppendAction("Disconnect Input Ports", e => DisconectInputPorts());
            evt.menu.AppendAction("Disconnect Output Ports", e => DisconectOutputPorts());
            evt.menu.AppendAction("Disconnect All", e => DisconectAllPorts());
            base.BuildContextualMenu(evt);
        }
        #endregion

        #region Style
        internal void SetErrorStyle(Color color)
        {
            mainContainer.style.backgroundColor = color;
        }
        internal void ResetStyle()
        {
            mainContainer.style.backgroundColor = defaultbackgroundColor;
        }
        #endregion

        #region Utilits
        internal void DisconectAllPorts()
        {
            DisconectInputPorts();
            DisconectOutputPorts();
        }
        internal void DisconectInputPorts()
        {
            DisconectPort(inputContainer);
        }
        internal void DisconectOutputPorts()
        {
            DisconectPort(outputContainer);
        }
        internal void DisconectPort(VisualElement container)
        {
            foreach (Port port in container.Children())
            {
                if (port.connected)
                {
                    //OnDisconectedPort(port);
                    graphView.DeleteElements(port.connections);
                }
            }
        }
        #endregion

        #region Mono
        public virtual void OnConnectOutputPort(Port port, Edge edge)
        {
            Debug.Log($"On output {this.DialogueName}");
        }
        public virtual void OnConnectInputPort(Port port, Edge edge)
        {
            Debug.Log($"On input {this.DialogueName}");
        }

        public virtual void OnDestroyConnectionOutput(Port port, Edge edge)
        {
            Debug.Log($"OnDestroyOutput {this.DialogueName}");
        }
        public virtual void OnDestroyConnectionInput(Port port, Edge edge)
        {
            Debug.Log($"OnDestroyInput {this.DialogueName}");
        }

        public virtual void OnChangePosition(Vector2 position){}
        public virtual void OnCreate() {}
        public virtual void OnDestroy() {}
        public virtual void OnGroupUp(BaseGroup group) {}
        public virtual void OnUnGroup(BaseGroup group) {}
        #endregion
    }
}
