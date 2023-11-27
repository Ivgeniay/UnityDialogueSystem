using DialogueSystem.Nodes;
using DialogueSystem.Window;
using System.Collections.Generic;
using UnityEngine;

namespace DialogueSystem.Nodes
{
    internal class EndDialogueNode : BaseDialogueNode
    {
        internal override void Initialize(DSGraphView graphView, Vector2 position, List<object> portsContext)
        {
            base.Initialize(graphView, position, portsContext);
            if (portsContext == null) Model.Text = "End dialogue";
        }
    }
}
