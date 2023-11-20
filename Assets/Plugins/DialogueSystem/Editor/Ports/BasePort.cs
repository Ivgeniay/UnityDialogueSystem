using UnityEditor.Experimental.GraphView;
using DialogueSystem.Manipulations;
using UnityEngine.UIElements;
using DialogueSystem.Edges;
using System.Reflection;
using UnityEngine;
using System;
using DialogueSystem.Abstract;

namespace DialogueSystem.Ports
{
    public class BasePort : Port, IDataHolder
    {
        public string ID { get; set; }
        public bool IsFunctions { get; set; }
        public object Value { get; private set; }
        public Type Type { get; private set; }
        public string Name { get; private set; }
        public bool IsSerializedInScript { get; set; }
        public PortSide PortSide;


        public Type[] AvailableTypes;
        public Color capColor;
        
        public BasePort(Orientation portOrientation, Direction portDirection, Capacity portCapacity, Type type) : base(portOrientation, portDirection, portCapacity, type) 
        { }
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

            port.AddManipulator(port.m_EdgeConnector);

            StartDragManipulator startDrag= new StartDragManipulator(port);
            port.AddManipulator(startDrag);

            return port;
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
                
                //edge.parent.Remove(edge);
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


        internal void SetValue(object value)
        {
            Value = value;
        }

        internal void SetPortType(Type type)
        {
            Type = type;
            Type = type;
        }

        internal void ChangeName(string name)
        {
            portName = name;
            Name = name;
        }
    }
    public enum PortSide
    {
        Input,
        Output,
    }
}
