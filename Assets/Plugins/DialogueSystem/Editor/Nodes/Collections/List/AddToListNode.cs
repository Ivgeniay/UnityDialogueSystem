using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DialogueSystem.Database.Save;
using DialogueSystem.Generators;
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
                Model.AddPort(new DSPortModel(DSConstants.CollectionsTypes, PortSide.Input)
                {
                    PortText = "ListOfObject",
                    Type = typeof(List<object>),
                    Cross = false,
                    IsField = false,
                    IsSingle = true,
                    IsInput = true,
                    Value = "",
                    IsAnchorable = false,
                    IsFunction = false,
                });

                Model.AddPort(new DSPortModel(DSConstants.AvalilableTypes, PortSide.Input)
                {
                    PortText = "Object",
                    Type = typeof(object),
                    Cross = false,
                    IsField = false,
                    IsSingle = true,
                    IsInput = true,
                    Value = "",
                    IsAnchorable = false,
                    IsFunction = false,
                });

                Model.AddPort(new DSPortModel(DSConstants.CollectionsTypes, Ports.PortSide.Output)
                {
                    PortText = "ListOfObject",
                    Type = typeof(List<object>),
                    Cross = false,
                    IsField = false,
                    IsSingle = true, 
                    IsInput = false,
                    Value = "",
                    IsAnchorable = false,
                    IsFunction = true,
                });
            }
        }

        public override void OnConnectInputPort(BasePort port, Edge edge)
        {
            base.OnConnectInputPort(port, edge);

            BasePort connectedPort = edge.output as BasePort;
            bool compireType = connectedPort.Type == port.Type;
            if (!compireType)
            {
                bool continues = BasePortManager.HaveCommonTypes(connectedPort.Type, port.AvailableTypes);
                if (!continues) return;
            }

            PortInfo[] portInfos = new PortInfo[inputPorts.Count];

            for (var i = 0; i < inputPorts.Count; i++)
                portInfos[i] = new PortInfo()
                {
                    node = this,
                    port = inputPorts[i],
                    Type = inputPorts[i].Type,
                    Value = inputPorts[i].Type.IsValueType == true ? Activator.CreateInstance(inputPorts[i].Type) : ""
                }; 

            Type type = connectedPort.Type;
            if (port == inputPorts[0])
            {
                if (!type.IsGenericType) return;
                Type genericType = type.GetGenericTypeDefinition();
                if (genericType == null && genericType != typeof(List<>)) return;
                Type genericArgType = type.GetGenericArguments()[0];

                if (connectedPort != null)
                {
                    ChangePortTypeAndName(port, connectedPort.Type);
                    PortInfo infos = portInfos.Where(e => e.port == port).FirstOrDefault();
                    infos.Value = connectedPort.Value;
                    infos.Type = connectedPort.Type;
                }
                for (int i = 1; i < inputPorts.Count; i++)
                {
                    if (inputPorts[i].Type != genericArgType) inputPorts[i].DisconnectAll();
                    ChangePortTypeAndName(inputPorts[i], genericArgType, genericArgType.Name);
                    PortInfo infos = portInfos.Where(e => e.port == inputPorts[i]).FirstOrDefault();
                    infos.Value = "";
                    infos.Type = genericArgType;
                }

            }
            else
            {
                Type genericArgType = inputPorts[0].Type.GetGenericArguments()[0];
                if (connectedPort.Type == genericArgType){ }
                else
                {
                    Type genType = typeof(List<>).MakeGenericType(connectedPort.Type);
                    inputPorts[0].DisconnectAll();
                    ChangePortTypeAndName(inputPorts[0], genType, $"ListOf{connectedPort.Type.Name}");

                    for (int i = 1; i < inputPorts.Count; i++)
                    {
                        if (inputPorts[i].Type != connectedPort.Type) inputPorts[i].DisconnectAll();
                        ChangePortTypeAndName(inputPorts[i], connectedPort.Type, connectedPort.Type.Name);
                        PortInfo infos = portInfos.Where(e => e.port == inputPorts[i]).FirstOrDefault();
                        infos.Type = connectedPort.Type;
                        if (inputPorts[i] == port) infos.Value = connectedPort.Value;
                        else infos.Value = DSUtilities.GetDefaultValue(connectedPort.Type);
                    }
                }
            }
            Do(portInfos);
        }

        public override void Do(PortInfo[] portInfos)
        {
            base.Do(portInfos);
            ChangeOutputPortsTypeAndName(portInfos[0].Type, $"ListOf{portInfos[0].Type.GetGenericArguments()[0].Name}");
        }

        internal override string LambdaGenerationContext(MethodParamsInfo[] inputVariables, MethodParamsInfo[] outputVariables)
        {
            StringBuilder sb = new StringBuilder();
            sb
                .Append("System.Collections.Generic.List<T> GetList<T>(params T[] param) => new System.Collections.Generic.List<T>(param);")
                .Append(GHelper.TR);

            sb
                .Append("var")
                .Append(GHelper.SPACE)
                .Append("result")
                .Append(GHelper.SPACE)
                .Append(GHelper.EQLS)
                .Append(GHelper.SPACE)
                .Append(inputVariables[0].ParamName)
                .Append(GHelper.QUOTES)
                .Append(GHelper.TR)
                .Append("result.AddRange")
                .Append(GHelper.BR_OP)
                .Append("GetList")
                .Append(GHelper.L_TRIANGE)
                .Append(inputVariables[1].ParamType.FullName)
                .Append(GHelper.R_TRIANGE)
                .Append(GHelper.BR_OP);

            for (int i = 1; i < inputVariables.Length; i++)
            {
                sb.Append(inputVariables[i].ParamName);
                if (i < inputVariables.Length - 1) sb.Append(GHelper.COMMA).Append(GHelper.SPACE);
            }

            sb
                .Append(GHelper.BR_CL)
                .Append(GHelper.BR_CL)
                .Append(GHelper.QUOTES)
                .Append(GHelper.TR);

            sb
                .Append("return")
                .Append(GHelper.SPACE)
                .Append("result");

            return sb.ToString();
        }
    }
}
