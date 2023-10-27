using UnityEditor.Experimental.GraphView;
using System.Collections.Generic;
using DialogueSystem.Utilities;
using DialogueSystem.Dialogue;
using UnityEngine.UIElements;
using DialogueSystem.Groups;
using DialogueSystem.Window;
using UnityEngine;
using System.Linq;

namespace DialogueSystem.Nodes
{
    public class BaseNode : Node
    {
        public string DialogueName { get; set; }
        public List<string> Choises { get; set; }
        public string Text { get; set; }
        public DialogueType DialogueType { get; set; }

        private Color defaultbackgroundColor;
        protected DialogueSystemGraphView graphView;
        public BaseGroup Group { get; set; }

        internal virtual void Initialize(DialogueSystemGraphView graphView, Vector2 position)
        {
            this.graphView = graphView;

            DialogueName = "Dialogue Name";
            Choises = new List<string>();
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
                    if (Group is null)
                    {
                        graphView.RemoveUngroupedNode(this);
                        DialogueName = callback.newValue;
                        graphView.AddUngroupedNode(this);
                    }
                    else
                    {
                        BaseGroup currentGroup = Group;
                        graphView.RemoveGroupedNode(Group, this);
                        DialogueName = callback.newValue;
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

        //internal bool IsMyPort(Port port) => inputContainer.Children().Any(e => e == port) || outputContainer.Children().Any(e => e == port);
        //internal virtual void OnDisconectedPort(Port port) 
        //{
        //    Debug.Log(port);
        //}
        public virtual void OnCreate() {}
        public virtual void OnDestroy() {}
        public virtual void OnGroupUp(BaseGroup group) {}
        public virtual void OnUnGroup(BaseGroup group) {}
    }
}
