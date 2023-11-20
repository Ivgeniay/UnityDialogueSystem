using UnityEditor.Experimental.GraphView;
using DialogueSystem.Database.Error;
using DialogueSystem.Database.Save;
using DialogueSystem.SDictionary;
using System.Collections.Generic;
using DialogueSystem.Generators;
using DialogueSystem.Utilities;
using DialogueSystem.MiniMaps;
using UnityEngine.UIElements;
using DialogueSystem.Groups;
using DialogueSystem.Nodes;
using DialogueSystem.Ports;
using DialogueSystem.Edges;
using DialogueSystem.Text;
using DialogueSystem.Save;
using System.Linq;
using UnityEngine;
using UnityEditor;
using System.IO;
using System;

namespace DialogueSystem.Window
{
    public class DSGraphView : GraphView
    {
        public event Action<bool> OnCanSaveGraphEvent;
        public event Action<float> OnSaveEvent;
        public DSGraphModel Model { get; protected set; }
        internal DSMiniMap MiniMap;

        private const string GRAPH_STYLE_LINK = "Assets/Plugins/DialogueSystem/Resources/Front/DialogueSystemStyles.uss";
        private const string NODE_STYLE_LINK = "Assets/Plugins/DialogueSystem/Resources/Front/DialogueSystemNodeStyles.uss";

        private Generator generator;
        private DSSearchWindow searchWindow;
        private DSEditorWindow editorWindow;

        private SerializableDictionary<string, DSNodeErrorData> ungroupedNodes;
        private SerializableDictionary<string, DSGroupErrorData> groups;
        private SerializableDictionary<BaseGroup, SerializableDictionary<string, DSNodeErrorData>> groupedNodes;

        internal List<BaseNode> i_Nodes { get; set; } = new List<BaseNode>();
        internal List<BaseGroup> i_Groups { get; set; } = new List<BaseGroup>();

        private int repeatedNameAmount;
        private int RepeatedNameAmount
        {
            get => repeatedNameAmount;
            set
            {
                repeatedNameAmount = value;
                if (repeatedNameAmount > 0) OnCanSaveGraphEvent?.Invoke(false);
                else OnCanSaveGraphEvent?.Invoke(true);
            }
        }
        internal bool IsCanSave { get => repeatedNameAmount == 0; }

        internal DSGraphView(DSEditorWindow editorWindow)
        {
            Model = new DSGraphModel()
            {
                FileName = "DialogueFileName"
            };

            this.editorWindow = editorWindow;
            ungroupedNodes = new();
            groups = new();
            groupedNodes = new();
            generator = new(this);

            AddManipulators();
            AddSearchWindow();
            AddMinimap();
            AddGridBackground();

            OnElementDeleted();
            OnGroupElementAdded();
            OnGroupElementRemoved();
            OnGroupRenamed();
            OnGraphViewChanged();

            AddStyles();
        }

        #region Overrides
        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
        {
            List<Port> compatiblePorts = new List<Port>();

            ports.ForEach(port =>
            {
                if (startPort == port) return;
                if (startPort.node == port.node) return;
                if (startPort.direction == port.direction) return;

                compatiblePorts.Add(port);
            });

            return compatiblePorts;
        }
        #endregion
        #region Manipulators
        private void AddManipulators()
        {
            SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);

            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());

            //this.AddManipulator(new StartDragManipulator());

