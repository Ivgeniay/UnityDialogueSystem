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
            BasePrimitiveNode[] numbersN    = graphView.GetArrayNodesOfType<BasePrimitiveNode>();   
            ActorNode[] actorN              = graphView.GetArrayNodesOfType<ActorNode>();         
            BaseConvertNode[] convertesN    = graphView.GetArrayNodesOfType<BaseConvertNode>();   
            BaseOperationNode[] operationN  = graphView.GetArrayNodesOfType<BaseOperationNode>(); 
            BaseLogicNode[] logicN          = graphView.GetArrayNodesOfType<BaseLogicNode>();     
            BaseLetterNode[] letterN        = graphView.GetArrayNodesOfType<BaseLetterNode>();    
            BaseDialogueNode[] dialogN      = graphView.GetArrayNodesOfType<BaseDialogueNode>();  

            string className = string.IsNullOrEmpty(filename) == true ? "MyClass" : filename;
            ScriptGen scrGen = new(graphView, filename);

            var _script = scrGen.Draw(new StringBuilder());
            scrGen.Build();
        }
    }
}
