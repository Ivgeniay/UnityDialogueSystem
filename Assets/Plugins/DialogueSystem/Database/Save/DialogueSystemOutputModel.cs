using UnityEngine;
using System;
using DialogueSystem.Ports;

namespace DialogueSystem.Database.Save
{
    [Serializable]
    public class DialogueSystemOutputModel
    {
        [field: SerializeField] public string NodeID { get; set; }
        [field: SerializeField] public object Value { get; set; }
        [field: SerializeField] public Type PortType { get; set; }
        public DialogueSystemOutputModel(string nodeID, object value = null) 
        { 
            this.NodeID = nodeID;
            this.Value = value;
        }
    }
}
