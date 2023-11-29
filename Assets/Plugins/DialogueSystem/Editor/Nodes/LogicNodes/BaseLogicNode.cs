using UnityEditor.Experimental.GraphView;
using System.Collections.Generic;
using DialogueSystem.Utilities;
using DialogueSystem.Window;
using DialogueSystem.Ports;
using UnityEngine;
using System.Linq;
using System;

namespace DialogueSystem.Nodes
{
    internal abstract class BaseLogicNode : BaseNode
    {
        internal override void Initialize(DSGraphView graphView, Vector2 position, List<object> portsContext)
        {
            base.Initialize(graphView, position, portsContext);

            if (portsContext == null)
            {
                Model.AddPort(new Database.Save.DSPortModel(new Type[]
                {
                    typeof(bool)
                }, Ports.PortSide.Output)
                {
                    Type = typeof(bool),
                    Cross = false,
                    IsField = false,
                    IsIfPort = false,
                    IsInput = false,
                    IsSingle = false,
                    PortText = DSConstants.Bool,
                    Value = "false",
                    IsFunction = true,
                });
            }
        }

        public override void OnConnectInputPort(BasePort _port, Edge edge)
        {
            base.OnConnectInputPort(_port, edge);

            List<BasePort> inputPorts = GetInputPorts();
            PortInfo[] portInfos = new PortInfo[inputPorts.Count];

            for (var i = 0; i < inputPorts.Count; i++)
                portInfos[i] = new PortInfo()
                {
                    node = this,
                    port = inputPorts[i],
                    Type = inputPorts[i].Type,
                    Value = inputPorts[i].Type.IsValueType == true ? Activator.CreateInstance(inputPorts[i].Type) : null
                };

            foreach (BasePort port in inputPorts)
            {
                if (port.connected)
                {
                    BasePort connectedPort = port.connections.First().output as BasePort;
                    if (connectedPort != null && connectedPort.Value != null)
                    {
                        ChangePortValueAndType(port, connectedPort.Type);
                        var infos = portInfos.Where(e => e.port == port).FirstOrDefault();
                        infos.Value = connectedPort.Value;
                        infos.Type = connectedPort.Type;
                    }
                }
                else if (!port.connected && port != _port)
                {
                    port.SetValue(DSUtilities.GetDefaultValue(port.Type));
                    var infos = portInfos.Where(e => e.port == port).FirstOrDefault();
                    infos.Value = port.Value;
                    infos.Type = port.Type;
                }
                else if (!port.connected && port == _port)
                {
                    BasePort connectedPort = edge.output as BasePort;
                    if (connectedPort != null && connectedPort.Value != null)
                    {
                        ChangePortValueAndType(port, connectedPort.Type);

                        var infos = portInfos.Where(e => e.port == port).FirstOrDefault();
                        infos.Value = connectedPort.Value;
                        infos.Type = connectedPort.Type;
                    }
                }
            }

            Do(portInfos);
        }

        public override void Do(PortInfo[] portInfos)
        {
            base.Do(portInfos);

            //BasePort output = GetOutputPorts()[0];

            //var isStr = portInfos.Any(e => e.port.Type == typeof(string));
            //ChangeOutputPortType(isStr == true ? typeof(string) : typeof(double));
        }
    }
}
