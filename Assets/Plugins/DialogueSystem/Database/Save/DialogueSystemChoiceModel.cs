using UnityEngine;
using System;

namespace DialogueSystem.Database.Save
{
    [Serializable]
    public class DialogueSystemChoiceModel
    {
        [field: SerializeField] public string Text { get; set; }
        [field: SerializeField] public string NodeID { get; set; }
    }
}
