using DialogueSystem.Abstract;
using DialogueSystem.Nodes;
using DialogueSystem.Ports;
using DialogueSystem.Text;
using DialogueSystem.TextFields;
using DialogueSystem.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine.UIElements;

namespace DialogueSystem.Generators
{
    internal class VariablesGen : BaseGeneratorHelper
    {
        private Dictionary<VisualElement, string> classVariable = new();
        private List<DSClass> innerClassVariable = new();


        internal string GetMainClassVariable(BaseNode node)
        {
            if (node != null)
            {
                if (classVariable.TryGetValue(node, out string varLib)) { }
                else
                {
                    varLib = $"{node.GetType().Name.RemoveWhitespaces().RemoveSpecialCharacters()}_{classVariable.Count}";
                    classVariable.Add(node, varLib);
                }
                return varLib;
            }
            throw new NullReferenceException();
        }

        internal string GetInnerClassVariable(IDataHolder dataHolder)
        {
            VisualElement currentElement = dataHolder as VisualElement;
            while (currentElement != null && !(currentElement is BaseNode))
                currentElement = currentElement.parent;
            
            var myNode = currentElement as BaseNode;
            if (myNode != null)
            {
                var typeText = "";

                if (myNode is ActorNode actorNode) typeText = actorNode.ActorType.ToString();
                else typeText = DSUtilities.GenerateClassNameFromType(myNode.GetType());

                DSClass t = innerClassVariable.Where(e => e.GetClassType() == typeText).FirstOrDefault();
                if (t != null)
                {
                    var innerVar = t.GetVariable(dataHolder);
                    var mainVar = GetMainClassVariable(myNode);
                    return mainVar + "." + innerVar.Name;
                }
                throw new NullReferenceException();
            }
            throw new NullReferenceException();
        }
        internal string GetAndCallInnerClassVariableFunction(IDataHolder dataHolder, params BasePort[] innerFuncVariables)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(GetInnerClassVariable(dataHolder));
            
            if (dataHolder.IsFunctions)
            {
                sb.Append(SPACE).Append(BR_OP);
                for (int i = 0; i < innerFuncVariables.Length; i++)
                {
                    sb.Append(GetAndCallInnerClassVariableFunction(innerFuncVariables[i]));
                    if (i != innerFuncVariables.Length - 1) sb.Append(COMMA);
                }
                sb.Append(BR_CL);
            }
            return sb.ToString();
        }

        internal void RegisterDSClass(DSClass dsClass)
        {
            if (!innerClassVariable.Contains(dsClass))
                innerClassVariable.Add(dsClass);
        }
    }
}
