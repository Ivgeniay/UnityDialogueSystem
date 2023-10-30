using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;
using System.Reflection;
using System;
using UnityEngine;

namespace DialogueSystem.Ports
{
    public class BasePort : Port
    {
        public object Value;
        
        public BasePort(Orientation portOrientation, Direction portDirection, Capacity portCapacity, Type type) : base(portOrientation, portDirection, portCapacity, type) 
        { }

        public override void OnStopEdgeDragging()
        {
            base.OnStopEdgeDragging();
            //Debug.Log($"Stop drug {this}");
        }

        public static BasePort CreateBasePort<TEdge>(Orientation orientation, Direction direction, Capacity capacity, Type type) where TEdge : Edge, new()
        {
            Type edgeConnectorType = typeof(EdgeConnector<>).MakeGenericType(typeof(TEdge));
            Type outerType = typeof(Port);
            Type innerType = outerType.GetNestedType("DefaultEdgeConnectorListener", BindingFlags.NonPublic);

            object listenerInstance = Activator.CreateInstance(innerType);
            object edgeConnectorInstance = Activator.CreateInstance(edgeConnectorType, listenerInstance);

            BasePort port = new BasePort(orientation, direction, capacity, type);

            Type portType = typeof(BasePort);
            FieldInfo edgeConnectorField = portType.GetField("m_EdgeConnector", BindingFlags.NonPublic | BindingFlags.Instance);

            if (edgeConnectorField != null)
                edgeConnectorField.SetValue(port, edgeConnectorInstance);

            port.AddManipulator(port.m_EdgeConnector);
            //StartDragManipulator startDrag= new StartDragManipulator();
            //port.AddManipulator(startDrag);

            return port;
        }

        internal void ChangeType(Type type)
        {
            portType = type;
        }

        internal void ChangeName(string name)
        {
            portName = name;
        }
    }
}
