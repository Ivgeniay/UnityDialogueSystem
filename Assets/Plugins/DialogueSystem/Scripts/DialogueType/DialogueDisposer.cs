using System.Collections.Generic;
using UnityEngine;
using System;
using static DialogueSystem.DialogueDisposer.DSDialogueOption;

namespace DialogueSystem
{
    public class DialogueDisposer
    {
        protected void Dialogue(DSDialogue dia)
        {
            Debug.Log("Dialogue text: " + dia.Text);
            var opt = dia.GetOptions();
            foreach (var op in opt) Option(op);
        }
        protected void Option(DSDialogueOption option)
        {
            Debug.Log("Options text: " + option.Text);
            if (option.NextDialogue != null) Dialogue(option.NextDialogue);
        }

        public record DSDialogueOption
        {
            public string Text { get; private set; }
            public DSDialogue NextDialogue { get; private set; }
            private Func<bool> Func { get; set; }

            public DSDialogueOption(string text, DSDialogue nextDialogues = null, Func<bool> func = null)
            {
                this.Text = text;
                this.NextDialogue = nextDialogues;
                Func = func == null ? () => true : func;
            }

            public class DSDialogue
            {
                #region Fields
                [SerializeField] public System.String Text;
                [SerializeField] public List<DSDialogueOption> DSDialogueOption = new();
                #endregion

                public IEnumerable<DSDialogueOption> GetOptions()
                {
                    foreach (DSDialogueOption option in DSDialogueOption)
                        if (option.Func()) yield return option;
                }
            }
        }
    }
}
