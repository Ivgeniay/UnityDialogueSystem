using System;
using System.Collections.Generic;
using UnityEngine;

namespace DialogueSystem.DialogueType
{
    public class Template : MonoBehaviour
    {
        public float int1 { get; set; } = 5;
        public float int2 { get; set; } = 2;
        public string str2 { get; set; } = "string";
        public Func<int, int, string> convert1 { get; set; }
        private Dictionary<int, Dialogue> dialogues { get; set; } = new ();

        public void StartDialogue()
        {
            convert1 = (int1, int2) =>
            {
                return (int1 + int2).ToString();
            };
        }

    }
}
