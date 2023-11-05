using DialogueSystem.Nodes;
using DialogueSystem.Ports;
using DialogueSystem.Text;
using System;
using System.Collections.Generic;

namespace DialogueSystem.Generators
{
    internal class VariablesGen : BaseGeneratorHelper
    {
        private readonly Dictionary<Type, Dictionary<BasePort, string>> variables = new();

        internal string GetVariable(BasePort port)
        {
            if (port != null)
            {
                if (variables.TryGetValue(port.GetType(), out Dictionary<BasePort, string> vars))
                {
                    if (vars.TryGetValue(port, out string variableName)) return variableName;
                    else
                    {
                        var node = port.node as BaseNode;
                        variableName = $"{node.Model.NodeName.RemoveWhitespaces().RemoveSpecialCharacters()}_{port.portType.Name}_{vars.Count}";
                        vars.Add(port, variableName);
                        return variableName;
                    }
                }
                else
                {
                    var node = port.node as BaseNode;
                    var dic = new Dictionary<BasePort, string>();

                    var variableName = $"{node.Model.NodeName.RemoveWhitespaces().RemoveSpecialCharacters()}_{port.portType.Name}_{dic.Count}";
                    dic.Add(port, variableName);
                    variables.Add(port.GetType(), dic);
                    return variableName;
                }
            }
            throw new NullReferenceException();
        }
    }
}
