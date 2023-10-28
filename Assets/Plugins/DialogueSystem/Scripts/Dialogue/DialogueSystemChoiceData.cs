using System;
using UnityEngine;

namespace DialogueSystem.Database.Save
{
    [Serializable]
    public class DialogueSystemChoiceData
    {
        [field: SerializeField] public string NodeID {  get; set; }
        [field: SerializeField] public string Text {  get; set; }
        [field: SerializeField] public DialogueSystemDialogueSO NextDialogue {  get; set; }
    }
}
