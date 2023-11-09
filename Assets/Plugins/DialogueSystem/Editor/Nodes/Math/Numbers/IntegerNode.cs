using DialogueSystem.Database.Save;
using DialogueSystem.Window;
using UnityEngine;
using System;
using System.Collections.Generic;
using DialogueSystem.Utilities;

namespace DialogueSystem.Nodes
{
    public class IntegerNode : BasePrimitiveNode
    {
        internal override void Initialize(DSGraphView graphView, Vector2 position, List<object> portsContext)
        {
            base.Initialize(graphView, position, context: portsContext);

            if (portsContext == null)
            {
                Model.Outputs.Add(new DSPortModel(new Type[] { typeof(int) })
                {
                    Value = 0,
                    Type = typeof(int),
                    Cross = false,
                    IsField = true,
                    IsInput = false,
                    IsSingle = false,
                    PortText = DSConstants.Int,
                    IsSerializedInScript = true,
                });
            }
        }
    }
}
