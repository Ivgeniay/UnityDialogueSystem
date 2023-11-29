using DialogueSystem.Characters;
using UnityEngine;

namespace DialogueSystem.Assets
{
    public class TestActor : MonoBehaviour, IDialogueActor
    {
        public int Age;
        public int Level;
        public string Name;
        public int Intellect { get; private set; }
        private DialogueFileName222 dialogueFileName;

        private void Awake()
        {
            dialogueFileName = new();
            //var startDialogue = dialogueFileName.StartDialogue(this);
            var startDialogue = dialogueFileName.StartDialogue();
            DialogueFileName222.TestDialogue(startDialogue);
        }

    }
}
