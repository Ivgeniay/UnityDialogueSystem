using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static DialogueSystem.DialogueOption;

namespace DialogueSystem
{
    [System.Serializable]
    [CreateAssetMenu(fileName = "Dialogue", menuName = "DES/Dialogue")]
    public class DialogueHolder : ScriptableObject
    {
        [SerializeField] private Dialogue dialogue;
        public void Init()
        {
            dialogue = ScriptableObject.CreateInstance<Dialogue>();
            AssetDatabase.AddObjectToAsset(dialogue, this);
            AssetDatabase.Refresh();
        }
    }

    [System.Serializable]
    public class DialogueOption
    {
        public string Text { get; private set; }
        private DialogueOption(Func<bool> func) => 
            Func = func;

        private IEnumerable<Dialogue> NextDialogues;
        private Func<bool> Func { get; set; }

        [System.Serializable]
        public class Dialogue : ScriptableObject
        {
            public string Text;
            private List<DialogueOption> options;

            private DialogueOption GenerateOption(Func<bool> predicate, IEnumerable<Dialogue> NextDialogues)
            {
                DialogueOption dialogueOption = new(predicate);
                dialogueOption.NextDialogues = NextDialogues;
                return dialogueOption;
            }

            public IEnumerable<DialogueOption> GetNext()
            {
                foreach (DialogueOption option in options)
                    if (option.Func()) yield return option;
            }
        }
    }
}
