using UnityEngine;
using System;

namespace DialogueSystem.Database.Save
{
    [Serializable]
    public class DialogueSystemPortModel
    {
        [field: SerializeField] public string NodeID { get; set; }
        [field: SerializeField] public object Value { get; set; }
        [field: SerializeField] public string PortText { get; set; }
        [field: SerializeField] public bool IsSingle { get; set; }
        [field: SerializeField] public bool IsInput { get; set; }
        [field: SerializeField] public bool Cross { get; set; }
        [field: SerializeField] public bool IsField { get; set; }
        [field: SerializeField] public Type Type { get; set; }
        public DialogueSystemPortModel(string nodeID, Type type = null, string text = null, object value = null) 
        { 
            this.NodeID = nodeID;
            this.Value = value;
        }
    }
}
