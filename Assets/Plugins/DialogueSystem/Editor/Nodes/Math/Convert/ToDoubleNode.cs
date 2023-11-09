using DialogueSystem.Generators;
using DialogueSystem.Nodes;
using DialogueSystem.Utilities;
using DialogueSystem.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Plugins.DialogueSystem.Editor.Nodes.Math.Convert
{
    internal class ToDoubleNode : BaseConvertNode
    {
        internal override void Initialize(DSGraphView graphView, Vector2 position, List<object> portsContext)
        {
            base.Initialize(graphView, position, context: portsContext);

            if (portsContext == null)
            {
                Model.Inputs.Add(new(DSConstants.AvalilableTypes)
                {
                    PortText = $"All",
                    Value = 0,
                    Cross = false,
                    IsField = false,
                    IsInput = true,
                    IsSingle = true,
                    Type = typeof(bool)
                });

                Model.Outputs.Add(new(new Type[]
                {
                    typeof(double)
                })
                {
                    Type = typeof(double),
                    Value = 0f,
                    Cross = false,
                    IsField = false,
                    IsInput = false,
                    IsSingle = false,
                    PortText = DSConstants.Double,
                    IsFunction = true,
                    IsSerializedInScript = true,
                });
            }
        }

        internal override string LambdaGenerationContext(MethodGen.MethodParamsInfo[] inputVariables, MethodGen.MethodParamsInfo[] outputVariables)
        {
            StringBuilder sb = new();

            if (inputVariables.Any(e => e.ParamType == typeof(string)))
            {
                sb.Append("double SafeParseToDouble(string input)")
                    .Append("\n")
                    .Append("{")
                    .Append("\n")
                    .Append("double result;")
                    .Append("\n")
                    .Append("if (double.TryParse(input, out result)) return result;")
                    .Append("\n")
                    .Append("return 0;")
                    .Append("\n")
                    .Append("}\n");
            }

            sb.Append("return ");
            for (int i = 0; i < inputVariables.Length; i++)
            {
                switch (inputVariables[i].ParamType)
                {
                    case var t when t == typeof(double):
                        sb.Append($"{inputVariables[i].ParamName}");
                        break;
                    case var t when t == typeof(float):
                        sb.Append($"(double){inputVariables[i].ParamName}");
                        break;
                    case var t when t == typeof(int):
                        sb.Append($"(double){inputVariables[i].ParamName}");
                        break;
                    case var t when t == typeof(string):
                        sb.Append($"SafeParseToDouble({inputVariables[i].ParamName})");
                        break;
                    case var t when t == typeof(bool):
                        sb.Append($"{inputVariables[i].ParamName} ? 1d : 0d");
                        break;
                }
                if (i != inputVariables.Length - 1) sb.Append(" + ");
            }
            sb.Append(';');

            return sb.ToString();
        }
    }
}
