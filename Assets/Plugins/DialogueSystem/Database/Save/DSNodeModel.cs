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
        [field: SerializeField] public object Value { get; set; }
        [field: SerializeField] public List<DSPortModel> Choices { get; set; }
        [field: SerializeField] public string GroupID { get; set; }
        [field: SerializeField] public string Text { get; set; }
        [field: SerializeField] public Type DialogueType { get; set; }
        [field: SerializeField] public Vector2 position { get; set; }
    }
}
