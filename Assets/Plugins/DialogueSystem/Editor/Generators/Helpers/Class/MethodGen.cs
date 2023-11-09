using System.Collections.Generic;
using DialogueSystem.Nodes;
using System.Text;
using System;
using DialogueSystem.Ports;
using System.Linq;
using DialogueSystem.Lambdas;
using UnityEngine;
using DialogueSystem.Edges;
using System.Reflection.Metadata;
using System.Runtime.InteropServices;
using static Codice.CM.WorkspaceServer.DataStore.IncomingChanges.StoreIncomingChanges.FileConflicts;
using DialogueSystem.TextFields;

namespace DialogueSystem.Generators
{
    internal class MethodGen : BaseGeneratorHelper
    {
        private Dictionary<BaseNode, string> methods { get; set; } = new();

        private List<string> methodToDraw = new();

        private VariablesGen variablesGen;
        private ClassGen classGen;

        internal MethodGen(VariablesGen variablesGen, ClassGen classGen) 
        {
            this.variablesGen = variablesGen;
            this.classGen = classGen;
        }


        internal Method ConstructInitializeMethod(string MethodName, params BaseNode[] nodesToInitialize)
        {
            Method method = MethodFabric(variablesGen, classGen);

            method.Visibility = Visibility.@private;
            method.MethodName = MethodName;

            StringBuilder mb = new();

            if (nodesToInitialize != null && nodesToInitialize.Length > 0)
            {
                for (int i = 0; i < nodesToInitialize.Length; i++)
                {
                    var r = classGen.GetDSClass(nodesToInitialize[i]);
                    var mainVariable = variablesGen.GetMainClassVariable(nodesToInitialize[i]);

                    if (nodesToInitialize[i] is ActorNode actor)
                    {
                        method.InnerParams.Add(actor);
                        mb
                            .Append(THIS)
                            .Append(mainVariable)
                            .Append(SPACE)
                            .Append(EQLS)
                            .Append(SPACE)
                            .Append(mainVariable)
                            .Append(QUOTES)
                            .Append(TR);
                        continue;
                    }

                    mb.Append(mainVariable)
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
                        if (nodesToInitialize[i] is BasePrimitiveNode number)
                        {
                            mb.Append(localVariable2.Name)
                                .Append(EQLS)
                                .Append(SPACE)
                                .Append((outputs[j].Value).ToString().Replace(",", "."));
                            if (outputs[j].Type == typeof(float)) mb.Append("f");
                            if (outputs[j].Type == typeof(double)) mb.Append("d");
                            mb.Append(COMMA);

                            continue;
                        }
                        else if (nodesToInitialize[i] is BaseLetterNode letter)
                        {
                            mb.Append(localVariable2.Name)
                                .Append(EQLS)
                                .Append(SPACE)
                                .Append(QM)
                                .Append(outputs[j].Value)
                                .Append(QM)
                                .Append(COMMA);

                            continue;
                        }
                        else if (nodesToInitialize[i] is BaseOperationNode operation) InitFunctionalNodes(operation, mb, localVariable2);
                        else if (nodesToInitialize[i] is BaseConvertNode convert) InitFunctionalNodes(convert, mb, localVariable2);
                        else if (nodesToInitialize[i] is BaseLogicNode logic) InitFunctionalNodes(logic, mb, localVariable2);
                    }

                    if (nodesToInitialize[i] is BaseDialogueNode baseDialogue)
                    {
                        DSTextField dialogueText = baseDialogue.GetDialogueTextField();
                        var localVariable = r.GetVariable(dialogueText);
                        mb.Append(localVariable.Name)
                            .Append(EQLS)
                            .Append(QM)
                            .Append(dialogueText.text)
                            .Append(QM)
                            .Append(COMMA);

                        if (baseDialogue is StartDialogueNode startDialogue)
                        {

                        }
                    }


                    mb.Append(BR_F_CL)
                        .Append(QUOTES)
                        .Append(TR);
                }
            }

            method.MethodBody.Append(mb);
            return method;
        }

        internal Method MethodFabric(VariablesGen variablesGen, ClassGen classGen) => new Method(variablesGen, classGen);
        private void InitFunctionalNodes(BaseNode nodesToInitialize, StringBuilder mb, VariableInfo localVariable2)
        {
            mb.Append(localVariable2.Name)
                .Append(EQLS)
                .Append(SPACE)
                .Append(BR_OP)
                .Append(BR_CL)
                .Append(EQLS)
                .Append(R_TRIANGE);

            var inputs = nodesToInitialize.GetInputPorts();
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
                    ParamType = e.Type
                };
                return methofInfo;
            }).ToArray();

            string lambda = "";
            if (inputParameters.Length == 0 && localVariable2.Type.IsValueType) lambda = "return " + Activator.CreateInstance(localVariable2.Type) + ";";
            else if (inputParameters.Length == 0 && !localVariable2.Type.IsValueType) lambda = "return \"\"";
            else lambda = nodesToInitialize.LambdaGenerationContext(inputParameters, new MethodParamsInfo[0]);

            mb.Append(BR_F_OP)
                .Append(lambda)
                .Append(BR_F_CL)
                .Append(COMMA);
        }


        internal class MethodParamsInfo
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

    internal class Method : BaseGeneratorHelper
    {
        internal Visibility Visibility;
        internal List<Type> OutParameters;
        internal StringBuilder MethodBody = new();
        internal List<BaseNode> InnerParams = new();
        internal string MethodName = "";

        private VariablesGen variablesGen;
        private ClassGen classGen;
        internal Method(VariablesGen variablesGen, ClassGen classGen)
        {
            this.variablesGen = variablesGen;
            this.classGen = classGen;
        }

        public string Build()
        {
            StringBuilder final = new();

            final.Append(GetVisibility(Visibility == Visibility.None ? Visibility.@private : Visibility))
                .Append(SPACE);

            if (OutParameters == null || OutParameters.Count == 0) final.Append("void");
            else
            {
                if (OutParameters.Count > 1) final.Append(BR_OP);
                for (int i = 0; i < OutParameters.Count; i++)
                {
                    final.Append(OutParameters[i].FullName);
                    if (i < InnerParams.Count - 1) final.Append(SPACE).Append(COMMA);
                }
                if (OutParameters.Count > 1) final.Append(BR_CL);
            }

            final
                .Append(SPACE)
                .Append(MethodName)
                .Append(BR_OP);
            
            if (InnerParams != null && InnerParams.Count > 0)
            {
                for (int i = 0; i < InnerParams.Count; i++)
                {
                    var v = variablesGen.GetMainClassVariable(InnerParams[i]);
                    var t = classGen.GetDSClass(InnerParams[i]);
                    final.Append(t.GetClassType()).Append(SPACE).Append(v);
                    if (i < InnerParams.Count - 1) final.Append(COMMA);
                }
            }
            final
                .Append(BR_CL)
                .Append(TR)
                .Append(BR_F_OP)
                .Append(TR);

            final.Append(MethodBody);

            final
                .Append(BR_F_CL)
                .Append(TR);

            return final.ToString();
        }
    }
}
