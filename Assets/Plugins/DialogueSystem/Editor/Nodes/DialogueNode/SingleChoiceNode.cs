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

            Outputs.Add(new DialogueSystemPortModel(ID, new System.Type[] 
            { 
                typeof(string) 
            })
            {
                Value = "Next Dialogue",
                Cross = false,
                IsField = true,
                IsInput = false,
                IsSingle = false,
                PortText = string.Empty,
                Type = typeof(string),
            });
        }
    }
}
