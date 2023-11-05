using System.Collections.Generic;
using DialogueSystem.Nodes;
using System.Text;
using System;
using DialogueSystem.Ports;
using System.Linq;

namespace DialogueSystem.Generators
{
    internal class MethodGen : BaseGeneratorHelper
    {
        private Dictionary<BaseNode, MethodInfo> methodInfos { get; set; } = new();
        private Dictionary<BaseNode, string> methods { get; set; } = new();
        private List<string> methodToDraw = new();

        private VariablesGen variablesGen;

        internal MethodGen(VariablesGen variablesGen) 
        {
            this.variablesGen = variablesGen;
        }

        internal string ConstructMethod(string MethodName, string visibility, string context, string[] returnTypes = null, string[] inputTypes = null)
        {
            StringBuilder sb = new StringBuilder()
                    .Append(visibility)
                    .Append(SPACE);

            if (returnTypes == null && returnTypes.Length == 0) sb.Append("void").Append(SPACE);
            else if (returnTypes != null && returnTypes.Length == 1) sb.Append(returnTypes[0]);
            else if (returnTypes != null && returnTypes.Length > 1)
            {
                sb.Append(BR_OP);

                for (int i = 0; i < returnTypes.Length; i++)
                {
                    sb.Append(returnTypes[i]);
                    if (i != returnTypes.Length - 1) sb.Append(", ");
                }
                sb.Append(BR_CL);
            }
            sb.Append(SPACE).Append(MethodName).Append(BR_OP);
            if (inputTypes!= null)
            {
                for (int i = 0;i < inputTypes.Length;i++)
                {
                    sb.Append(inputTypes[i]);
                    if (i != inputTypes.Length - 1) sb.Append(", ");
                }
            }
            sb.Append(')')
                .Append(TR)
                .Append(BR_F_OP)
                .Append(context)
                .Append(BR_F_CL);

            return sb.ToString();
        }

        internal void GetMethod(BaseNode node, Visibility visibility = Visibility.Public, Attribute attribute = Attribute.None)
        {
            if (methods.TryGetValue(node, out string methodStr)) AddMethodToDraw(methodStr);
            else
            {
                StringBuilder sb = new StringBuilder();
                if (attribute != Attribute.None) sb.Append(GetAttribute(attribute));
                sb.Append(GetVisibility(visibility)).Append(SPACE);

                MethodInfo methodInfo = GetMethodInfo(node);
                MethodParamsInfo[] inputVariables = new MethodParamsInfo[methodInfo.CountParams];
                MethodParamsInfo[] outputVariables = new MethodParamsInfo[methodInfo.CountOutParams];

                if (methodInfo.CountOutParams == 0) sb.Append("void");
                else if (methodInfo.CountOutParams == 1)
                {
                    outputVariables[0] = new MethodParamsInfo()
                    {
                        ParamName = methodInfo.OutputParamTypes[0].Name + "_variable",
                        ParamType = methodInfo.OutputParamTypes[0]
                    };
                    sb.Append(outputVariables[0].ParamType.Name);
                }
                else
                {
                    sb.Append("(");
                    for (int i = 0; i < methodInfo.CountOutParams; i++)
                    {
                        outputVariables[i] = new MethodParamsInfo()
                        {
                            ParamName = methodInfo.OutputParamTypes[i].Name + "_variable",
                            ParamType = methodInfo.OutputParamTypes[i]
                        };
                        sb.Append(methodInfo.OutputParamTypes[i].Name);
                        if (i != methodInfo.CountOutParams) sb.Append(", ");
                    }
                    sb.Append(")");
                }
                sb.Append(SPACE)
                    .Append(node.Model.NodeName)
                    .Append(BR_OP);

                if (methodInfo.CountParams > 0)
                {
                    string vName = "item_";
                    for (int i = 0; i < methodInfo.CountParams; i++)
                    {
                        inputVariables[i] = new MethodParamsInfo()
                        {
                            ParamType = methodInfo.InputParamTypes[i],
                            ParamName = vName + i,
                        };
                        sb.Append(inputVariables[i].ParamType.Name)
                            .Append(SPACE)
                            .Append(vName + i);

                        if (i != methodInfo.CountParams - 1)
                        {
                            sb.Append(',')
                                .Append(SPACE);
                        }
                    }
                }
                sb.Append(BR_CL)
                .Append(TR)
                .Append(BR_F_OP)
                .Append(node.MethodGenerationContext(inputVariables, outputVariables))
                .Append(TR)
                .Append(BR_F_CL);

                AddMethodToDraw(sb.ToString());
            }
        }
        internal string GetCallMethod(BaseNode node)
        {
            StringBuilder sb = new StringBuilder();
            var methodInfo = GetMethodInfo(node);
            var inputPorts = node.GetInputPorts();

            sb
                .Append(methodInfo.MethodName)
                .Append('(');

            for (int i = 0; i < inputPorts.Count; i++)
            {
                string variable = string.Empty;
                if (inputPorts[i].connected)
                {
                    var connections = inputPorts[i].connections.ToList();
                    BasePort connectedPort = connections[0].output as BasePort;
                    variable = variablesGen.GetVariable(connectedPort);
                }
                else variable = inputPorts[i].portType.IsValueType ? Activator.CreateInstance(inputPorts[i].portType).ToString() : "null";
                sb.Append(variable);
                if (i != inputPorts.Count - 1) sb.Append(", ");
            }

            sb.Append(");\n");
            return sb.ToString();
        }
        internal void AddMethodToDraw(string method)
        {
            if (!methodToDraw.Contains(method))
                methodToDraw.Add(method);
        }

        private MethodInfo GenerateMethodInfo(BaseNode baseNode, Visibility visibility = Visibility.Public, Attribute attribute = Attribute.None)
        {
            var inputs = baseNode.GetInputPorts();
            var outputs = baseNode.GetOutputPorts();

            List<Type> inputParamTypes = new();
            List<Type> outputParamTypes = new();

            foreach (var port in inputs) inputParamTypes.Add(port.portType);
            foreach (var port in outputs) outputParamTypes.Add(port.portType);

            return new MethodInfo()
            {
                InputParamTypes = inputParamTypes.ToArray(),
                OutputParamTypes = outputParamTypes.ToArray(),
                MethodName = baseNode.Model.NodeName,
                Visibility = visibility,
                Attribute = attribute,
                CountParams = inputParamTypes.Count,
                CountOutParams = outputParamTypes.Count
            };
        }
        private MethodInfo GetMethodInfo(BaseNode node)
        {
            if (methodInfos.TryGetValue(node, out MethodInfo method)) return method;
            else return GenerateMethodInfo(node);
        }
        private record MethodInfo
        {
            public int CountParams;
            public int CountOutParams;
            public Type[] InputParamTypes;
            public Type[] OutputParamTypes;
            public string MethodName;
            public Visibility Visibility;
            public Attribute Attribute;
            public string OutputParamT;
        }
        public class MethodParamsInfo
        {
            public string ParamName;
            public Type ParamType;
        }

        internal override StringBuilder Draw(StringBuilder context)
        {
            foreach (string method in methodToDraw)
                context.AppendLine(method);

            context.AppendLine();
            return context;
        }
    }

}
