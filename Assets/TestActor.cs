using DialogueSystem.Attributes;
using DialogueSystem.Characters;
using UnityEngine;

namespace DialogueSystem.Assets
{
    public class TestActor : MonoBehaviour, IDialogueActor
    {
        public DialogueDisposer.DSDialogueOption.DSDialogue dialogue22;
        [DSActorAttributes(Order = 0, Description = "Person age")]
        public int Age;
        public int Level;
        public string Name;
        public int Intellect { get; private set; }
        [SerializeField] private DialogueFileName22 dialogueFileNametest;
        //private DialogueFileNametest dialogueFileNametest;

        private void Awake()
        {
            dialogueFileNametest = new();
            DialogueDisposer.DSDialogueOption.DSDialogue dialogue = null;
            //dialogue = dialogueFileNametest.StartDialogue(this);
            DialogueFileNametest.TestDialogue(dialogue);
        }

    }
}
