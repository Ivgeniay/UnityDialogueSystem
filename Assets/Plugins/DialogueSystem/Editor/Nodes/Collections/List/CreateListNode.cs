using UnityEditor.Experimental.GraphView;
using DialogueSystem.Database.Save;
using System.Collections.Generic;
using DialogueSystem.Utilities;
using UnityEngine.UIElements;
using DialogueSystem.Window;
using DialogueSystem.Ports;
using UnityEngine;
using System.Linq;
using System;

namespace DialogueSystem.Nodes
{
    internal class CreateListNode : BaseListNode
    {
        internal override void Initialize(DSGraphView graphView, Vector2 position, List<object> context)
        {
            base.Initialize(graphView, position, context);
            if (context == null)
            {
                Model.AddPort(new DSPortModel(DSConstants.AvalilableTypes, PortSide.Input)
                {
                    PortText = "Element",
                    Cross = false,
                    IsField = false,
                    IsSingle = true,
                    IsInput = true,
                    Value = "",
                    IsAnchorable = true,
                });

                Model.AddPort(new DSPortModel(DSConstants.AvalilableTypes, PortSide.Output)
                {
                    PortText = "Collection",
                    Cross = false,
                    IsField = false,
                    IsSingle = false,
                    IsInput = false,
                    Value = "",
                    IsAnchorable = true,
                });
            }
        }

        protected override void DrawMainContainer(VisualElement container)
        {
            base.DrawMainContainer(container);

            Button addChoiceBtn = DSUtilities.CreateButton(
                "AddInput",
                () =>
                {
                    var t = AddPortByType(
                        ID: Guid.NewGuid().ToString(),
                        portText: "Element_" + this.Model.Inputs.Count,
                        type: typeof(float),
                        value: "",
                        isInput: true,
                        isSingle: true,
                        cross: true,
                        portSide: PortSide.Input,
                        availableTypes: DSConstants.AvalilableTypes,
                        isAnchorable: true);
                },
                styles: new string[] { "ds-node__button" }
            );
            container.Insert(1, addChoiceBtn);
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
            if (connectedPort != null && connectedPort.Value != null)
            {
                ChangePortValueAndType(_port, connectedPort.Type);
                PortInfo infos = portInfos.Where(e => e.port == _port).FirstOrDefault();
                infos.Value = connectedPort.Value;
                infos.Type = connectedPort.Type;
            }

            foreach (BasePort port in inputPorts)
            {
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

        public override void Do(PortInfo[] portInfos)
        {
            base.Do(portInfos);

            Type listType = typeof(List<>).MakeGenericType(portInfos[0].Type);
            ChangeOutputPortType(listType);
        }
    }
}