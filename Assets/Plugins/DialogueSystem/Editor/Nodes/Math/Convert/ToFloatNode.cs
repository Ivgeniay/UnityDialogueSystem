using System.Collections.Generic;
using DialogueSystem.Utilities;
using DialogueSystem.Window;
using UnityEngine;
using System;
using DialogueSystem.Generators;
using System.Text;

namespace DialogueSystem.Nodes
{
    internal class ToFloatNode : BaseConvertNode
    {
        internal override void Initialize(DSGraphView graphView, Vector2 position, List<object> portsContext)
        {
            base.Initialize(graphView, position, context: portsContext);

            if (portsContext == null)
            {
                Model.Inputs.Add(new(DSConstants.AvalilableTypes)
                {
                    PortText = DSConstants.All,
                    Value = 0,
                    Cross = false,
                    IsField = false,
                    IsInput = true,
                    IsSingle = true,
                    Type = typeof(bool),
                });

                Model.Outputs.Add(new(new Type[] { typeof(float) })
                {
                    PortText = DSConstants.Float,
                    Type = typeof(float),
                    Value = 0f,
                    Cross = false,
                    IsField = false,
                    IsInput = false,
                    IsSingle = false,
                });
            }
        }

        internal override string LambdaGenerationContext(MethodGen.MethodParamsInfo[] inputVariables, MethodGen.MethodParamsInfo[] outputVariables)
        {
            StringBuilder sb = new();
            sb.Append("return ");
            for (int i = 0; i < inputVariables.Length; i++)
            {
                switch(inputVariables[i].ParamType)
                {
                    case var t when t == typeof(double):
                        break;
                    case var t when t == typeof(float):
                        break;
                    case var t when t == typeof(int):
                        break;
                    case var t when t == typeof(string):
                        break;
                    case var t when t == typeof(bool):
                        break;
                }
                sb.Append($"{inputVariables[i].ParamName}");
                if (i != inputVariables.Length - 1) sb.Append(" + ");
            }
            sb.Append(';');

            return sb.ToString();
        }
    }
}
