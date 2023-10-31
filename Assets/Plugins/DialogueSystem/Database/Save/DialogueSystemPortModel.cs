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
        [field: SerializeField] public bool IsIfPort { get; set; }
        [field: SerializeField] public bool Cross { get; set; }
        [field: SerializeField] public bool IsField { get; set; }
        [field: SerializeField] public Type Type { get; set; }
        [field: SerializeField] public Type[] AvailableTypes { get; set; }
        public DialogueSystemPortModel(Type[] availableTypes, Type type = null, string portText = null, object value = null) 
        {
            this.Value = value;
            this.AvailableTypes = availableTypes;
            this.Type = type;
            this.PortText = portText;
        }
    }
}
