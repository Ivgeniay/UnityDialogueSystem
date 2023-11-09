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
                Model.Outputs.Add(new DSPortModel(DSConstants.DialogueTypes)
                {
                    Value = null,
                    Cross = false,
                    IsField = true,
                    IsInput = false,
                    IsSingle = false,
                    PortText = DSConstants.Dialogue,
                    Type = DSConstants.DialogueTypes[0],
                    IsSerializedInScript = true,
                });
            }
        }
    }
}
