using DialogueSystem.Database.Save;
using DialogueSystem.Generators;
using DialogueSystem.Ports;
using DialogueSystem.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace DialogueSystem.Nodes
{
    internal class MultiplyNode : BaseOperationNode
    {
        
        internal override string LambdaGenerationContext(MethodGen.MethodParamsInfo[] inputVariables, MethodGen.MethodParamsInfo[] outputVariables)
        {
            StringBuilder sb = new();
            sb.Append("return ");
            for (int i = 0; i < inputVariables.Length; i++)
            {
                sb.Append($"{inputVariables[i].ParamName}");
                if (i != inputVariables.Length - 1) sb.Append(" * ");
            }
            sb.Append(';');

            return sb.ToString();
        }
    }
}
