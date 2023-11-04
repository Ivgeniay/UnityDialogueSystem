using DialogueSystem.Nodes;
using DialogueSystem.Window;
using System;
using System.Collections.Generic;
using System.IO;
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

        internal void Generate(string filename)
        {
            BaseNumbersNode[] numbersN      = graphView.GetNodesOfType<BaseNumbersNode>();   
            ActorNode[] actorN              = graphView.GetNodesOfType<ActorNode>();         
            BaseConvertNode[] convertesN    = graphView.GetNodesOfType<BaseConvertNode>();   
            BaseOperationNode[] operationN  = graphView.GetNodesOfType<BaseOperationNode>(); 
            BaseLogicNode[] logicN          = graphView.GetNodesOfType<BaseLogicNode>();     
            BaseLetterNode[] letterN        = graphView.GetNodesOfType<BaseLetterNode>();    
            BaseDialogueNode[] dialogN      = graphView.GetNodesOfType<BaseDialogueNode>();  

            var className = string.IsNullOrEmpty(filename) == true ? "MyClass" : filename;
            ScriptGen scrGen = new(actorN, numbersN, convertesN, operationN, logicN, letterN, dialogN);

            scrGen.Class.SetClassName(className);

            foreach (var el in actorN) scrGen.Class.GeneratePropField(el, true, Visibility.Private, Attribute.FieldSerializeField);
            foreach (var el in letterN) scrGen.Class.GeneratePropField(el, true, Visibility.Private, Attribute.FieldSerializeField);
            foreach (var el in numbersN) scrGen.Class.GeneratePropField(el, true, Visibility.Private, Attribute.FieldSerializeField);

            foreach (var el in operationN)
            {
                scrGen.Class.MethodGen.GetMethod(el, Visibility.Public);
                Debug.Log(scrGen.Class.MethodGen.GetCallMethod(el));
                //string context = scrGen.Class.MethodGen.CallMethod(el, numbersN[0], numbersN[1]);
            }

            var _script = scrGen.Draw(new StringBuilder());
            scrGen.Build();

            //foreach (var el in operationN)
            //{
            //    string context = GU.CallMethod(el, numbersN[0], numbersN[1]);
            //    script += GU.GetMethod(el, Visibility.Public, context: context);
            //}

            //script += GU.BRACKET_CLOSE;

            //Debug.Log(script);
        }
    }
}
