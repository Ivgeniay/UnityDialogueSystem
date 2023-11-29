using DialogueSystem.Window;
using System;
using System.Collections.Generic;
using DialogueSystem.Database.Save;
using DialogueSystem.Utilities;
using UnityEngine;

namespace DialogueSystem.Nodes
{
    internal class StringNode : BaseLetterNode
    {
        internal override void Initialize(DSGraphView graphView, Vector2 position, List<object> portsContext)
        {
            base.Initialize(graphView, position, context: portsContext);

            if (portsContext == null)
            {
                Model.AddPort(new DSPortModel(new Type[] { typeof(string) }, Ports.PortSide.Output)
                {
                    Value = string.Empty,
                    Type = typeof(string),
                    Cross = false,
                    IsField = true,
                    IsInput = false,
                    IsSingle = false,
                    PortText = DSConstants.String
                });
            }
        }

       
    }
}
