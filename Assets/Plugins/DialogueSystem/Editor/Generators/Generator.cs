using DialogueSystem.Nodes;
using DialogueSystem.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        internal void Generate()
        {
            var numbersN    = graphView.GetNodesOfType<BaseNumbersNode>();       //graphView.i_Nodes.Where(e => e.GetType().IsAssignableFrom(typeof(BaseNumbersNode)));
            var actorN      = graphView.GetNodesOfType<ActorNode>();         // graphView.i_Nodes.Where(e => e.GetType().IsAssignableFrom(typeof(ActorNode)));
            var convertesN  = graphView.GetNodesOfType<BaseConvertNode>();     // graphView.i_Nodes.Where(e => e.GetType().IsAssignableFrom(typeof(BaseConvertNode)));
            var operationN  = graphView.GetNodesOfType<BaseOperationNode>();     // graphView.i_Nodes.Where(e => e.GetType().IsAssignableFrom(typeof(BaseOperationNode)));
            var logicN      = graphView.GetNodesOfType<BaseLogicNode>();         // graphView.i_Nodes.Where(e => e.GetType().IsAssignableFrom(typeof(BaseLogicNode)));
            var letterN     = graphView.GetNodesOfType<BaseLetterNode>();        // graphView.i_Nodes.Where(e => e.GetType().IsAssignableFrom(typeof(BaseLetterNode)));
            var dialogN     = graphView.GetNodesOfType<BaseDialogueNode>();        // graphView.i_Nodes.Where(e => e.GetType().IsAssignableFrom(typeof(BaseDialogueNode)));

            var countNodes = numbersN.Count() + actorN.Count() + convertesN.Count() +
                operationN.Count() + logicN.Count() + letterN.Count() + dialogN.Count();

            var usings = GetUsings(actorN, numbersN, convertesN, operationN, logicN, letterN, dialogN);
            var className = "MyClass";

            string classText = GeneratorUtility.GetClassLine(className);


            string script = usings + classText + GeneratorUtility.BRACKET_OPEN;

            foreach (var el in actorN) script += GeneratorUtility.GeneratePropery(el, Visibility.Internal);
            foreach (var el in letterN) script += GeneratorUtility.GeneratePropery(el, Visibility.Public);
            foreach (var el in numbersN) script += GeneratorUtility.GeneratePropery(el, Visibility.Internal);

            script += GeneratorUtility.BRACKET_CLOSE;


            Debug.Log(script);
        }

        private string GetUsings(object[] actorNodes, params object[] ob)
        {
            var usings = GeneratorUtility.GetUsings(ob);

            if (actorNodes != null && actorNodes.Length > 0)
            {
                var arr = new List<object>();
                foreach (var act in actorNodes)
                {
                    var r = act as ActorNode;
                    if (r.ActorType != null)
                        arr.Add(r.ActorType);
                }

                usings += GeneratorUtility.GetUsings(arr.ToArray());
            }
            return usings;
        }
    }
}
