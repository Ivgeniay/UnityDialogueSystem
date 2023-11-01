using System;
using System.Collections.Generic;
using UnityEngine;

namespace DialogueSystem.Database.Save
{
    [Serializable]
    public class NodePortModel
    {
        [field: SerializeField] public string NodeID { get; set; }
        [field: SerializeField] public List<string> PortIDs { get; set; }
    }
}
