using DialogueSystem.SDictionary;
using System.Collections.Generic;
using UnityEngine;

namespace DialogueSystem.Database.Save
{
    public class DSContainerSO : ScriptableObject
    {
        [field: SerializeField] public string FileName {  get; set; }
        [field: SerializeField] public SerializableDictionary<DSGroupSO, List<DSDialogueSO>> DialogueGroups { get; set; }
        [field: SerializeField] public List<DSDialogueSO> UngroupedDialogues { get; set; }

        public void Init(string fileName)
        {
            FileName = fileName;
            DialogueGroups = new();
            UngroupedDialogues = new();
        }
    }
}
