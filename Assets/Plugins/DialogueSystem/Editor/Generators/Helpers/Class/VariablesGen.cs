using DialogueSystem.Nodes;
using DialogueSystem.Ports;
using DialogueSystem.Text;
using DialogueSystem.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor.Experimental.GraphView;

namespace DialogueSystem.Generators
{
    internal class VariablesGen : BaseGeneratorHelper
    {
        private Dictionary<BaseNode, string> classVariable = new();
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
        internal string GetInnerClassVariable(BasePort port)
        {
            var motherNode = port.node as BaseNode;
            var typeText = DSUtilities.GenerateClassNameFromType(motherNode.GetType());
            var t = innerClassVariable.Where(e => e.GetClassType() == typeText).FirstOrDefault();
            if (t != null)
            {
                var innerVar = t.GetVariable(port);
                var mainVar = GetMainClassVariable(motherNode);
                return mainVar + "." + innerVar.Name;
            }
            throw new NullReferenceException();
        }

        internal string GetAndCallInnerClassVariableFunction(BasePort basePort, params BasePort[] innerFuncVariables)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(GetInnerClassVariable(basePort));
            
            if (basePort.IsFunctions)
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
        //AddNode_1.doubleFunc()

        internal void RegisterDSClass(DSClass dsClass)
        {
            if (!innerClassVariable.Contains(dsClass))
                innerClassVariable.Add(dsClass);
        }
    }
}
