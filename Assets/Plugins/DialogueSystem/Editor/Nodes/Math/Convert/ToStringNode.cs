using System.Collections.Generic;
using DialogueSystem.Utilities;
using DialogueSystem.Window;
using UnityEngine;
using System;
using DialogueSystem.Generators;
using System.Text;

namespace DialogueSystem.Nodes
{
    internal class ToStringNode : BaseConvertNode
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
                    Type = typeof(bool)
                });

                Model.AddPort(new(new Type[] { typeof(string) }, Ports.PortSide.Output)
                {
                    Type = typeof(string),
                    Value = 0f,
                    Cross = false,
                    IsField = false,
                    IsInput = false,
                    IsSingle = false,
                    PortText = DSConstants.String,
                    IsFunction = true,
                    IsSerializedInScript = true,
                });
            }
        }

        internal override string LambdaGenerationContext(MethodParamsInfo[] inputVariables, MethodParamsInfo[] outputVariables)
        {
            StringBuilder sb = new();
            sb.Append("return ");
            for (int i = 0; i < inputVariables.Length; i++)
            {
                sb.Append($"{inputVariables[i].ParamName}.ToString()");
                if (i != inputVariables.Length - 1) sb.Append(" + ");
            }
            sb.Append(';');

            return sb.ToString();
        }
    }
}
