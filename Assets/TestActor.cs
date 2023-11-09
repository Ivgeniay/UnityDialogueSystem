using DialogueSystem.Characters;
using UnityEngine;
using System;

namespace DialogueSystem.Assets
{
    public class TestActor : MonoBehaviour, IDialogueActor
    {
        public int Age;
        public string Name;
        public int Intellect { get; private set; }

        private void Awake()
        {
            
        }
    }
}
