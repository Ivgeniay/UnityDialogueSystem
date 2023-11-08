using DialogueSystem.Nodes;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static DialogueSystem.DialogueOption;

namespace DialogueSystem
{



    [System.Serializable]
    public record DialogueOption
    {
        public string Text { get; private set; }
        public Dialogue nextDialogues { get; private set; }
        private Func<bool> Func { get; set; }

        public DialogueOption(string text, Dialogue nextDialogues = null, Func<bool> func = null)
        {
            this.Text = text;
            this.nextDialogues = nextDialogues;
            Func = func == null ? () => true : func;
        }

        public Dialogue GetNextDialogue() => nextDialogues;



        [System.Serializable]
        public class Dialogue
        {
            public string Text { get; private set; }
            private List<DialogueOption> options;
            public Dialogue(string text, List<DialogueOption> options = null)
            {
                Text = text;
                this.options = options;
            }

            public IEnumerable<DialogueOption> GetOptions()
            {
                foreach (DialogueOption option in options)
                    if (option.Func()) yield return option;
            }
        }
    }

    public class Test
    {

        private Dialogue dialogue;

        private TestActor ActorNode_2512 { get; set; }
        public int AgeNode_225 { get; set; } = 25;

        private bool BoolNode_53()
        {
            return ActorNode_2512.Streght > AgeNode_225;
        }

        public Dialogue StartDialogue(TestActor testActor)
        {
            Initialize(testActor);
            return null;
        }

        private void Initialize(TestActor testActor)
        {
            this.ActorNode_2512 = testActor;

            dialogue = new("Dialogue Text", new List<DialogueOption>()
            {
                new DialogueOption(
                    text: "Dialogue Option1",
                    nextDialogues: new Dialogue("Dialogue Text2", new List<DialogueOption>()
                    {
                        //new DialogueOption("Dialogue Option4",)
                    }),
                    func: () => BoolNode_53()),
                new DialogueOption(
                    text: "Dialogue Option2",
                    func:() => true),
                new DialogueOption(
                    text:"Dialogue Option3",
                    func:() => true)
            });
        }

    }

    public class TestActor
    {
        public int Age;
        public int Streght;
    }
}
