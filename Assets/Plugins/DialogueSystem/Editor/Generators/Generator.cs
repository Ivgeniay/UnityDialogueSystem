using DialogueSystem.Nodes;
using DialogueSystem.Window;
using System.Text;

namespace DialogueSystem.Generators
{
    internal class Generator 
    {
        private DSGraphView graphView;
        public Generator(DSGraphView view)
        {
            graphView = view;
        }

        internal void Generate(string filename)
        {
            string className = string.IsNullOrEmpty(filename) == true ? "MyClass" : filename;
            ScriptGen scrGen = new(graphView, filename);

            var _script = scrGen.Draw(new StringBuilder());
            scrGen.Build();
        }
    }
}
