using DialogueSystem.Generators;
using System.Text;

namespace DialogueSystem.Nodes
{
    internal class DivideNode : BaseOperationNode
    {
        internal override string LambdaGenerationContext(MethodGen.MethodParamsInfo[] inputVariables, MethodGen.MethodParamsInfo[] outputVariables)
        {
            StringBuilder sb = new();
            sb.Append("return ");
            for (int i = 0; i < inputVariables.Length; i++)
            {
                if (inputVariables[i].ParamType == typeof(bool)) sb.Append($"{inputVariables[i].ParamName} == true ? 1 : 0");
                else sb.Append($"{inputVariables[i].ParamName}");
                if (i != inputVariables.Length - 1) sb.Append(" / ");
            }
            sb.Append(';');

            return sb.ToString();
        }
    }
}
