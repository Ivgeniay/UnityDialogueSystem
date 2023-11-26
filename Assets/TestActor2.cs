using DialogueSystem.Characters;

namespace DialogueSystem.Assets
{
    public class TestActor2 : IDialogueActor
    {
        public int Ammo = 2;

        private DialogueFileNameR dialogueFileName;

        public void Awake()
        {
            dialogueFileName = new();
            //dialogueFileName.Initialize(this);
        }
    }
}
