using System;
using System.Collections.Generic;
using System.Linq;
using DialogueSystem.Database.Save;
using DialogueSystem.Ports;
using DialogueSystem.Utilities;
using DialogueSystem.Window;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace DialogueSystem.Nodes
{
    internal class AddToListNode : BaseListNode
    {
        internal override void Initialize(DSGraphView graphView, Vector2 position, List<object> context)
        {
            base.Initialize(graphView, position, context);
            if (context == null)
            {
                Model.AddPort(new DSPortModel(DSConstants.CollectionsTypes, Ports.PortSide.Input)
                {
                    PortText = "EnterCollection",
                    Type = typeof(List<int>),
                    Cross = false,
                    IsField = false,
                    IsSingle = true,
                    IsInput = true,
                    Value = "",
                    IsAnchorable = true,
                });

                Model.AddPort(new DSPortModel(DSConstants.AvalilableTypes, Ports.PortSide.Input)
                {
                    PortText = "Element",
                    Type = typeof(int),
                    Cross = false,
                    IsField = false,
                    IsSingle = true,
                    IsInput = true,
                    Value = "",
                    IsAnchorable = true,
                });

                Model.AddPort(new DSPortModel(DSConstants.CollectionsTypes, Ports.PortSide.Output)
                {
                    PortText = "ExitCollection",
                    Type = typeof(List<int>),
                    Cross = false,
                    IsField = false,
                    IsSingle = true, 
                    Value = "",
                    IsAnchorable = true,
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
                    Value = inputPorts[i].Type.IsValueType == true ? Activator.CreateInstance(inputPorts[i].Type) : ""
                };

            BasePort connectedPort = edge.output as BasePort;
            Type type = connectedPort.Type;
            if (connectedPort != null && connectedPort.Value != null)
            {
                ChangePortValueAndType(_port, connectedPort.Type);
                PortInfo infos = portInfos.Where(e => e.port == _port).FirstOrDefault();
                infos.Value = connectedPort.Value;
                infos.Type = connectedPort.Type;
            }

            for (int i = 0; i < inputPorts.Count; i++)
            {
                BasePort port = inputPorts[i];
                if (port != _port)
                {
                    if (port.Type != connectedPort.Type) port.DisconnectAll();
                    ChangePortValueAndType(port, connectedPort.Type);
                    PortInfo infos = portInfos.Where(e => e.port == port).FirstOrDefault();
                    infos.Value = connectedPort.Value;
                    infos.Type = connectedPort.Type;
                }
            }
            Do(portInfos);
        }
    }
}
