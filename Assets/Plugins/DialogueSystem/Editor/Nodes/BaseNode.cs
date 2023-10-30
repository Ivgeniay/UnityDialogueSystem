using UnityEditor.Experimental.GraphView;
using DialogueSystem.Database.Save;
using System.Collections.Generic;
using DialogueSystem.Utilities;
using UnityEngine.UIElements;
using DialogueSystem.Groups;
using DialogueSystem.Window;
using DialogueSystem.Ports;
using DialogueSystem.Text;
using UnityEngine;
using System;
using System.Linq;
using UnityEngine.Windows;

namespace DialogueSystem.Nodes
{
    public class BaseNode : Node
    {
        public List<DialogueSystemOutputModel> Outputs { get; set; }
        public List<DialogueSystemInputModel> Inputs { get; set; }
        public DialogueSystemNodeModel Model { get; private set; }
        public BaseGroup Group { get; private set; }
        protected TextField titleTF { get; set; }
        public string ID { get; protected set; }

        protected DialogueSystemGraphView graphView { get; set; }
        private Color defaultbackgroundColor;

        internal virtual void Initialize(DialogueSystemGraphView graphView, Vector2 position)
        {
            defaultbackgroundColor = new Color(29f/255f, 29f/255f, 30f/255f);
            Outputs = new List<DialogueSystemOutputModel>();
            Inputs = new List<DialogueSystemInputModel>();

            this.graphView = graphView;
            ID = Guid.NewGuid().ToString();

            Model = new()
            {
                ID = ID,
                NodeName = "NodeName",
                DialogueType = this.GetType(),
            };

            this.SetPosition(new Rect(position, Vector2.zero));
            AddStyles();
        }
        private void AddStyles()
        {
            mainContainer.AddToClassList("ds-node__main-container");
            extensionContainer.AddToClassList("ds-node__extension-container");
        }

        #region Draw
        protected virtual void DrawTitleContainer(VisualElement container)
        {
            titleTF = DialogueSystemUtilities.CreateTextField(
                Model.NodeName,
                null,
                callback =>
                {
                    TextField target = callback.target as TextField;
                    target.value = callback.newValue.RemoveWhitespaces().RemoveSpecialCharacters();

                    if (Group is null)
                    {
                        graphView.RemoveUngroupedNode(this);
                        Model.NodeName = target.value;
                        graphView.AddUngroupedNode(this);
                    }
                    else
                    {
                        BaseGroup currentGroup = Group;
                        graphView.RemoveGroupedNode(Group, this);
                        Model.NodeName = target.value;
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

            container.Insert(0, titleTF);
        }
        protected virtual void DrawInputContainer(VisualElement container)
        {
            foreach (var input in Inputs)
            {
                Port choicePort = CreateInputPort(input);
                container.Add(choicePort);
            }
        }
        protected virtual void DrawOutputContainer(VisualElement container) 
        {
            foreach (DialogueSystemOutputModel choice in Outputs)
            {
                Port choicePort = CreateOutputPort(choice);
                container.Add(choicePort);
            }
        }
        protected virtual void DrawMainContainer(VisualElement container)
        {

        }
        protected virtual void DrawExtensionContainer(VisualElement container) {}
        protected virtual void Draw()
        {
            DrawTitleContainer(titleContainer);
            DrawMainContainer(mainContainer);
            DrawInputContainer(inputContainer);
            DrawOutputContainer(outputContainer);
            DrawExtensionContainer(extensionContainer);

            RefreshExpandedState();
        }
        protected virtual BasePort CreateInputPort(object userData)
        {
            DialogueSystemInputModel choiceData = userData as DialogueSystemInputModel;

            BasePort inputPort = this.CreatePort(
                choiceData.PortText,
                Orientation.Horizontal,
                Direction.Input,
                Port.Capacity.Multi);

            return inputPort;
        }
        protected virtual BasePort CreateOutputPort(object userData)
        {
            return null;
        }
        #endregion

        #region Overrided
        public override Port InstantiatePort(Orientation orientation, Direction direction, Port.Capacity capacity, Type type) =>
            BasePort.CreateBasePort<Edge>(orientation, direction, capacity, type);
        public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
        {
            evt.menu.AppendAction("Disconnect Input Ports", e => DisconnectInputPorts());
            evt.menu.AppendAction("Disconnect Output Ports", e => DisconnectOutputPorts());
            base.BuildContextualMenu(evt);
        }
        #endregion

        #region Style
        internal virtual void SetErrorStyle(Color color) => mainContainer.style.backgroundColor = color;
        internal virtual void ResetStyle() => mainContainer.style.backgroundColor = defaultbackgroundColor;
        #endregion

        #region Utilits
        internal void DisconnectAllPorts()
        {
            DisconnectInputPorts();
            DisconnectOutputPorts();
        }
        private void DisconnectInputPorts() => DisconnectPort(inputContainer);
        private void DisconnectOutputPorts() => DisconnectPort(outputContainer);
        private void DisconnectPort(VisualElement container)
        {
            foreach (Port port in container.Children())
            {
                if (port.connected)
                {
                    graphView.DeleteElements(port.connections);
                }
            }
        }
        protected List<BasePort> GetInputPorts() => inputContainer.Children().Cast<BasePort>().ToList();
        protected List<BasePort> GetOutputPorts() => outputContainer.Children().Cast<BasePort>().ToList();
        public DialogueSystemNodeModel GetConnections()
        {
            return null;
        }

        #endregion

        #region MonoEvents
        public virtual void OnConnectOutputPort(BasePort port, Edge edge)
        {
            if (Outputs != null && Outputs.Count > 0)
            {
                var output1 = edge.output.node as BaseNode;
                var tt = Outputs.Where(el => string.IsNullOrEmpty(el.NodeID)).FirstOrDefault();
                if(tt != null) tt.NodeID = output1.Model.ID;
            }
        }
        public virtual void OnConnectInputPort(BasePort port, Edge edge)
        {
            if (Inputs != null && Inputs.Count > 0)
            {
                var output1 = edge.output.node as BaseNode;
                var tt = Inputs.Where(el => el.PortText == port.portName).FirstOrDefault();
                if (tt != null) tt.NodeID = output1.Model.ID;
            }
        }
        public virtual void OnDestroyConnectionOutput(BasePort port, Edge edge)
        {
            if (Outputs != null && Outputs.Count > 0)
            {
                var outputNode = edge.output.node as BaseNode;
                var tt = Outputs.Where(el => el.NodeID == outputNode.Model.ID).FirstOrDefault();
                if (tt != null)
                    tt.NodeID = string.Empty;
            }
        }
        public virtual void OnDestroyConnectionInput(BasePort port, Edge edge)
        {
            if (Inputs != null && Inputs.Count > 0)
            {
                var outputNode = edge.output.node as BaseNode;
                var tt = Inputs.Where(el => el.NodeID == outputNode.Model.ID).FirstOrDefault();
                if (tt != null)
                    tt.NodeID = string.Empty;
            }
        }

        public virtual void OnChangePosition(Vector2 position, Vector2 delta)
        {
            Model.position = position;
        }
        public virtual void OnCreate() => Draw();
        public virtual void OnDestroy() 
        {

        }
        public virtual void OnGroupUp(BaseGroup group)
        {
            Model.GroupID = group.Model.ID;
            Group = group;
        }
        public virtual void OnUnGroup(BaseGroup group)
        {
            Model.GroupID = "";
            Group = null;
        }

        public virtual void OnUpdate()
        {

        }
        #endregion
    }
}
