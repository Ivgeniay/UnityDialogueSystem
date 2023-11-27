using DialogueSystem.Nodes;
using DialogueSystem.Window;
using System.IO;
using System.Text;
using UnityEngine;

namespace DialogueSystem.Generators
{
    internal class Generator 
    {
        private DSGraphView graphView;
        public Generator(DSGraphView view)
        {
            graphView = view;
        }

        internal void Generate(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                Debug.Log("Generate is not done. Path is incorrect.");
                return;
            }
            string className = Path.GetFileNameWithoutExtension(path);
            className = string.IsNullOrEmpty(path) == true ? "MyClass" : className;
            ScriptGen scrGen = new(graphView, path.Substring(0, path.IndexOf(className) - 1), className);

            string _script = scrGen.Draw(new StringBuilder());
            scrGen.Build();
        }
    }
}
