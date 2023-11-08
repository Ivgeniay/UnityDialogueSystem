using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DialogueSystem.Database.Save
{
    public class DSPortModelSO : ScriptableObject
    {
        [SerializeField] public string PortID;
        [SerializeField] public List<NodePortModelSO> NodeIDs;
        [SerializeField] public object Value;
        [SerializeField] public string PortText;
        [SerializeField] public bool IsSingle;
        [SerializeField] public bool IsInput;
        [SerializeField] public bool IsIfPort;
        [SerializeField] public string IfPortSourceId;
        [SerializeField] public bool PlusIf;
        [SerializeField] public bool Cross;
        [SerializeField] public bool IsField;
        [SerializeField] public bool IsFunction;
        [SerializeField] public string Type;
        [SerializeField] public string[] AvailableTypes;

        public void Init(DSPortModel dSPortModel)
        {
            NodeIDs = new List<NodePortModelSO>();

            PortID = dSPortModel.PortID;
            Value = dSPortModel.Value;
            PortText = dSPortModel.PortText;
            IsSingle = dSPortModel.IsSingle;
            IsInput = dSPortModel.IsInput;
            IsIfPort = dSPortModel.IsIfPort;
            Cross = dSPortModel.Cross;
            IsField = dSPortModel.IsField;
            Type = dSPortModel.Type.ToString();
            AvailableTypes = dSPortModel.AvailableTypes;
            IfPortSourceId = dSPortModel.IfPortSourceId;
            PlusIf = dSPortModel.PlusIf;
            IsFunction = dSPortModel.IsFunction;

            if (dSPortModel.NodeIDs != null)
            {
                foreach (var o in dSPortModel.NodeIDs)
                {
                    if (o != null)
                    {
                        NodePortModelSO dSNodePortModelSO = CreateInstance<NodePortModelSO>();

                        dSNodePortModelSO.name = $"(PortModel){PortID}{o.NodeID}{Random.Range(0,100)}";
                        AssetDatabase.AddObjectToAsset(dSNodePortModelSO, this);
                        dSNodePortModelSO.Init(o);

                        NodeIDs.Add(dSNodePortModelSO);
                        AssetDatabase.SaveAssets();
                        AssetDatabase.Refresh();

                    }
                    if (o == null)
                    {

                    }
                }
            }
        }
    }
}
