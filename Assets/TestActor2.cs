using DialogueSystem.Characters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DialogueSystem.Assets
{
    public class TestActor2 : IDialogueActor
    {
        public int Ammo = 2;

        private DialogueFileName dialogueFileName;

        public void Awake()
        {
            dialogueFileName = new();
            dialogueFileName.Initialize(this);
        }
    }
}
