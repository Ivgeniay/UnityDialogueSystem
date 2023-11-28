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
        private DialogueFileName1 dialogueFileName;

        private void Awake()
        {
            dialogueFileName = new();
            var startDialogue = dialogueFileName.StartDialogue(this);
            DialogueFileName1.TestDialogue(startDialogue);
        }

    }
}
