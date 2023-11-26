using System.Collections.Generic;
using DialogueSystem.Utilities;
using DialogueSystem.Window;
using UnityEngine;
using System;
using DialogueSystem.Generators;
using System.Text;
using Unity.VisualScripting.YamlDotNet.Core.Tokens;
using System.Linq;

namespace DialogueSystem.Nodes
{
    internal class ToFloatNode : BaseConvertNode
    {
        internal override void Initialize(DSGraphView graphView, Vector2 position, List<object> portsContext)
        {
            base.Initialize(graphView, position, context: portsContext);

            if (portsContext == null)
            {
                Model.AddPort(new(DSConstants.AvalilableTypes, Ports.PortSide.Input)
                {
                    PortText = DSConstants.All,
                    Value = 0,
                    Cross = false,
                    IsField = false,
                    IsInput = true,
                    IsSingle = true,
                    Type = typeof(bool),
                });

                Model.AddPort(new(new Type[] { typeof(float) }, Ports.PortSide.Output)
                {
                    PortText = DSConstants.Float,
                    Type = typeof(float),
                    Value = 0f,
                    Cross = false,
                    IsField = false,
                    IsInput = false,
                    IsSingle = true,
                    IsFunction = true,
                    IsSerializedInScript = true,
                });
            }
        }

        internal override string LambdaGenerationContext(MethodParamsInfo[] inputVariables, MethodParamsInfo[] outputVariables)
        {
            StringBuilder sb = new();

            if (inputVariables.Any(e => e.ParamType == typeof(string)))
            {
                sb.Append("float SafeParseToFloat(string input)")
                    .Append("\n")
                    .Append("{")
                    .Append("\n")
                    .Append("input = input.Replace(',', '.');")
                    .Append("\n")
                    .Append("CultureInfo culture = CultureInfo.InvariantCulture;")
                    .Append("\n")
                    .Append("NumberStyles style = NumberStyles.Float;")
                    .Append("\n")
                    .Append("if (float.TryParse(input, style, culture, out float result)) return result;")
                    .Append("\n")
                    .Append("else return 0f;")
                    .Append("\n")
                    .Append("}\n");
            }

            sb.Append("return ");
            for (int i = 0; i < inputVariables.Length; i++)
            {
                switch(inputVariables[i].ParamType)
                {
                    case var t when t == typeof(double):
                        sb.Append($"(float){inputVariables[i].ParamName}");
                        break;
                    case var t when t == typeof(float):
                        sb.Append($"{inputVariables[i].ParamName}");
                        break;
                    case var t when t == typeof(int):
                        sb.Append($"(float){inputVariables[i].ParamName}");
                        break;
                    case var t when t == typeof(string):
                        sb.Append($"SafeParseToFloat({inputVariables[i].ParamName})");
                        break;
                    case var t when t == typeof(bool):
                        sb.Append($"{inputVariables[i].ParamName} ? 1f : 0f");
                        break;
                }
                if (i != inputVariables.Length - 1) sb.Append(" + ");
            }
            sb.Append(';');

            return sb.ToString();
        }
    }
}
