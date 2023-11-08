using System.Collections.Generic;
using DialogueSystem.Nodes;
using System.Text;
using System;
using DialogueSystem.Ports;
using System.Linq;
using DialogueSystem.Lambdas;
using UnityEngine;
using DialogueSystem.Edges;

namespace DialogueSystem.Generators
{
    internal class MethodGen : BaseGeneratorHelper
    {
        private Dictionary<BaseNode, string> methods { get; set; } = new();

        private List<string> methodToDraw = new();

        private VariablesGen variablesGen;
        private LambdaGenerator lambdaGenerator;
        private PropFieldGen propFieldGen;
        private ClassGen classGen;

        internal MethodGen(VariablesGen variablesGen, PropFieldGen propFieldGen, LambdaGenerator lambdaGenerator, ClassGen classGen) 
        {
            this.variablesGen = variablesGen;
            this.lambdaGenerator = lambdaGenerator;
            this.propFieldGen = propFieldGen;
            this.classGen = classGen;
        }

        internal string ConstructMethod(string MethodName, Visibility visibility, string context, string[] returnTypes = null, string[] inputTypes = null)
        {
            StringBuilder sb = new StringBuilder()
                    .Append(visibility)
                    .Append(SPACE);

            if (returnTypes == null || returnTypes.Length == 0) sb.Append("void").Append(SPACE);
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
        internal string ConstructInitializeMethod(string MethodName, params BaseNode[] nodesToInitialize)
        {
            StringBuilder sb = new StringBuilder()
                    .Append(GetVisibility(Visibility.@private))
                    .Append(SPACE)
                    .Append("void")
                    .Append(SPACE);

            sb.Append(MethodName).Append(BR_OP);



            sb.Append(')')
                .Append(TR)
                .Append(BR_F_OP)
                .Append(SPACE);

            if (nodesToInitialize != null && nodesToInitialize.Length > 0)
            {
                for (int i = 0; i < nodesToInitialize.Length; i++)
                {
                    var r = classGen.GetDSClass(nodesToInitialize[i]);
                    var mainVariable = variablesGen.GetMainClassVariable(nodesToInitialize[i]);

                    sb.Append(mainVariable)
                        .Append(SPACE)
                        .Append(EQLS)
                        .Append(SPACE)
                        .Append(NEW)
                        .Append(BR_OP)
                        .Append(BR_CL)
                        .Append(TR)
                        .Append(BR_F_OP);

                    var outputs = nodesToInitialize[i].GetOutputPorts();

                    for (int j = 0; j < outputs.Count; j++)
                    {
                        var localVariable2 = r.GetVariable(outputs[j]);
                        if (nodesToInitialize[i] is BaseNumbersNode number)
                        {
                            sb.Append(localVariable2.Name)
                                .Append(EQLS)
                                .Append(SPACE)
                                .Append((outputs[j].Value).ToString().Replace(",", "."));
                            if (outputs[j].portType == typeof(float)) sb.Append("f");
                            if (outputs[j].portType == typeof(double)) sb.Append("d");
                            sb.Append(COMMA);
                        }
                        else if (nodesToInitialize[i] is BaseLetterNode letter)
                        {
                            sb.Append(localVariable2.Name)
                                .Append(EQLS)
                                .Append(SPACE)
                                .Append(QM)
                                .Append(outputs[j].Value)
                                .Append(QM)
                                .Append(COMMA);
                        }
                        else if (nodesToInitialize[i] is BaseOperationNode operation)
                        {
                            sb.Append(localVariable2.Name)
                                .Append(EQLS)
                                .Append(SPACE)
                                .Append(BR_OP)
                                .Append(BR_CL)
                                .Append(EQLS)
                                .Append(R_TRIANGE);

                            var inputs = operation.GetInputPorts();
                            List<BasePort> connectedOuputPorts = new();
                            foreach (var inputPort in inputs)
                            {
                                if (inputPort.connected)
                                {
                                    var connectionsEdge = inputPort.connections.ToList();
                                    foreach (var edge in connectionsEdge)
                                        connectedOuputPorts.Add(edge.output as BasePort);
                                }
                            }

                            MethodParamsInfo[] inputParameters = connectedOuputPorts.Select(e =>
                            {
                                var t = variablesGen.GetAndCallInnerClassVariableFunction(e);
                                if (string.IsNullOrWhiteSpace(t)) 
                                    t = "0d";
                                var methofInfo = new MethodParamsInfo()
                                {
                                    ParamName = t,
                                    ParamType = e.portType
                                };
                                return methofInfo;
                            }).ToArray();

                            string lambda = "";
                            if (inputParameters.Length == 0 && localVariable2.Type.IsValueType) lambda = "return " + Activator.CreateInstance(localVariable2.Type) + ";";
                            else if (inputParameters.Length == 0 && !localVariable2.Type.IsValueType) lambda = "return \"\"";
                            else lambda = operation.LambdaGenerationContext(inputParameters, new MethodParamsInfo[0]);

                            sb.Append(BR_F_OP)
                                .Append(lambda)
                                .Append(BR_F_CL)
                                .Append(COMMA);
                        }
                        else if (nodesToInitialize[i] is BaseConvertNode convert)
                        {
                            sb.Append(localVariable2.Name)
                                .Append(EQLS)
                                .Append(SPACE)
                                .Append(BR_OP)
                                .Append(BR_CL)
                                .Append(EQLS)
                                .Append(R_TRIANGE);

                            var inputs = convert.GetInputPorts();
                            List<BasePort> connectedOuputPorts = new();
                            foreach (var inputPort in inputs)
                            {
                                if (inputPort.connected)
                                {
                                    var connectionsEdge = inputPort.connections.ToList();
                                    foreach (var edge in connectionsEdge)
                                        connectedOuputPorts.Add(edge.output as BasePort);
                                }
                            }

                            MethodParamsInfo[] inputParameters = connectedOuputPorts.Select(e =>
                            {
                                var t = variablesGen.GetAndCallInnerClassVariableFunction(e);
                                if (string.IsNullOrWhiteSpace(t))
                                    t = "0d";
                                var methofInfo = new MethodParamsInfo()
                                {
                                    ParamName = t,
                                    ParamType = e.portType
                                };
                                return methofInfo;
                            }).ToArray();

                            string lambda = "";
                            if (inputParameters.Length == 0 && localVariable2.Type.IsValueType) lambda = "return " + Activator.CreateInstance(localVariable2.Type) + ";";
                            else if (inputParameters.Length == 0 && !localVariable2.Type.IsValueType) lambda = "return \"\"";
                            else lambda = convert.LambdaGenerationContext(inputParameters, new MethodParamsInfo[0]);

                            sb.Append(BR_F_OP)
                                .Append(lambda)
                                .Append(BR_F_CL)
                                .Append(COMMA);
                        }

                    }
                    sb.Append(BR_F_CL)
                        .Append(QUOTES)
                        .Append(TR);
                }
            }

            sb.Append(BR_F_CL)
                .Append(TR);

            return sb.ToString();
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
