using DialogueSystem.Generators;
using System;

namespace DialogueSystem.Generators
{
    internal class MethodInfo
    {
        public Visibility Visibility;
        public string Name; 
        public MethodParamsInfo[] Parameters;
        public Type[] ReturnParameters;
    }
}