            //var listNodeTypes = DialogueSystemUtilities.GetListExtendedClasses(typeof(BaseNode));
            //foreach (var item in listNodeTypes) this.AddManipulator(CreateNodeContextMenu($"Add {item.Name}", item));
            this.AddManipulator(CreateGroupContextualMenu());
        }
        #endregion
        #region CreatingElements
        internal T CreateNode<T>(Vector2 position, List<object> portsContext) where T : BaseNode
        {
            var type = typeof(T);
            return (T)CreateNode(type, position, portsContext);
        }
        internal BaseNode CreateNode(Type type, Vector2 position, List<object> portsContext)
        {
            var node = DSUtilities.CreateNode(this, type, position, portsContext);
            i_Nodes.Add(node);
            return node;
        }
        internal BaseGroup CreateGroup(Type type, Vector2 mousePosition, string title = "DialogueGroup", string tooltip = null)
        {
            var group = DSUtilities.CreateGroup(this, type, mousePosition, title, tooltip);

            AddElement(group);

            List<BaseNode> innerNode = new List<BaseNode>();
            foreach (GraphElement graphElement in selection)
            {
                if (graphElement is BaseNode node)
                {
                    innerNode.Add(node);
                    group.AddElement(node);
                }
                else continue;
            }

            group.OnCreate(innerNode);
            i_Groups.Add(group);
            return group;
        }

        private IManipulator CreateGroupContextualMenu()
        {
            ContextualMenuManipulator contextualMenuManipulator = new(e =>
            {
                e.menu.AppendAction("Add Group", a =>
                    CreateGroup(typeof(BaseGroup), GetLocalMousePosition(a.eventInfo.mousePosition, false)));

            });

            return contextualMenuManipulator;
        }


        private IManipulator CreateNodeContextMenu(string actionTitle, Type type)
        {
            ContextualMenuManipulator contextualMenuManipulator = new(e =>
            {
                e.menu.AppendAction(actionTitle, a =>
                    AddElement(CreateNode(type, GetLocalMousePosition(a.eventInfo.mousePosition, false), null)));

            });

            return contextualMenuManipulator;
        }

        private void AddSearchWindow()
        {
            if (!searchWindow)
            {
                searchWindow = ScriptableObject.CreateInstance<DSSearchWindow>();
                searchWindow.Initialize(this);
            }
            nodeCreationRequest = e => SearchWindow.Open(new SearchWindowContext(e.screenMousePosition), searchWindow);
        }
        #endregion
        #region Callbacks
        private void OnElementDeleted()
        {
            deleteSelection += (operationName, askUser) =>
            {
                List<BaseNode> nodesToDelete = new();
                List<BaseGroup> groupToDelete = new();
                List<Edge> edgesToDelete = new();

                foreach (GraphElement element in selection)
                {
                    if (element is BaseNode node)
                    {
                        nodesToDelete.Add(node);
                        continue;
                    }
                    if (element is BaseGroup group)
                    {
                        groupToDelete.Add(group);
                        continue;
                    }
                    if (element is Edge edge)
                    {
                        edgesToDelete.Add(edge);
                        continue;
                    }
                }

                DeleteElements(edgesToDelete);

                nodesToDelete.ForEach(node =>
                {
                    node.OnDestroy();
                    if (node.Group is not null)
                        node.Group.RemoveElement(node);

                    RemoveUngroupedNode(node);
                    node.DisconnectAllPorts();
                    i_Nodes.Remove(node);
                    RemoveElement(node);
                });

                groupToDelete.ForEach(group =>
                {
                    List<BaseNode> nodes = new();
                    foreach (GraphElement elem in group.containedElements)
                    {
                        if (elem is BaseNode node)
                            nodes.Add(node);
                    }

                    group.OnDestroy();
                    group.RemoveElements(nodes);
                    RemoveGroup(group);
                    RemoveElement(group);
                });
            };
        }
        private void OnGroupElementAdded()
        {
            elementsAddedToGroup += (group, elements) =>
            {
                foreach (GraphElement element in elements)
                {
                    if (element is BaseNode node)
                    {
                        BaseGroup nodeGroup = (BaseGroup)group;
                        RemoveUngroupedNode(node);
                        node.OnGroupUp(nodeGroup);
                        AddGroupNode(nodeGroup, node);
                    }
                    else continue;
                }
            };
        }
        private void OnGroupElementRemoved()
        {
            elementsRemovedFromGroup += (group, elements) =>
            {
                foreach (GraphElement element in elements)
                {
                    if (element is BaseNode node)
                    {
                        BaseGroup nodeGroup = (BaseGroup)group;
                        RemoveGroupedNode(nodeGroup, node);
                        node.OnUnGroup(nodeGroup);
                        AddUngroupedNode(node);
                    }
                    else continue;
                }
            };
        }
        private void OnGroupRenamed()
        {
            groupTitleChanged += (group, newTitle) =>
            {
                BaseGroup baseGroup = (BaseGroup)group;
                group.title = newTitle.RemoveWhitespaces().RemoveSpecialCharacters();
                RemoveGroup(baseGroup);

                baseGroup.OnTitleChanged(group.title);
                AddGroup(baseGroup);
            };
        }
        private void OnGraphViewChanged()
        {
            graphViewChanged += (changes) =>
            {
                if (changes.edgesToCreate != null)
                {
                    foreach (Edge edge in changes.edgesToCreate)
                    {
                        BaseNode nextNode = edge.input.node as BaseNode;
                        BaseNode outputNode = edge.output.node as BaseNode;

                        outputNode.OnConnectOutputPort(edge.output as BasePort, edge);
                        nextNode.OnConnectInputPort(edge.input as BasePort, edge);
                    }
                }
                if (changes.movedElements != null)
                {
                    foreach (var elem in changes.movedElements)
                    {
                        if (elem is BaseNode node)
                        {
                            node.OnChangePosition(elem.GetPosition().position, changes.moveDelta);
                        }
                        if (elem is BaseGroup group)
                        {
                            group.OnChangePosition(elem.GetPosition().position, changes.moveDelta);
                        }
                    }
                }
                if (changes.elementsToRemove != null)
                {
                    foreach (var elem in changes.elementsToRemove)
                    {
                        if (elem is Edge edge)
                        {
                            BaseNode nextNode = edge.input.node as BaseNode;
                            BaseNode prevNode = edge.output.node as BaseNode;

                            prevNode?.OnDestroyConnectionOutput(edge.output as BasePort, edge);
                            nextNode?.OnDestroyConnectionInput(edge.input as BasePort, edge);
                        }
                    }
                }
                return changes;
            };
        }
        #endregion
        #region Entities Manipulations

        private void AddMinimap()
        {
            MiniMap = new DSMiniMap()
            {
                anchored = true,
            };
            MiniMap.SetPosition(new Rect(10f, 40f, 200, 100));
            Add(MiniMap);
        }
        public void AddUngroupedNode(BaseNode node)
        {
            string nodeName = node.Model.NodeName.ToLower();

            if (!ungroupedNodes.ContainsKey(nodeName))
            {
                DSNodeErrorData nodeErrorData = new();
                nodeErrorData.Nodes.Add(node);
                ungroupedNodes.Add(nodeName, nodeErrorData);
                return;
            }
            List<BaseNode> ungroupedNodeList = ungroupedNodes[nodeName].Nodes;

            ungroupedNodeList.Add(node);
            Color errorColor = ungroupedNodes[nodeName].ErrorData.Color;
            node.SetErrorStyle(errorColor);

            if (ungroupedNodeList.Count == 2)
            {
                ++RepeatedNameAmount;
                ungroupedNodeList[0].SetErrorStyle(errorColor);
            }
        }
        public void RemoveUngroupedNode(BaseNode node)
        {
            var nodeName = node.Model.NodeName.ToLower();
            List<BaseNode> ungroupedNodeList = ungroupedNodes[nodeName].Nodes;
            ungroupedNodeList.Remove(node);
            node.ResetStyle();

            if (ungroupedNodeList.Count == 1)
            {
                --RepeatedNameAmount;
                ungroupedNodeList[0].ResetStyle();
                return;
            }

            if (ungroupedNodeList.Count == 0)
            {
                ungroupedNodes.Remove(nodeName);
                return;
            }
        }
        public void AddGroupNode(BaseGroup group, BaseNode node)
        {
            string nodeName = node.Model.NodeName.ToLower();

            if (!groupedNodes.ContainsKey(group))
            {
                groupedNodes.Add(group, new SerializableDictionary<string, DSNodeErrorData>());
            }

            if (!groupedNodes[group].ContainsKey(nodeName))
            {
                DSNodeErrorData errorData = new();
                errorData.Nodes.Add(node);
                groupedNodes[group].Add(nodeName, errorData);
                return;
            }
            List<BaseNode> groupedNodesList = groupedNodes[group][nodeName].Nodes;
            groupedNodesList.Add(node);
            Color errorColor = groupedNodes[group][nodeName].ErrorData.Color;
            node.SetErrorStyle(errorColor);

            if (groupedNodesList.Count == 2)
            {
                ++RepeatedNameAmount;
                groupedNodesList[0].SetErrorStyle(errorColor);
            }
        }
        public void RemoveGroupedNode(BaseGroup group, BaseNode node)
        {
            string nodeName = node.Model.NodeName.ToLower();

            List<BaseNode> groupedNodeList = groupedNodes[group][nodeName].Nodes;
            groupedNodeList.Remove(node);

            node.ResetStyle();
            if (groupedNodeList.Count == 1)
            {
                --RepeatedNameAmount;
                groupedNodeList[0].ResetStyle();
                return;
            }

            if (groupedNodeList.Count == 0)
            {
                groupedNodes[group].Remove(nodeName);
                if (groupedNodes[group].Count == 0)
                {
                    groupedNodes.Remove(group);
                }
            }
        }
        public void AddGroup(BaseGroup group)
        {
            string groupName = group.title.ToLower();
            if (!groups.ContainsKey(groupName))
            {
                DSGroupErrorData error = new();
                error.Groups.Add(group);
                groups.Add(groupName, error);
                return;
            }

            List<BaseGroup> groupsList = groups[groupName].Groups;
            groupsList.Add(group);
            Color errorColor = groups[groupName].ErrorData.Color;
            group.SetErrorStyle(errorColor);

            if (groupsList.Count == 2)
            {
                ++RepeatedNameAmount;
                groupsList[0].SetErrorStyle(errorColor);
            }
        }
        private void RemoveGroup(BaseGroup group)
        {
            string oldGroupName = group.Model.GroupName.ToLower();
            List<BaseGroup> groupList = groups[oldGroupName].Groups;
            groupList.Remove(group);
            i_Groups.Remove(group);
            group.ResetStyle();

            if (groupList.Count == 1)
            {
                --RepeatedNameAmount;
                groupList[0].ResetStyle();
                return;
            }

            if (groupList.Count == 0)
            {
                groups.Remove(oldGroupName);
            }
        }

        #endregion
        #region Styles
        private void AddStyles()
        {
            this.LoadAndAddStyleSheets(GRAPH_STYLE_LINK, NODE_STYLE_LINK);
        }
        private void AddGridBackground()
        {
            GridBackground gridBackground = new GridBackground();
            gridBackground.StretchToParentSize();
            Insert(0, gridBackground);
        }
        #endregion
        #region Utilities
        public Vector2 GetLocalMousePosition(Vector2 mousePosition, bool isSearchWindow = false)
        {
            Vector2 worldMP = mousePosition;

            if (isSearchWindow)
                worldMP -= editorWindow.position.position;

            var local = contentViewContainer.WorldToLocal(worldMP);
            return local;
        }
        internal List<T> GetListNodesOfType<T>() =>
            i_Nodes.OfType<T>().ToList();

        internal T[] GetArrayNodesOfType<T>() =>
            i_Nodes.OfType<T>().ToArray();

        internal void Save(string fileName)
        {
            string path = $"Assets/{fileName}.asset";

            if (File.Exists(path)) File.Delete(path);

            GraphSO newGraphSO = ScriptableObject.CreateInstance<GraphSO>();
            AssetDatabase.CreateAsset(newGraphSO, path);

            List<DSNodeModel> nodes = new List<DSNodeModel>();
            List<DSGroupModel> groups = new List<DSGroupModel>();

            foreach (var node in i_Nodes) nodes.Add(node.Model);
            foreach (var group in i_Groups) groups.Add(group.Model);
            newGraphSO.Init(fileName, nodes, groups,
                callback: (cur, from) =>
                {
                    OnSaveEvent?.Invoke((float)cur / (float)from);
                });
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        internal void CleanGraph()
        {
            List<GraphElement> graphElements = new List<GraphElement>();
            foreach (var item in i_Nodes) graphElements.Add(item);
            foreach (var item in i_Groups) graphElements.Add(item);
            foreach (var item in graphElements) AddToSelection(item);
            deleteSelection?.Invoke("delete", AskUser.DontAskUser);
            DeleteSelection();
        }

        internal string Load(string filePath)
        {
            if (string.IsNullOrEmpty(filePath) || string.IsNullOrWhiteSpace(filePath)) return Model.FileName;
            if (!File.Exists(filePath)) throw new FileNotFoundException();
            filePath = filePath.Substring(filePath.IndexOf("Assets"));
            GraphSO graphSO = AssetDatabase.LoadAssetAtPath<GraphSO>(filePath);

            if (graphSO == null) return graphSO.FileName;

            CleanGraph();
            Model.FileName = graphSO.FileName;
            foreach (var groupModel in graphSO.GroupModels)
            {
                var group = CreateGroup(Type.GetType(groupModel.Type), groupModel.Position, groupModel.GroupName);
            }

            foreach (var nodeModel in graphSO.NodeModels)
            {
                var node = CreateNode(Type.GetType(nodeModel.DialogueType), nodeModel.Position, new List<object>
                {
                    nodeModel
                });
                AddElement(node);
            }

            foreach (var node in i_Nodes)
            {
                if (node.Model.Outputs == null || node.Model.Outputs.Count == 0) continue;

                foreach (var output in node.Model.Outputs)
                {
                    ToMakeConnections(output, node.GetOutputPorts());
                }
            }

            return Model.FileName;
        }

        private void ToMakeConnections(DSPortModel portModel, List<BasePort> ports)
        {
            var port = ports.Where(el => el.ID == portModel.PortID).FirstOrDefault();
            if (port != null)
            {
                if (portModel.NodeIDs == null)
                {

                }
                else
                {
                    foreach (var portIdModel in portModel.NodeIDs)
                    {
                        var inputNode = i_Nodes.Where(e => e.Model.ID == portIdModel.NodeID).FirstOrDefault();
                        if (inputNode != null)
                        {
                            foreach (var portId in portIdModel.PortIDs)
                            {
                                var inp = inputNode.GetInputPorts();
                                var neededInputPort = inp.Where(e => e.ID == portId).FirstOrDefault();
                                if (neededInputPort != null)
                                {
                                    DSEdge edge = new DSEdge
                                    {
                                        output = port,
                                        input = neededInputPort
                                    };

                                    edge.input.Connect(edge);
                                    edge.output.Connect(edge);
                                    AddElement(edge);
                                }
                            }
                        }
                    }
                }
            }
        }
        internal void GenerateAsset(string filename) => generator.Generate(filename);
        #endregion
    }
}
