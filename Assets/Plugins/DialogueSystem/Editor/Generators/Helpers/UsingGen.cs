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
        private object[] scriptContext;
        private string usings;

        public UsingGen(params object[] scriptContext)
        {
            this.scriptContext = scriptContext;
        }

        internal StringBuilder GetUsings(StringBuilder sb, params object[][] @params)
        {
            List<string> strings = new List<string>()
            {
                "UnityEngine",
                "System"
            };

            foreach (var param in @params)
            {
                foreach (var arr in param)
                {
                    var st = GetNamespace(arr);
                    if (!string.IsNullOrEmpty(st) && !string.IsNullOrWhiteSpace(st) && !strings.Contains(st))
                        strings.Add(st);

                }
            }

            foreach (var t in strings) sb.Append($"using {t};\t\n");
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
