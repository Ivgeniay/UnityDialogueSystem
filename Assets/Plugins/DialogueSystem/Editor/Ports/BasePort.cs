using UnityEditor.Experimental.GraphView;
using DialogueSystem.Manipulations;
using DialogueSystem.Abstract;
using UnityEngine.UIElements;
using DialogueSystem.Window;
using DialogueSystem.Edges;
using System.Reflection;
using UnityEngine;
using System;
using DialogueSystem.Nodes;
using System.Linq;
using DialogueSystem.Database.Save;

namespace DialogueSystem.Ports
{
    public class BasePort : Port, IDataHolder
    {
        public string ID { get; set; }
        public bool IsFunctions { get; set; }
        public BasePort IfPortSource { get; set; }
        public object Value { get; private set; }
        public Type Type { get; private set; }
        public string Name { get; private set; }
        public bool IsSerializedInScript { get; set; }
        public DSGraphView GrathView { get; internal set; }

        public string Anchor = string.Empty;

        public Type[] AvailableTypes;
        public PortSide PortSide;
        public bool IsAnchorable = false;
        private Color defaultColor;
        
        public BasePort(Orientation portOrientation, Direction portDirection, Capacity portCapacity, Type type) : base(portOrientation, portDirection, portCapacity, type) { }
        public static BasePort CreateBasePort<TEdge>(Orientation orientation, Direction direction, Capacity capacity, Type type) where TEdge : DSEdge, new()
        {
            Type edgeConnectorType = typeof(CustomEdgeConnector<>).MakeGenericType(typeof(TEdge));
            Type outerType = typeof(Port);
            Type innerType = outerType.GetNestedType("DefaultEdgeConnectorListener", BindingFlags.NonPublic);

            object listenerInstance = Activator.CreateInstance(innerType);
            object edgeConnectorInstance = Activator.CreateInstance(edgeConnectorType, listenerInstance);

            BasePort port = new BasePort(orientation, direction, capacity, type);

            Type Type = typeof(BasePort);
            FieldInfo edgeConnectorField = Type.GetField("m_EdgeConnector", BindingFlags.NonPublic | BindingFlags.Instance);

            if (edgeConnectorField != null)
                edgeConnectorField.SetValue(port, edgeConnectorInstance);

            port.AddManipulators();
            port.defaultColor = port.portColor;
            return port;
        }

        internal void OnDistroy()
        {
            if (!string.IsNullOrWhiteSpace(Anchor))
            {
                GrathView?.RemoveAnchor(this);
                BaseNode node = this.node as BaseNode;
                DSPortModel model = null;
                switch (PortSide)
                {
                    case PortSide.Input:
                        model = node.Model.Inputs.FirstOrDefault(e => e.PortID == ID);
                        break;
                    case PortSide.Output:
                        model = node.Model.Outputs.FirstOrDefault(e => e.PortID == ID);
                        break;
                }
                if (model != null) model.Anchor = string.Empty;
            }
        }

        public override void Connect(Edge edge)
        {
            base.Connect(edge);
            if (edge.output != null && edge.input == this)
            {
                var other = edge.output as BasePort;
                if (!BasePortManager.HaveCommonTypes(AvailableTypes, other.AvailableTypes))
                {
                    Disconnect(edge);
                }
            }
        }
        public override void Disconnect(Edge edge)
        {
            base.Disconnect(edge);

            if (edge != null)
            {
                GraphView graphView = GetFirstAncestorOfType<GraphView>();
                if (graphView != null)
                {
                    var t = new GraphViewChange();
                    t.elementsToRemove = new() { edge };
                    graphView.graphViewChanged?.Invoke(t);
                    graphView.RemoveElement(edge);
                }
            }
        } 
        public override void OnStartEdgeDragging()
        {
            if (this.m_EdgeConnector?.edgeDragHelper?.draggedPort == this)
                BasePortManager.CallStartDrag(this);
            base.OnStartEdgeDragging();
        }
        public override void OnStopEdgeDragging()
        {
            base.OnStopEdgeDragging();
            
            if (this.m_EdgeConnector?.edgeDragHelper?.draggedPort == this)
                BasePortManager.CallStopDrag(this);
        }

        private void AddManipulators()
        {
            this.AddManipulator(this.m_EdgeConnector);
            StartDragManipulator startDrag = new StartDragManipulator(this);
            this.AddManipulator(startDrag);

            this.AddManipulator(CreateContextualMenu());
        }
        internal void AddOrUpdateAnchor(string anchorName)
        {
            Anchor = anchorName;
            BaseNode node = this.node as BaseNode;
            DSPortModel model = null;
            switch (PortSide)
            {
                case PortSide.Input:
                    model = node.Model.Inputs.FirstOrDefault(e => e.PortID == ID);
                    break;
                case PortSide.Output:
                    model = node.Model.Outputs.FirstOrDefault(e => e.PortID == ID);
                    break;
            }
            if (model != null) model.Anchor = anchorName;
            GrathView?.AddOrUpdateAnchor(this, anchorName);
            if (string.IsNullOrWhiteSpace(Anchor))
            {
                this.portColor = defaultColor;
                this.tooltip = null;
            }
            else
            {
                if (ColorUtility.TryParseHtmlString("#FDD057", out Color col)) this.portColor = col;
                else this.portColor = Color.yellow;
                this.tooltip = Anchor;
            }
        }

        internal void SetValue(object value) => Value = value; 
        internal void SetPortType(Type type)
        {
            Type = type;
            portType = type;
        } 
        internal void ChangeName(string name)
        {
            portName = name;
            Name = name;
        }

        private IManipulator CreateContextualMenu()
        {
            ContextualMenuManipulator contextualMenuManipulator = new(e =>
            {
                if (IsAnchorable)
                {
                    e.menu.AppendAction("Anchor", a => { DSAnchorWindow.OpenWindow(this); });
                }
            });
            return contextualMenuManipulator;
        }
    }
}
