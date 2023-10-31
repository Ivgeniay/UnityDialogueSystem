using DialogueSystem.Window;
using UnityEngine;

namespace DialogueSystem.Nodes
{
    internal class BoolNode : BaseLogicNode
    {
        internal override void Initialize(DialogueSystemGraphView graphView, Vector2 position)
        {
            base.Initialize(graphView, position);
            Outputs.Add(new(ID)
            {
                PortText = typeof(bool).Name,
                Cross = false,
                IsField = true,
                IsInput = false,
                IsSingle = false,
                Type = typeof(bool),
                Value = false,
            });
        }
    }
}
