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
            BasePrimitiveNode[] numbersN      = graphView.GetNodesOfType<BasePrimitiveNode>();   
            ActorNode[] actorN              = graphView.GetNodesOfType<ActorNode>();         
            BaseConvertNode[] convertesN    = graphView.GetNodesOfType<BaseConvertNode>();   
            BaseOperationNode[] operationN  = graphView.GetNodesOfType<BaseOperationNode>(); 
            BaseLogicNode[] logicN          = graphView.GetNodesOfType<BaseLogicNode>();     
            BaseLetterNode[] letterN        = graphView.GetNodesOfType<BaseLetterNode>();    
            BaseDialogueNode[] dialogN      = graphView.GetNodesOfType<BaseDialogueNode>();  

            var className = string.IsNullOrEmpty(filename) == true ? "MyClass" : filename;
            ScriptGen scrGen = new(actorN, numbersN, convertesN, operationN, logicN, letterN, dialogN);

            scrGen.Class.SetClassName(className);

            var _script = scrGen.Draw(new StringBuilder());
            scrGen.Build();
        }
    }
}
