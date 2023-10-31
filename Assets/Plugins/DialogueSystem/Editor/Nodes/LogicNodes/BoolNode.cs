using DialogueSystem.Window;
using System.Collections.Generic;
using UnityEngine;

namespace DialogueSystem.Nodes
{
    internal class BoolNode : BaseLogicNode
    {
        internal override void Initialize(DialogueSystemGraphView graphView, Vector2 position, List<object> portsContext)
        {
            base.Initialize(graphView, position, portsContext: portsContext);

            if (portsContext == null)
            {
                Outputs.Add(new(new System.Type[]
                {
                    typeof(bool)
                })
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
}
