using System.Collections.Generic;
using DialogueSystem.Nodes;
using DialogueSystem.Ports;
using System.Text;
using System;

namespace DialogueSystem.Generators
{
    internal class PropFieldGen : BaseGeneratorHelper
    {
        private Dictionary<DSClass, Dictionary<BaseNode, string>> declaredProperties = new();
        private VariablesGen variablesGen;

        internal PropFieldGen(VariablesGen variablesGen) 
        {
            this.variablesGen = variablesGen;
        }


        internal string GetDeclaratedPropField(DSClass dsClass, BaseNode node, bool isAutoproperty = true, Visibility visibility = Visibility.@public, Attribute attribute = Attribute.None)
        {
            if (declaredProperties.TryGetValue(dsClass, out Dictionary<BaseNode, string> nodeLibs))
            {
                if (nodeLibs.TryGetValue(node, out string propname)) return propname;
                else
                {
                    var property = GenerateDeclaredProperty(dsClass, node, isAutoproperty, visibility, attribute);
                    nodeLibs = new()
                    {
                        { node, property }
                    };
                    return property;
                }
            }
            else
            { 
                var property = GenerateDeclaredProperty(dsClass, node, isAutoproperty, visibility, attribute);
                nodeLibs = new()
                    {
                        { node, property }
                    };
                declaredProperties.Add(dsClass, nodeLibs);
                return property;
            }
        }

        private string GenerateDeclaredProperty(DSClass dsClass, BaseNode node, bool isAutoproperty = true, Visibility visibility = Visibility.@public, Attribute attribute = Attribute.None)
        {
            StringBuilder sb = new StringBuilder()
                        .Append(GetAttribute(attribute))
                        .Append(GetVisibility(visibility))
                        .Append(SPACE)
                        .Append(dsClass.GetClassType())
                        .Append(SPACE)
                        .Append(variablesGen.GetMainClassVariable(node));

            if (isAutoproperty)
            {
                sb.Append(SPACE)
                .Append(APROP);
            }
            return sb.ToString();
        }
    }
}
