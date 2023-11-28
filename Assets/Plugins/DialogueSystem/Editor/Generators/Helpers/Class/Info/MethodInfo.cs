using DialogueSystem.Generators;
using System;

namespace DialogueSystem.Generators
{
    internal class MethodInfo : Info
    {
        public Visibility Visibility;
        public string Name; 
        public MethodParamsInfo[] Parameters;
        public string[] ReturnParameters;
    }
}
