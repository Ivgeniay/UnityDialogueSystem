using DialogueSystem.Database.Save;
using DialogueSystem.Window;
using UnityEngine;
using System;
using System.Collections.Generic;

namespace DialogueSystem.Nodes
{
    public class IntegerNode : BaseNumbersNode
    {
        internal override void Initialize(DialogueSystemGraphView graphView, Vector2 position, List<object> portsContext)
        {
            base.Initialize(graphView, position, portsContext: portsContext);

            if (portsContext == null)
            {
                Outputs.Add(new DialogueSystemPortModel(new Type[] { typeof(int) })
                {
                    Value = 0,
                    Type = typeof(int),
                    Cross = false,
                    IsField = true,
                    IsInput = false,
                    IsSingle = false,
                    PortText = typeof(int).Name,
                });
            }

            Model.Value = 0;
        }
    }
}
