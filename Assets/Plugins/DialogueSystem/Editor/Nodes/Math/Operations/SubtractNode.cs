using DialogueSystem.Generators;
using System.Linq;
using System.Text;

namespace DialogueSystem.Nodes
{
    internal class SubtractNode : BaseOperationNode
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
                    .Append("string concatenatedString = parameters.OfType<string>().FirstOrDefault();;")
                    .Append("\n")
                    .Append("foreach (var param in parameters)")
                    .Append("\n")
                    .Append("{")
                    .Append("\n")
                    .Append("if (param is int num) if (concatenatedString.Length >= num) concatenatedString = concatenatedString.Substring(0, num);")
                    .Append("\n")
                    .Append("else if (param is double dbl) if (concatenatedString.Length >= num) concatenatedString = concatenatedString.Substring(0, (int)dbl);")
                    .Append("\n")
                    .Append("else if (param is float fl) if (concatenatedString.Length >= num) concatenatedString = concatenatedString.Substring(0, (int)fl);")
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
                    if (i != inputVariables.Length - 1) sb.Append(" - ");
                }
            }
            sb.Append(';');

            return sb.ToString();
        }
    }
}
