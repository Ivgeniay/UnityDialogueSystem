using DialogueSystem.Database.Save;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DialogueSystem.Save
{
    public class GraphSO : ScriptableObject
    {
        [SerializeField] public string FileName;
        [SerializeField] public List<DSNodeModelSO> NodeModels;
        [SerializeField] public List<DSGroupModelSO> GroupModels;

        public void Init(string fileName, List<DSNodeModel> nodes, List<DSGroupModel> groups, Action<float, float> callback = null)
        {
            FileName = fileName;
            NodeModels = new List<DSNodeModelSO>();
            GroupModels = new List<DSGroupModelSO>();
            int counter = 0;
            int counValues = nodes.Count + groups.Count;

            foreach (DSGroupModel group in groups)
            {
                DSGroupModelSO dSGroupModel = ScriptableObject.CreateInstance<DSGroupModelSO>();
                dSGroupModel.name = $"{Type.GetType(group.Type).Name}({group.GroupName}";
                AssetDatabase.AddObjectToAsset(dSGroupModel, this);
                dSGroupModel.Init(group);
                this.GroupModels.Add(dSGroupModel);
                AssetDatabase.SaveAssets();
                counter++;
                callback?.Invoke(counter, counValues);
            }

            foreach (DSNodeModel node in nodes)
            {
                DSNodeModelSO dSNodeModelSO = ScriptableObject.CreateInstance<DSNodeModelSO>();
                dSNodeModelSO.name = $"{Type.GetType(node.DialogueType).Name}({node.NodeName}";
                AssetDatabase.AddObjectToAsset(dSNodeModelSO, AssetDatabase.GetAssetPath(this));
                dSNodeModelSO.Init(node);
                this.NodeModels.Add(dSNodeModelSO);
                AssetDatabase.SaveAssets();
                counter++;
                callback?.Invoke(counter, counValues);
            }
        }
    }
}
