using DialogueSystem.Characters;
using UnityEngine;

namespace DialogueSystem.Assets
{
    public class TestActor : IDialogueActor
    {
        public string Name { get; set; }
        public float attack;
        public bool IsHangry;

    }
}
