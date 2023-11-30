using System.Collections.Generic;
using UnityEngine;
using System;
using DialogueSystem.Ports;
using System.Linq;
using DialogueSystem.Generators;

namespace DialogueSystem.Database.Save
{
    [Serializable]
    public class DSNodeModel
    {
        [SerializeField] internal List<DSPortModel> Outputs = new();
        [SerializeField] internal List<DSPortModel> Inputs = new();
        [SerializeField] public string DialogueType;
        [SerializeField] public Vector2 Position;
        [SerializeField] public string NodeName;
        [SerializeField] public string GroupID;
        [SerializeField] public string Text;
        [SerializeField] public int Minimal;
        [SerializeField] public string ID;
        [SerializeField] public Visibility Visibility = Visibility.@private;
        [SerializeField] public Generators.Attribute Attribute = Generators.Attribute.None;

        internal void Init()
        {
            Outputs = new List<DSPortModel>();
            Inputs = new List<DSPortModel>();
        }

        internal void AddPort(DSPortModel port)
        {
            if (port.PortSide == Ports.PortSide.Input) Inputs.Add(port);
            else if (port.PortSide == Ports.PortSide.Output) Outputs.Add(port);
        }

    }
}
