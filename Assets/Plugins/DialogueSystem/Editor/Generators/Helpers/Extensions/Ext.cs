using DialogueSystem.Nodes;
using DialogueSystem.Ports;

namespace DialogueSystem.Generators
{
    internal static class Ext
    {
        internal static void GetVariable(this ClassGen classGen, BasePort node) =>
            classGen.PropFieldGen.GetVariable(node);
        internal static void GeneratePropField(this ClassGen classGen, BaseNode node, bool isAutoproperty = true, Visibility visibility = Visibility.Public, Attribute attribute = Attribute.None) =>
            classGen.PropFieldGen.GeneratePropField(node, isAutoproperty, visibility, attribute);
        
    }
}
