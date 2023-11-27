using System.Collections.Generic;
using DialogueSystem.Window;
using DialogueSystem.Ports;
using UnityEngine;
using System;
using DialogueSystem.Utilities;
using DialogueSystem.Generators;
using System.Text;

namespace DialogueSystem.Nodes
{
    internal class ToBoolNode : BaseConvertNode
    {
        internal override void Initialize(DSGraphView graphView, Vector2 position, List<object> portsContext)
        {
            base.Initialize(graphView, position, context: portsContext);

            if (portsContext == null)
            {
                Model.AddPort(new(DSConstants.AvalilableTypes, PortSide.Input)
                {
                    PortText = $"All",
                    Value = "0",
                    Cross = false,
                    IsField = false,
                    IsInput = true,
                    IsSingle = true,
                    Type = typeof(bool)
                });

                Model.AddPort(new(new Type[] { typeof(bool) }, PortSide.Output)
                {
                    Type = typeof(bool),
                    Value = "false",
                    Cross = false,
                    IsField = false,
                    IsInput = false,
                    IsSingle = true,
                    PortText = typeof(bool).Name,
                    IsFunction = true
                });
            }
        }

        internal override string LambdaGenerationContext(MethodParamsInfo[] inputVariables, MethodParamsInfo[] outputVariables)
        {
            StringBuilder sb = new();
            sb.Append("return ");

            if (inputVariables.Length == 0)
            {
                sb.Append("false;");
            }
            else
            {
                for (int i = 0; i < inputVariables.Length; i++)
                {
                    switch (inputVariables[i].ParamType)
                    {
                        case var t when t == typeof(double):
                            sb.Append($"Convert.ToBoolean({inputVariables[i].ParamName})");
                            break;
                        case var t when t == typeof(float):
                            sb.Append($"Convert.ToBoolean({inputVariables[i].ParamName})");
                            break;
                        case var t when t == typeof(int):
                            sb.Append($"Convert.ToBoolean({inputVariables[i].ParamName})");
                            break;
                        case var t when t == typeof(string):
                            sb.Append($"{inputVariables[i].ParamName} == \"true\"");
                            break;
                        case var t when t == typeof(bool):
                            sb.Append($"{inputVariables[i].ParamName}");
                            break;
                    }

                    if (i != inputVariables.Length - 1) sb.Append(" && ");
                }
            }
            sb.Append(';');

            return sb.ToString();
        }
    }
}
