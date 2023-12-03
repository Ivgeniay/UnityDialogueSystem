﻿using DialogueSystem.Database.Save;
using DialogueSystem.Generators;
using DialogueSystem.Ports;
using DialogueSystem.Utilities;
using DialogueSystem.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace DialogueSystem.Nodes
{
    internal class ActionNode : BaseActionNode
    {
        internal override void Initialize(DSGraphView graphView, Vector2 position, List<object> context)
        {
            base.Initialize(graphView, position, context);

            if (context == null)
            {
                Model.AddPort(new DSPortModel(DSConstants.DialogueTypes, Ports.PortSide.Input)
                {
                    PortText = DSConstants.Dialogue,
                });

                Model.AddPort(new DSPortModel(DSConstants.DialogueTypes, Ports.PortSide.Output)
                {
                    PortText = DSConstants.Dialogue,
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
                    (BasePort port, DSPortModel model) t = AddPortByType(
                        ID: Guid.NewGuid().ToString(),
                        portText: DSConstants.Double,
                        type: typeof(double),
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

            Pill badge = new Pill();
            badge.text = "PIIIIIIIL";
            container.Add(badge);
        }

        public override void OnConnectInputPort(BasePort port, Edge edge)
        {
            base.OnConnectInputPort(port, edge);
            BasePort connectedPort = edge.output as BasePort;
            bool continues = BasePortManager.HaveCommonTypes(connectedPort.Type, port.AvailableTypes);
            if (!continues) return;

            List<BasePort> inputPorts = GetInputPorts().ToList();
            PortInfo[] portInfos = new PortInfo[inputPorts.Count];

            for (var i = 0; i < inputPorts.Count; i++)
                portInfos[i] = new PortInfo()
                {
                    node = this,
                    port = inputPorts[i],
                    Type = inputPorts[i].Type,
                    Value = inputPorts[i].Type.IsValueType == true ? Activator.CreateInstance(inputPorts[i].Type) : ""
                };

            if (connectedPort != null && connectedPort.Value != null && port != inputPorts[0])
            { 
                ChangePortTypeAndName(port, connectedPort.Type);
                PortInfo infos = portInfos.Where(e => e.port == port).FirstOrDefault();
                infos.Value = connectedPort.Value;
                infos.Type = connectedPort.Type;
            }
            for (int i = 1; i < inputPorts.Count; i++)
            {
                if (inputPorts[i] != port && !inputPorts[i].connected)
                {
                    if (inputPorts[i].Type != connectedPort.Type) inputPorts[i].DisconnectAll();
                    ChangePortTypeAndName(inputPorts[i], connectedPort.Type);
                    PortInfo infos = portInfos.Where(e => e.port == inputPorts[i]).FirstOrDefault();
                    infos.Value = connectedPort.Value;
                    infos.Type = connectedPort.Type;
                }
            }
            Do(portInfos);
        }

    }
}
