using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DialogueSystem.Generators
{
    internal class UsingGen : GHelper
    {
        private List<string> usingsList;
        private string usings;

        public UsingGen(params string[] scriptContext)
        {
            usingsList = scriptContext.ToList();
        }

        internal StringBuilder GetUsings(StringBuilder sb, params object[] gettingNamespaces)
        {
            foreach (var arr in gettingNamespaces)
            {
                var st = GetNamespace(arr);
                if (!string.IsNullOrEmpty(st) && !string.IsNullOrWhiteSpace(st) && !usingsList.Contains(st))
                    usingsList.Add(st);
            }
            foreach (var t in usingsList) sb.Append($"using {t};\t\n");
            sb.Append($"\n");
            return sb;
        }

        
        internal string GetNamespace(object t)
        {
            var names = t.GetType().Namespace;
            if (names == "System")
            {
                string assemblyFullName = "Assembly-CSharp";
                Assembly assembly = Assembly.Load(assemblyFullName);
                Type ty = assembly.GetType(t.ToString());
                names = ty.Namespace;
            }

            return names;
        }
    }
}
