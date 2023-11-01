using UnityEngine;
using System;
using System.Collections.Generic;
using DialogueSystem.Ports;
using DialogueSystem.Nodes;
using System.Linq;

namespace DialogueSystem.Database.Save
{
    [Serializable]
    public class DSPortModel
    {
        //Собственный ID для сериализации из памяти и соединения с другим портом
        [field: SerializeField] public string PortID { get; set; }
        //ID ноды к которой присоединен
        [field: SerializeField] public List<NodePortModel> NodeIDs { get; set; }
        //Значение по-умолчанию или в процессе работы внутри порта
        [field: SerializeField] public object Value { get; set; }
        //Порт текст
        [field: SerializeField] public string PortText { get; set; }
        //Количество присоединяемых соединение (Одиночное, множественное)
        [field: SerializeField] public bool IsSingle { get; set; }
        //Является ли порт инпутом. (input/output)
        [field: SerializeField] public bool IsInput { get; set; }
        //Является ли порт присоединяемым к другому порту (IfPort)
        [field: SerializeField] public bool IsIfPort { get; set; }
        //Нужен ли этому порту кнопка удаления
        [field: SerializeField] public bool Cross { get; set; }
        //Нужно ли поле для этого порта
        [field: SerializeField] public bool IsField { get; set; }
        //Тип порта
        [field: SerializeField] public Type Type { get; set; }
        //Список типов для присоединения порта
        [field: SerializeField] public Type[] AvailableTypes { get; set; }
        public DSPortModel(Type[] availableTypes, Type type = null, string portText = null, object value = null) 
        {
            PortID = Guid.NewGuid().ToString();

            this.Value = value;
            this.AvailableTypes = availableTypes;
            this.Type = type;
            this.PortText = portText;
        }

        public void AddPort(BaseNode node, BasePort port)
        {
            if (NodeIDs == null) NodeIDs = new();

            var findNodeModel = NodeIDs.Where(el => el.NodeID == node.Model.ID).FirstOrDefault();
            if (findNodeModel == null)
            {
                NodeIDs.Add(new NodePortModel()
                {
                    NodeID = node.Model.ID,
                    PortIDs = new() { port.ID }
                });
            }
            else
            {
                var findPort = findNodeModel.PortIDs.Where(el => el == port.ID);
                if (findPort == null) { findNodeModel.PortIDs.Add(port.ID); }
            }
        }

        public void RemovePort(BaseNode node, BasePort port)
        {
            if (NodeIDs == null) NodeIDs = new();

            var findNodeModel = NodeIDs.Where(el => el.NodeID == node.Model.ID).FirstOrDefault();
            if (findNodeModel == null)
            {
                return;
            }
            else
            {
                var findPort = findNodeModel.PortIDs.Where(el => el == port.ID).FirstOrDefault();
                if (findPort != null)
                    findNodeModel.PortIDs.Remove(port.ID);

                if (findNodeModel.PortIDs.Count == 0)
                    NodeIDs.Remove(findNodeModel);
            }
        }
    }
}
