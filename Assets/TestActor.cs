using DialogueSystem.Characters;
using UnityEngine;
using System;
using System.Linq.Expressions;
using System.Linq;

using System.CodeDom.Compiler;
using Microsoft.CSharp;
using System.Reflection;
using DialogueSystem.Utilities;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using NUnit.Framework.Internal;

namespace DialogueSystem.Assets
{
    public class TestActor : MonoBehaviour, IDialogueActor
    {
        public int Age;
        public string Name;
        [SerializeField] private Tttt tttt;
        public int Intellect { get; private set; }

        private void Awake()
        {
            
        }

        [Serializable]
        private class Tttt
        {
            public string Name;
        }
    }
}
