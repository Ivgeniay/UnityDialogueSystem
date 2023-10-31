using DialogueSystem.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using DialogueSystem.Database.Save;
using System.Threading.Tasks;
using UnityEngine;
using DialogueSystem.Ports;
using DialogueSystem.Utilities;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

namespace DialogueSystem.Nodes
{
    internal class StringNode : LetterNodeBase
    {
        internal override void Initialize(DialogueSystemGraphView graphView, Vector2 position)
        {
            base.Initialize(graphView, position);

            Outputs.Add(new DialogueSystemPortModel(ID, new Type[] 
            { 
                typeof(string) 
            })
            {
                Value = string.Empty,
                Type = typeof(string),
                Cross = false,
                IsField = true,
                IsInput = false,
                IsSingle = false,
                PortText = typeof(string).Name,
            });
        }

       
    }
}
