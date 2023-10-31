using DialogueSystem.Database.Save;
using DialogueSystem.Window;
using UnityEngine;
using System;

namespace DialogueSystem.Nodes
{
    public class IntegerNode : BaseNumbersNode
    {
        internal override void Initialize(DialogueSystemGraphView graphView, Vector2 position)
        {
            base.Initialize(graphView, position);
            Outputs.Add(new DialogueSystemPortModel(ID, new Type[] { typeof(int) })
            {
                Value = 0,
                Type = typeof(int),
                Cross = false,
                IsField = true,
                IsInput = false,
                IsSingle = false,
                PortText = typeof(int).Name,
            });

            Model.Value = 0;
        }
    }
}
