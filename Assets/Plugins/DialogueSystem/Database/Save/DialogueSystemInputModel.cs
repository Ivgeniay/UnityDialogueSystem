using System;
using UnityEngine;

namespace DialogueSystem.Database.Save
{
    [Serializable]
    public class DialogueSystemInputModel
    {
        [field: SerializeField] public string NodeID { get; set; }
        [field: SerializeField] public string PortText { get; set; }
        public DialogueSystemInputModel(string NodeID, string Text = null) { }
    }
}
