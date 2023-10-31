using DialogueSystem.Nodes;
using DialogueSystem.Ports;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;

namespace DialogueSystem.Nodes
{
    internal abstract class BaseLogicNode : BaseNode
    {
        public override void OnConnectInputPort(BasePort _port, Edge edge)
        {
            base.OnConnectInputPort(_port, edge);

            var inpPorts = GetInputPorts();
            List<object> values = new List<object>();
            foreach (BasePort port in inpPorts)
            {
                if (port.connected)
                {
                    BasePort connectedPort = port.connections.First().output as BasePort;
                    if (connectedPort != null && connectedPort.Value != null) values.Add(connectedPort.Value);
                }
                if (!port.connected && port == _port)
                {
                    BasePort connectedPort = edge.output as BasePort;
                    if (connectedPort != null && connectedPort.Value != null) values.Add(connectedPort.Value);
                }
            }
            if (values.Count > 0)
            {
                Do(values);
            }
        }
    }
}
