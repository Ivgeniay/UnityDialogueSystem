using System.Linq.Expressions;
using System.Linq;
using System;

namespace DialogueSystem.Lambdas
{
    public
    //internal
    class LambdaGenerator
    {
        public static Delegate CreateLambda(params Type[] types)
        {
            //ParameterExpression[] parameters = types.Select(Expression.Parameter).ToArray();
            //Expression body = parameters.Aggregate((x, y) => Expression.Add(x, y));
            //LambdaExpression lambda = Expression.Lambda(body, parameters);

            //return lambda.Compile();
            return null;
        }
    }
}
