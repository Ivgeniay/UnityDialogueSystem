using DialogueSystem.Nodes;
using DialogueSystem.Ports;
using System;
using UnityEngine;

namespace DialogueSystem.Database.Save
{
    [Serializable]
    public class DialogueSystemInputModel
    {
        [field: SerializeField] public string NodeID { get; set; }
        [field: SerializeField] public string Text { get; set; }
        [field: SerializeField] public BasePort Port { get; set; }
        public DialogueSystemInputModel(string NodeID, string Text = null) { }
    }
}
