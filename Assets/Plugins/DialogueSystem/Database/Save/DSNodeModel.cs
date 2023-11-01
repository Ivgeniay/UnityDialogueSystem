using System.Collections.Generic;
using UnityEngine;
using System;

namespace DialogueSystem.Database.Save
{
    [Serializable]
    public class DSNodeModel
    {
        [field: SerializeField] public string ID { get; set; }
        [field: SerializeField] public string NodeName { get; set; }
        [field: SerializeField] public int Minimal { get; set; }
        [field: SerializeField] public List<DSPortModel> Outputs { get; set; }
        [field: SerializeField] public List<DSPortModel> Inputs { get; set; }
        [field: SerializeField] public string GroupID { get; set; }
        [field: SerializeField] public string Text { get; set; }
        [field: SerializeField] public string DialogueType { get; set; }
        [field: SerializeField] public Vector2 position { get; set; }

        public DSNodeModel() 
        {
            Outputs = new List<DSPortModel>();
            Inputs = new List<DSPortModel>();
        }

    }
}
