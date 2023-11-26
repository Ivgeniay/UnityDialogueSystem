using System.Collections.Generic;
using UnityEngine;
using System;
using DialogueSystem.Ports;
using System.Linq;

namespace DialogueSystem.Database.Save
{
    [Serializable]
    public class DSNodeModel
    {
        [SerializeField] public string ID;
        [SerializeField] public string NodeName;
        [SerializeField] public int Minimal;
        [SerializeField] public List<DSPortModel> Outputs = new();
        [SerializeField] public List<DSPortModel> Inputs = new();
        [SerializeField] public string GroupID;
        [SerializeField] public string Text;
        [SerializeField] public string DialogueType;
        [SerializeField] public Vector2 Position;

        public void Init()
        {
            Outputs = new List<DSPortModel>();
            Inputs = new List<DSPortModel>();
        }

        public void AddPort(DSPortModel port)
        {
            if (port.PortSide == Ports.PortSide.Input) Inputs.Add(port);
            else if (port.PortSide == Ports.PortSide.Output) Outputs.Add(port);
        }

    }
}
