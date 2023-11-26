using System;
using System.Collections.Generic;
using System.Linq;
//using static DialogueSystem.DialogueOption;

namespace DialogueSystem
{
    [System.Serializable]
    public record DialogueOption
    {
        public string Text { get; private set; }
        public object NextDialogues { get; private set; }
        private Func<bool> Func { get; set; }

        public DialogueOption(string text, object nextDialogues = null, Func<bool> func = null)
        {
            this.Text = text;
            this.NextDialogues = nextDialogues;
            Func = func == null ? () => true : func;
        }


        [System.Serializable]
        public class Dialogue
        {
            public string Text { get; private set; }
            private List<DialogueOption> options;

            public IEnumerable<DialogueOption> GetOptions()
            {
                foreach (DialogueOption option in options)
                    if (option.Func()) yield return option;
            }
        }
    }

    public class TestActor
    {
        public int Age;
        public int Streght;
    }
}
