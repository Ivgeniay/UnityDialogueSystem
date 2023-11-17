using DialogueSystem.Generators;
using DialogueSystem.Utilities;
using DialogueSystem.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace DialogueSystem.Nodes
{
    internal class EqualNode : BaseLogicNode
    {
        internal override void Initialize(DSGraphView graphView, Vector2 position, List<object> portsContext)
        {
            base.Initialize(graphView, position, portsContext: portsContext);

            if (portsContext == null)
            {
                Model.Inputs.Add(new(DSConstants.AvalilableTypes)
                {
                    Cross = false,
                    IsField = false,
                    IsIfPort = false,
                    IsInput = true,
                    IsSingle = true,
                    PortText = DSConstants.All,
                    Type = null,
                    Value = null,
                });

                Model.Inputs.Add(new(DSConstants.AvalilableTypes)
                {
                    Cross = false,
                    IsField = false,
                    IsIfPort = false,
                    IsInput = true,
                    IsSingle = true,
                    PortText = DSConstants.All,
                    Type = null,
                    Value = null,
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
                if (i != inputVariables.Length - 1) sb.Append(" == ");
            }
            sb.Append(';');

            return sb.ToString();
        }
    }
}
