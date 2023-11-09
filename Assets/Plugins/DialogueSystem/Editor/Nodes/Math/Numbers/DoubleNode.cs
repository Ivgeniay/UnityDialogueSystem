using DialogueSystem.Generators;
using System.Collections.Generic;
using DialogueSystem.Utilities;
using DialogueSystem.Window;
using System.Linq;
using System.Text;
using UnityEngine;
using System;
using DialogueSystem.Database.Save;

namespace DialogueSystem.Nodes
{
    internal class DoubleNode : BasePrimitiveNode
    {
        internal override void Initialize(DSGraphView graphView, Vector2 position, List<object> portsContext)
        {
            base.Initialize(graphView, position, context: portsContext);

            if (portsContext == null)
            {
                Model.Outputs.Add(new DSPortModel(new Type[] { typeof(double) })
                {
                    Value = 0f,
                    Type = typeof(double),
                    Cross = false,
                    IsField = true,
                    IsInput = false,
                    IsSingle = false,
                    PortText = DSConstants.Double,
                    IsSerializedInScript = true,
                });
            }
        }

    }
}
