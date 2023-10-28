using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace DialogueSystem.Ports
{
    public class BasePort : Port
    {
        public object Value;
        public BasePort(Orientation portOrientation, Direction portDirection, Capacity portCapacity, Type type) : base(portOrientation, portDirection, portCapacity, type)
        {
            
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

            return port;
        }

        /*
        public static BasePort Create<TEdge>(Orientation orientation, Direction direction, Capacity capacity, Type type) where TEdge : Edge, new()
        {
            BasePort port = new BasePort(orientation, direction, capacity, type);

            Type listenerType = typeof(Port).Assembly.GetType("UnityEditor.Experimental.GraphView.DefaultEdgeConnectorListener");
            object listenerInstance = Activator.CreateInstance(listenerType);

            Type portType = typeof(BasePort);
            FieldInfo edgeConnectorField = portType.GetField("m_EdgeConnector", BindingFlags.NonPublic | BindingFlags.Instance);

            Type edgeConnectorType = typeof(EdgeConnector<>).MakeGenericType(typeof(TEdge));
            object edgeConnectorInstance = Activator.CreateInstance(edgeConnectorType, listenerInstance);



            port.AddManipulator(port.m_EdgeConnector);
            return port;
        }

        public static Port Create<TEdge>(Orientation orientation, Direction direction, Capacity capacity, Type type) where TEdge : Edge, new()
        {
            DefaultEdgeConnectorListener listener = new DefaultEdgeConnectorListener();
            Port port = new Port(orientation, direction, capacity, type)
            {
                m_EdgeConnector = new EdgeConnector<TEdge>(listener)
            };
            port.AddManipulator(port.m_EdgeConnector);
            return port;
        }
        */

    }
}
