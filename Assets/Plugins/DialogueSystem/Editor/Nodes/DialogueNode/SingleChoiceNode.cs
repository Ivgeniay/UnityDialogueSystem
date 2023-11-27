using DialogueSystem.Utilities;
using DialogueSystem.Window;
using DialogueSystem.Database.Save;
using UnityEngine;
using System.Collections.Generic;

namespace DialogueSystem.Nodes
{
    internal class SingleChoiceNode : BaseDialogueNode
    {
        internal override void Initialize(DSGraphView graphView, Vector2 position, List<object> portsContext)
        {
            base.Initialize(graphView, position, portsContext: portsContext);

            if (portsContext == null)
            {
                Model.AddPort(new DSPortModel(DSConstants.DialogueTypes, Ports.PortSide.Output)
                {
                    Value = null,
                    Cross = false,
                    IsField = true,
                    IsInput = false,
                    IsSingle = true,
                    PortText = DSConstants.Dialogue,
                    Type = DSConstants.DialogueTypes[0]
                });
            }
        }
    }
}
