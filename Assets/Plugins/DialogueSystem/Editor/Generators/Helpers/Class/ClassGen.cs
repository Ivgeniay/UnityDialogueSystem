using System.Text;

namespace DialogueSystem.Generators
{
    internal class ClassGen : BaseGeneratorHelper
    {
        private object[] scriptContext;             ///????
        private string className = "MyClass";
        internal PropFieldGen PropFieldGen;
        internal MethodGen MethodGen;
        internal VariablesGen VariablesGen;

        internal ClassGen(params object[] scriptContext) 
        { 
            this.scriptContext = scriptContext;
            VariablesGen = new();
            PropFieldGen = new(VariablesGen);
            MethodGen = new(VariablesGen);
        }
        internal void SetClassName(string className) => this.className = className;
        internal string GetClassName() => this.className;

        private StringBuilder GetClassLine(string className, StringBuilder context)
        {
            context.Append("public class ")
                .Append(className);
            return context;
        }

        internal override StringBuilder Draw(StringBuilder context)
        {
            context = GetClassLine(className, context);
            context.Append(TR).Append(BR_F_OP);

            context = PropFieldGen.Draw(context);
            context = MethodGen.Draw(context);

            context.Append(TR).Append(BR_F_CL);

            return context;
        }
    }
}
