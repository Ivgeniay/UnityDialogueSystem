using DialogueSystem.Characters;
using System;
using System.Reflection;
using UnityEngine;

namespace DialogueSystem.Assets
{
    public class TestActor : MonoBehaviour, IDialogueActor
    {
        public string Name { get; set; }
        public float attack;
        public float attack228;
        public bool IsHangry;

        private void Awake()
        {
            
        }
    }
}
