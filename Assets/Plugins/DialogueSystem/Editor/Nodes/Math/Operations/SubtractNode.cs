using DialogueSystem.Generators;
using System.Text;

namespace DialogueSystem.Nodes
{
    internal class SubtractNode : BaseOperationNode
    {

        internal override string LambdaGenerationContext(MethodGen.MethodParamsInfo[] inputVariables, MethodGen.MethodParamsInfo[] outputVariables)
        {
            StringBuilder sb = new();
            sb.Append("return ");
            for (int i = 0; i < inputVariables.Length; i++)
            {
                sb.Append($"{inputVariables[i].ParamName}");
                if (i != inputVariables.Length - 1) sb.Append(" - ");
            }
            sb.Append(';');

            return sb.ToString();
        }
    }
}
