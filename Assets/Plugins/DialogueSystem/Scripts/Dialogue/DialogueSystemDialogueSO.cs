using DialogueSystem.Database.Save;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace DialogueSystem.Database.Save
{
    public class DialogueSystemDialogueSO : ScriptableObject
    {
        [field: SerializeField] public string DialogueName { get; set; }
        [field: SerializeField] [field: TextArea()] public string Text { get; set; }
        [field: SerializeField] public List<DialogueSystemChoiceData> Choices { get; set; }
        [field: SerializeField] public Type DialogueType { get; set; }
        [field: SerializeField] public bool IsStartingDialogue { get; set; }

        public void Init(
            string dialogueName,
            string text,
            List<DialogueSystemChoiceData> choices,
            Type dialogueType,
            bool isStartingDialogue)
        {
            this.DialogueName = dialogueName;
            this.Text = text;
            this.Choices = choices;
            this.DialogueType = dialogueType;
            this.IsStartingDialogue = isStartingDialogue;
        }
    }
}
