using DialogueSystem.Database.Save;
using DialogueSystem.Utilities;
using DialogueSystem.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DialogueSystem.Nodes
{
    internal class StartDialogueNode : BaseDialogueNode
    {
        internal override void Initialize(DSGraphView graphView, Vector2 position, List<object> portsContext)
        {
            base.Initialize(graphView, position, portsContext);
            if (portsContext == null)
            {
                Model.Inputs.Clear();
            }

            if (portsContext == null)
            {
                Model.AddPort(new DSPortModel(DSConstants.DialogueTypes, Ports.PortSide.Output)
                {
                    Value = "Hello",
                    Cross = false,
                    IsField = true,
                    IsInput = false,
                    IsSingle = true,
                    PlusIf = false,
                    PortText = DSConstants.Dialogue,
                    Type = DSConstants.DialogueTypes[0],
                    IsSerializedInScript = true,
                });
            }
            Model.Text = "Start dialogue";
        }
    }
}
