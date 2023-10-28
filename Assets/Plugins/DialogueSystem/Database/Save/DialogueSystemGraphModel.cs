using System.Collections.Generic;
using UnityEngine;
using System;
using DialogueSystem.SDictionary;

namespace DialogueSystem.Database.Save
{
    [Serializable]
    public class DialogueSystemGraphModel
    {
        [field: SerializeField] public string FileName { get; set; }
        [field: SerializeField] public List<DialogueSystemGroupModel> Groups { get; set; }
        [field: SerializeField] public List<DialogueSystemNodeModel> Nodes { get; set; }
        [field: SerializeField] public List<string> OldGroupNames { get; set; }
        [field: SerializeField] public List<string> OldUngroupedNodeNames { get; set; }
        [field: SerializeField] public SerializableDictionary<string, List<string>> OldUGroupedNodeNames { get; set; }

        public void Init(string filename)
        {
            FileName = filename;
            Groups = new();
            Nodes = new();
            OldGroupNames = new();
            OldUngroupedNodeNames = new();
            OldUGroupedNodeNames = new();
        }
    }
}
