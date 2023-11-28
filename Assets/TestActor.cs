using DialogueSystem.Characters;
using UnityEngine;
using System;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using DialogueSystem.Database.Save;
using static UnityEditor.Rendering.CameraUI;
using UnityEditor;
using System.IO;

namespace DialogueSystem.Assets
{
    public class TestActor : MonoBehaviour, IDialogueActor
    {
        public int Age;
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
