using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DialogueSystem.Lambdas;
using UnityEngine;
using static DialogueSystem.DialogueOption;

namespace DialogueSystem.Assets
{
    public class TestDialogue : MonoBehaviour
    {
        [SerializeField] private DialogueHolder dialogue;

        private void Awake()
        {
            var del = LambdaGenerator.CreateLambda(typeof(int), typeof(float), typeof(string));
            Debug.Log(del);
            //dialogue?.Init();
        }

        public void Initialize()
        {
            //Debug.Log(dialogue.Text);
            //var opt = dialogue.GetNext();
        }

        private void Test()
        {
            //var options = dialogue.GetNext();
            //foreach (var opt in options)
            //{
            //    Debug.Log(opt.Text);
            //}
        }
        

    }
}
