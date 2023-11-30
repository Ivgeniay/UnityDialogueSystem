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
        [SerializeField] private DialogueFileName22 dialogueFileNametest;
        //private DialogueFileNametest dialogueFileNametest;

        private void Awake()
        {
            dialogueFileNametest = new();
            var dialogue = dialogueFileNametest.StartDialogue();
            DialogueFileNametest.TestDialogue(dialogue);
        }

    }
}
