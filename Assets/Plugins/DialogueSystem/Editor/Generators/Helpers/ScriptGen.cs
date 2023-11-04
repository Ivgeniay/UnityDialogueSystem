using System.Text;
using UnityEngine;

namespace DialogueSystem.Generators
{
    internal class ScriptGen
    {
        internal ClassGen Class { get; private set; }

        private UsingGen usingGen;
        private object[] scriptContext;
        public string script;

        internal ScriptGen(params object[] scriptContext) 
        {
            this.scriptContext = scriptContext;
            usingGen = new(scriptContext);
            Class = new(scriptContext);
        }

        internal string Draw(StringBuilder context)
        {
            context = usingGen.Draw(context);
            context.AppendLine("");
            context = Class.Draw(context);

            script = context.ToString();
            script = DrawTabs(script);
            return script;
        }

        private string DrawTabs(string str)
        {
            int indentLevel = 0;
            StringBuilder result = new StringBuilder();

            foreach (char c in str)
            {
                if (c == '{')
                    indentLevel++;
                

                if (c == '}')
                    indentLevel--;
                

                if (c == '\n')
                {
                    result.Append(c);
                    result.Append(new string('\t', indentLevel));
                }
                else
                {
                    result.Append(c);
                }
            }

            return result.ToString();
        }

        internal void Build()
        {
#if UNITY_EDITOR
            Debug.Log(script);
            //File.WriteAllText(filePath, script);
            //UnityEditor.AssetDatabase.Refresh();
#endif
        }
    }
}
