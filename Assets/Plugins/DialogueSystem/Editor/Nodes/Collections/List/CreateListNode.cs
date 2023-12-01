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
using DialogueSystem.Generators;
using System.Text;

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
                    IsFunction = true,
                    IsAnchorable = true,
                });
            }
        }
        protected override void DrawMainContainer(VisualElement container)
        {
            base.DrawMainContainer(container);

            BasePort firstInputPort = GetInputPorts()[0];
            Button addChoiceBtn = DSUtilities.CreateButton(
                "AddInput",
                () =>
                {
                    var t = AddPortByType(
                        ID: Guid.NewGuid().ToString(),
                        portText: firstInputPort.portName,
                        type: firstInputPort.Type,
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
            BasePort connectedPort = edge.output as BasePort;

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

            Debug.Log($"{Model.NodeName}: input port: {_port.portName}-{_port.Type} connected {connectedPort.portName}-{connectedPort.Type}");
            Do(portInfos);
        }

        internal override string LambdaGenerationContext(MethodParamsInfo[] inputVariables, MethodParamsInfo[] outputVariables)
        {
            StringBuilder sb = new StringBuilder();
            sb
                .Append("System.Collections.Generic.List<T> GetList<T>(params T[] param) => new System.Collections.Generic.List<T>(param);")
                .Append(GHelper.TR)
                .Append("return ")
                .Append("GetList")
                .Append(GHelper.L_TRIANGE)
                .Append(inputVariables[0].ParamType.FullName)
                .Append(GHelper.R_TRIANGE)
                .Append(GHelper.BR_OP);

            for (int i = 0; i < inputVariables.Length; i++)
            {
                sb.Append(inputVariables[i].ParamName);
                if (i < inputVariables.Length - 1) sb.Append(GHelper.COMMA).Append(GHelper.SPACE);
            }
            
            sb
                .Append(GHelper.BR_CL)
                .Append(GHelper.QUOTES);

            return sb.ToString();
        }

        public override void Do(PortInfo[] portInfos)
        {
            base.Do(portInfos);

            Type listType = typeof(List<>).MakeGenericType(portInfos[0].Type);
            ChangeOutputPortsTypeAndName(listType, $"ListOf{portInfos[0].Type.Name}");
        }
    }
}