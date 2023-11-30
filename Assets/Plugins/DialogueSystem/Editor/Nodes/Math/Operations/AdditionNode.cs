using System.Collections.Generic;
using DialogueSystem.Generators;
using DialogueSystem.Utilities;
using System.Linq.Expressions;
using System.Linq;
using System.Text;
using System;

namespace DialogueSystem.Nodes
{
    internal class AdditionNode : BaseOperationNode
    {
        internal override string LambdaGenerationContext(MethodParamsInfo[] inputVariables, MethodParamsInfo[] outputVariables)
        {
            StringBuilder sb = new();
            sb.Append("return ");
            for (int i = 0; i < inputVariables.Length; i++)
            {
                if (inputVariables[i].ParamType == typeof(bool)) sb.Append($"{inputVariables[i].ParamName} == true ? 1 : 0");
                else sb.Append($"{inputVariables[i].ParamName}");
                if (i != inputVariables.Length - 1) sb.Append(" + ");
            }
            sb.Append(';');

            return sb.ToString();
        }

        internal Delegate LambdaGenerationContext(ParameterExpression[] parameters)
        {
            var anyString = parameters.Any(e => e.Type == typeof(string));
            LambdaExpression lambda = null;
            if (!anyString)
            {
                List<Expression> convertExpressions2 = new List<Expression>();
                foreach (ParameterExpression param in parameters)
                    if (DSConstants.NumberTypes.Contains(param.Type)) convertExpressions2.Add(Expression.Convert(param, typeof(double)));
                Expression sum = convertExpressions2.Aggregate((acc, exp) => Expression.Add(acc, exp));
                lambda = Expression.Lambda(sum, parameters);
            }
            else
            {
                Expression[] stringExpressions = parameters
                    .Select(param => Expression.Call(param, typeof(object).GetMethod("ToString")))
                    .ToArray();

                MethodCallExpression concatExpression = Expression.Call(
                    typeof(string).GetMethod("Concat", new[] { typeof(string[]) }),
                    Expression.NewArrayInit(typeof(string), stringExpressions)
                );

                lambda = Expression.Lambda(concatExpression, parameters);
            }


            return lambda.Compile();
        }
    }
}
