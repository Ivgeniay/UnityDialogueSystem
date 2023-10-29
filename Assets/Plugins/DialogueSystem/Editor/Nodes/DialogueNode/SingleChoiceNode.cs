using DialogueSystem.Utilities;
using DialogueSystem.Window;
using DialogueSystem.Database.Save;
using UnityEngine;

namespace DialogueSystem.Nodes
{
    internal class SingleChoiceNode : BaseDialogueNode
    {
        internal override void Initialize(DialogueSystemGraphView graphView, Vector2 position)
        {
            base.Initialize(graphView, position);

            Outputs.Add(new DialogueSystemOutputModel(ID)
            {
                Value = "Next Dialogue"
            });
        }
    }
}
