using DialogueSystem.SDictionary;
using System.Collections.Generic;
using UnityEngine;

namespace DialogueSystem.Database.Save
{
    public class DialogueSystemContainerSO : ScriptableObject
    {
        [field: SerializeField] public string FileName {  get; set; }
        [field: SerializeField] public SerializableDictionary<DialogueSystemGroupSO, List<DialogueSystemDialogueSO>> DialogueGroups { get; set; }
        [field: SerializeField] public List<DialogueSystemDialogueSO> UngroupedDialogues { get; set; }

        public void Init(string fileName)
        {
            FileName = fileName;
            DialogueGroups = new();
            UngroupedDialogues = new();
        }
    }
}
