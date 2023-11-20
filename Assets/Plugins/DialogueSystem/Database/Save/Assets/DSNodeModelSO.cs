using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace DialogueSystem.Database.Save
{
    public class DSNodeModelSO : ScriptableObject
    {
        [SerializeField] public string ID;
        [SerializeField] public string NodeName;
        [SerializeField] public int Minimal;
        [SerializeField] public List<DSPortModelSO> Outputs;
        [SerializeField] public List<DSPortModelSO> Inputs;
        [SerializeField] public string GroupID;
        [SerializeField] public string Text;
        [SerializeField] public string DialogueType;
        [SerializeField] public Vector2 Position;

        public void Init(DSNodeModel dSNodeModel)
        {
            Outputs = new List<DSPortModelSO>();
            Inputs = new List<DSPortModelSO>();

            ID = dSNodeModel.ID;
            NodeName = dSNodeModel.NodeName;
            Minimal = dSNodeModel.Minimal;
            GroupID = dSNodeModel.GroupID;
            Text = dSNodeModel.Text;
            DialogueType = dSNodeModel.DialogueType;
            Position = dSNodeModel.Position;

            foreach (var portModel in dSNodeModel.Outputs)
            {
                DSPortModelSO dSPortModelSO = ScriptableObject.CreateInstance<DSPortModelSO>();
                dSPortModelSO.name = $"(OutputPort){portModel.PortID}";

                AssetDatabase.AddObjectToAsset(dSPortModelSO, AssetDatabase.GetAssetPath(this));
                dSPortModelSO.Init(portModel);
                Outputs.Add(dSPortModelSO);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }

            foreach (var portModel in dSNodeModel.Inputs)
            {
                DSPortModelSO dSPortModelSO = ScriptableObject.CreateInstance<DSPortModelSO>();
                dSPortModelSO.name = $"(InputPort){portModel.PortID}";

                AssetDatabase.AddObjectToAsset(dSPortModelSO, AssetDatabase.GetAssetPath(this));
                dSPortModelSO.Init(portModel);
                Inputs.Add(dSPortModelSO);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }
        }

        public DSNodeModel Deconstruct()
        {
            DSNodeModel Model = new()
            {
                ID = this.ID,
                DialogueType = this.DialogueType,
                Position = this.Position,
                GroupID = this.GroupID,
                Outputs = new(),
                Inputs = new(),
                Minimal = this.Minimal,
                NodeName = this.NodeName,
                Text = this.Text,
                
            };

            DecompForDsPortModelList(Model.Outputs, this.Outputs);
            DecompForDsPortModelList(Model.Inputs, this.Inputs);

            return Model;
        }

        private void DecompForDsPortModelList(List<DSPortModel> dSPortModel, List<DSPortModelSO> dSPortModelSO)
        {
            foreach (DSPortModelSO outputsSO in dSPortModelSO)
            {
                var aTypes = outputsSO.AvailableTypes.Select(x => Type.GetType(x)).ToArray();
                DSPortModel portModel = new DSPortModel(aTypes, outputsSO.PortSide)
                {
                    PortID = outputsSO.PortID,
                    Cross = outputsSO.Cross,
                    IsField = outputsSO.IsField,
                    IsIfPort = outputsSO.IsIfPort,
                    IfPortSourceId = outputsSO.IfPortSourceId,
                    IsInput = outputsSO.IsInput,
                    IsSingle = outputsSO.IsSingle,
                    PortText = outputsSO.PortText,
                    Type = Type.GetType(outputsSO.Type),
                    Value = outputsSO.Value,
                    NodeIDs = new(),
                    IsFunction = outputsSO.IsFunction,
                    IsSerializedInScript = outputsSO.IsSerializedInScript,
                    PlusIf = outputsSO.PlusIf,
                    PortSide = outputsSO.PortSide,
                };

                foreach (NodePortModelSO nodeIds in outputsSO.NodeIDs)
                {
                    var pModel = new NodePortModel()
                    {
                        NodeID = nodeIds.NodeID,
                        PortIDs = new()
                    };
                    foreach (var portConnectionId in nodeIds.PortIDs)
                    {
                        pModel.PortIDs.Add(portConnectionId);
                    }

                    portModel.NodeIDs.Add(pModel);
                }
                dSPortModel.Add(portModel);
            }
        }
    }
}
