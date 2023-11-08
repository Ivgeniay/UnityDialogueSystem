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
            var isString = inputVariables.Any(e => e.ParamType == typeof(string));
            if (isString)
            {
                sb.Append("string LambdaGen(params object[] parameters)")
                    .Append("\n")
                    .Append("{")
                    .Append("\n")
                    .Append("string concatenatedString = \"\";")
                    .Append("\n")
                    .Append("foreach (var param in parameters)")
                    .Append("\n")
                    .Append("{")
                    .Append("if (param is string str) concatenatedString += str;")
                    .Append("\n")
                    .Append("else if (param is int num) concatenatedString = string.Concat(System.Linq.Enumerable.Repeat(concatenatedString, num));")
                    .Append("\n")
                    .Append("else if (param is double dbl) concatenatedString = string.Concat(System.Linq.Enumerable.Repeat(concatenatedString, (int)dbl));")
                    .Append("\n")
                    .Append("else if (param is float fl) concatenatedString = string.Concat(System.Linq.Enumerable.Repeat(concatenatedString, (int)fl));")
                    .Append("\n")
                    .Append("}")
                    .Append("return concatenatedString;")
                    .Append("}");
            }
            sb.Append("return ");
            if (isString)
            {
                sb.Append("LambdaGen(");
            }

            for (int i = 0; i < inputVariables.Length; i++)
            {
                if (isString)
                {
                    sb.Append($"{inputVariables[i].ParamName}");
                    if (i != inputVariables.Length - 1) sb.Append(", ");
                    else sb.Append(")");
                }
                else
                {
                    sb.Append($"{inputVariables[i].ParamName}");
                    if (i != inputVariables.Length - 1) sb.Append(" * ");
                }
            }
            sb.Append(';');

            return sb.ToString();
        }
    }
}
