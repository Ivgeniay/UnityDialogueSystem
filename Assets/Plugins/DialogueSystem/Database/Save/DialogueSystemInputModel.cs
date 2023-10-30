using System;
using UnityEngine;

namespace DialogueSystem.Database.Save
{
    [Serializable]
    public class DialogueSystemInputModel
    {
        [field: SerializeField] public string NodeID { get; set; }
        [field: SerializeField] public string PortText { get; set; }
        [field: SerializeField] public Type Type { get; set; }
        [field: SerializeField] public object Value { get; set; }
        public DialogueSystemInputModel(string nodeID, Type type = null, string text = null) { }
    }
}
