using System.Linq.Expressions;
using System.Linq;
using System;
using System.Collections.Generic;
using DialogueSystem.Nodes;
using static DialogueSystem.Generators.MethodGen;
using DialogueSystem.Generators;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;

namespace DialogueSystem.Lambdas
{
    internal class LambdaGenerator : BaseGeneratorHelper
    {
        public Dictionary<BaseNode, DelegateInfo> lambdas = new();

        private VariablesGen variablesGen;
        internal LambdaGenerator(VariablesGen variablesGen)
        {
            this.variablesGen = variablesGen;
        }

        internal DelegateInfo GetLambda(BaseNode node)
        {
            if (lambdas.TryGetValue(node, out DelegateInfo delegateInfo)) return delegateInfo;
            else return BuildLambda(node);
        }

        internal DelegateInfo BuildLambda(BaseNode node)
        {
            ParameterExpression[] paramExpressions = null;
                node.GetInputPorts()
                .Select(p => Expression.Parameter(p.Type, variablesGen.GetInnerClassVariable(p)))
                .ToArray();


            var delegate_ = node.LambdaGenerationContext(paramExpressions);
            var delegateInfo = new DelegateInfo()
            {
                Delegate = delegate_,
                parameterExpressions = paramExpressions,
                type = delegate_.GetType(),
                VarName = variablesGen.GetMainClassVariable(node)
            };
            lambdas.Add(node, delegateInfo);

            return delegateInfo;
        }

        internal byte[] SerializeLambda(Expression lambda)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                IFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, lambda);
                return stream.ToArray();
            }
        }
        internal T DeserializeLambda<T>(byte[] data)
        {
            using (MemoryStream stream = new MemoryStream(data))
            {
                IFormatter formatter = new BinaryFormatter();
                return (T)formatter.Deserialize(stream);
            }
        }

        internal string GetStringLambda(DelegateInfo delegateInfo)
        {
            StringBuilder sb = new StringBuilder();
            var typeString = delegateInfo.type.ToString();

            if (typeString.Contains("System.Func"))
                sb.Append("Func").Append(L_TRIANGE);

            sb.Append(ConvertTypeString(typeString))
                .Append(R_TRIANGE)
                .Append(SPACE)
                .Append(delegateInfo.VarName);

            return sb.ToString();
        }

        private string ConvertTypeString(string input)
        {
            int startIndex = input.IndexOf('[');
            int endIndex = input.LastIndexOf(']');

            if (startIndex != -1 && endIndex != -1 && endIndex > startIndex)
                input = input.Substring(startIndex + 1, endIndex - startIndex - 1);
            
            return input;
        }


        public record DelegateInfo
        {
            public string VarName { get; set; }
            public Delegate Delegate { get; set; }
            public ParameterExpression[] parameterExpressions { get; set; }
            public Type type { get; set; }
        }
    }
}
