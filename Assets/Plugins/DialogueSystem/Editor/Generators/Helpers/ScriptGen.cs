using DialogueSystem.Window;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace DialogueSystem.Generators
{
    internal class ScriptGen
    {
        private UsingGen usingGen;
        private object[] scriptContext;
        private DSGraphView dsGrathView;
        private string className;

        public string script;
        ClassGenerator classGenerator = null;

        internal ScriptGen(DSGraphView dsGrathView, string className)
        {
            this.dsGrathView = dsGrathView;
            this.className = className;
            usingGen = new(scriptContext);
        }

        internal string Draw(StringBuilder context)
        {
            classGenerator = new(this.dsGrathView, className);
            classGenerator.Initialize();

            ClassDrawer mainClassDrawer = classGenerator.GetDrawer();

            script += mainClassDrawer.Draw();
            script = DrawTabs(script);
            return script;
        }

        private string DrawTabs(string str)
        {
            int indentLevel = 0;
            StringBuilder result = new StringBuilder();
            bool inAutoProperty = false;

            for (int i = 0; i < str.Length; i++)
            {
                char c = str[i];

                if (c == '{')
                {
                    indentLevel++;
                    result.Append(c);

                    if (i + 3 < str.Length && str.Substring(i + 1, 3) == "get")
                    {
                        inAutoProperty = true;
                        result.Append("get");
                        i += 3; // Пропустим символы "get"
                    }

                    if (!inAutoProperty)
                    {
                        result.Append("\n");
                        result.Append(new string('\t', indentLevel));
                    }
                }
                else if (c == '}')
                {
                    indentLevel--;
                    if (!inAutoProperty)
                    {
                        result.Append("\n");
                        result.Append(new string('\t', indentLevel));
                    }
                    result.Append(c);

                    inAutoProperty = false;
                }
                else if (c == '\n')
                {
                    if (!inAutoProperty)
                    {
                        result.Append(c);
                        result.Append(new string('\t', indentLevel));
                    }
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
            //string filePath = Application.dataPath + "/" + Class.GetClassName() + ".cs";
            //File.WriteAllText(filePath, script);
            //UnityEditor.AssetDatabase.Refresh();
#endif
        }
    }

}
