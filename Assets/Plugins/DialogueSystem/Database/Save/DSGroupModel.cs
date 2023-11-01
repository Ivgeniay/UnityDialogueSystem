using System.Collections.Generic;
using UnityEngine;
using System;

namespace DialogueSystem.Database.Save
{
    [Serializable]
    public class DSGroupModel
    {
        [field: SerializeField] public string ID { get; set; }
        [field: SerializeField] public Type Type { get; set; }
        [field: SerializeField] public string GroupName { get; set; }
        [field: SerializeField] public Vector2 Position { get; set; }
    }
}
