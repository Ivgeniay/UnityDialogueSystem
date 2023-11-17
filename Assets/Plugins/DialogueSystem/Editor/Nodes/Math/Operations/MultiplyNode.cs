using DialogueSystem.Generators;
using System.Linq;
using System.Text;

namespace DialogueSystem.Nodes
{
    internal class MultiplyNode : BaseOperationNode
    {
        
        internal override string LambdaGenerationContext(MethodParamsInfo[] inputVariables, MethodParamsInfo[] outputVariables)
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
                    .Append("else if (param is bool b) concatenatedString += string.Concat(System.Linq.Enumerable.Repeat(concatenatedString, (int)fl));")
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
                    if (inputVariables[i].ParamType == typeof(bool)) sb.Append($"{inputVariables[i].ParamName} == true ? 1 : 0");
                    else sb.Append($"{inputVariables[i].ParamName}");
                    if (i != inputVariables.Length - 1) sb.Append(" * ");
                }
            }
            sb.Append(';');

            return sb.ToString();
        }
    }
}
