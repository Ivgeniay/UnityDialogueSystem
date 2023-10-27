using DialogueSystem.Nodes;
using UnityEditor.Experimental.GraphView;
using System.Collections.Generic;
using DialogueSystem.Utilities;
using UnityEngine.UIElements;
using UnityEngine;
using System;
using DialogueSystem.Database.Error;
using DialogueSystem.SDictionary;
using DialogueSystem.Groups;
using DialogueSystem.Text;

namespace DialogueSystem.Window
{
    public sealed class DialogueSystemGraphView : GraphView
    {
        public event Action<bool> OnCanSaveGraphEvent;

        private const string GRAPH_STYLE_LINK = "Assets/Plugins/DialogueSystem/Resources/Front/DialogueSystemStyles.uss";
        private const string NODE_STYLE_LINK = "Assets/Plugins/DialogueSystem/Resources/Front/DialogueSystemNodeStyles.uss";

        private DialogueSearchWindow searchWindow;
        private DialogueSystemEditorWindow editorWindow;

        private SerializableDictionary<string, DialogueSystemNodeErrorData> ungroupedNodes;
        private SerializableDictionary<string, DialogueSystemGroupErrorData> groups;
        private SerializableDictionary<BaseGroup, SerializableDictionary<string, DialogueSystemNodeErrorData>> groupedNodes;
        private List<BaseNode> _nodes = new List<BaseNode>();

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

        internal DialogueSystemGraphView(DialogueSystemEditorWindow editorWindow)
        {
            this.editorWindow = editorWindow;
            ungroupedNodes = new();
            groups = new();
            groupedNodes = new();

            AddManipulators();
            AddSearchWindow();
            AddGridBackground();

            OnElementDeleted();
            OnGroupElementAdded();
            OnGroupElementRemoved();
            OnGroupRenamed();

            AddStyles();
        }


        #region Overrides
        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
        {
            List<Port> compatiblePorts = new List<Port>();

            ports.ForEach(port =>
            {
                if (startPort == port)
                    return;
                if (startPort.node == port.node)
                    return;
                if (startPort.direction == port.direction)
                    return;

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

            var listNodeTypes = DialogueSystemUtilities.GetListExtendedClasses(typeof(BaseNode));
            foreach (var item in listNodeTypes) this.AddManipulator(CreateNodeContextMenu($"Add {item.Name}", item));
            this.AddManipulator(CreateGroupContextualMenu());
        }
        #endregion
        #region CreatingElements
        internal BaseNode CreateNode(Type type, Vector2 position)
        {
            var node = DialogueSystemUtilities.CreateNode(this, type, position);
            _nodes.Add(node);
            node.OnCreate();
            return node;
        }
        internal BaseGroup CreateGroup(Type type, Vector2 mousePosition, string title = "DialogueGroup", string tooltip = null)
        {
            var group = DialogueSystemUtilities.CreateGroup(this, type, mousePosition, title, tooltip);

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
                    AddElement(CreateNode(type, GetLocalMousePosition(a.eventInfo.mousePosition, false))));

            });

            return contextualMenuManipulator;
        }

        private void AddSearchWindow()
        {
            if (!searchWindow)
            {
                searchWindow = ScriptableObject.CreateInstance<DialogueSearchWindow>();
                searchWindow.Initialize(this);
            }
            nodeCreationRequest = e => SearchWindow.Open(new SearchWindowContext(e.screenMousePosition), searchWindow);
        }
        #endregion
        #region Callbacks
        private void OnElementDeleted()
        {
            deleteSelection = (operationName, askUser) =>
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

                //foreach (Edge edge in edgesToDelete)
                //{
                //    _nodes.ForEach(node =>
                //    {
                //        var inConnction = node.IsMyPort(edge.input);
                //        var outConnction = node.IsMyPort(edge.output);
                //        if (inConnction) node.OnDisconectedPort(edge.input);
                //        if (outConnction) node.OnDisconectedPort(edge.output);
                //    });
                //}
                DeleteElements(edgesToDelete);

                nodesToDelete.ForEach(node =>
                {
                    node.OnDestroy();
                    if (node.Group is not null)
                        node.Group.RemoveElement(node);

                    RemoveUngroupedNode(node);
                    node.DisconectAllPorts();
                    _nodes.Remove(node);
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
            elementsAddedToGroup = (group, elements) =>
            {
                foreach (GraphElement element in elements)
                {
                    if (element is BaseNode node)
                    {
                        BaseGroup nodeGroup = (BaseGroup)group;
                        RemoveUngroupedNode(node);
                        AddGroupNode(nodeGroup, node);
                        node.OnGroupUp(nodeGroup);
                    }
                    else continue;
                }
            };
        }
        private void OnGroupElementRemoved()
        {
            elementsRemovedFromGroup = (group, elements) =>
            {
                foreach (GraphElement element in elements)
                {
                    if (element is BaseNode node)
                    {
                        BaseGroup nodeGroup = (BaseGroup)group;
                        RemoveGroupedNode(nodeGroup, node);
                        AddUngroupedNode(node);
                        node.OnUnGroup(nodeGroup);
                    }
                    else continue;
                }
            };
        }
        private void OnGroupRenamed()
        {
            groupTitleChanged = (group, newTitle) =>
            {
                BaseGroup baseGroup = (BaseGroup)group;
                group.title = newTitle.RemoveWhitespaces().RemoveSpecialCharacters();
                RemoveGroup(baseGroup);

                baseGroup.OldTitle = group.title;
                AddGroup(baseGroup);
            };
        }

        #endregion
        #region Entities Manipulations

        public void AddUngroupedNode(BaseNode node)
        {
            string nodeName = node.DialogueName.ToLower();

            if (!ungroupedNodes.ContainsKey(nodeName))
            {
                DialogueSystemNodeErrorData nodeErrorData = new();
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
            var nodeName = node.DialogueName.ToLower();
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
            string nodeName = node.DialogueName.ToLower();

            node.Group = group;

            if (!groupedNodes.ContainsKey(group))
            {
                groupedNodes.Add(group, new SerializableDictionary<string, DialogueSystemNodeErrorData>());
            }

            if (!groupedNodes[group].ContainsKey(nodeName))
            {
                DialogueSystemNodeErrorData errorData = new();
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
            string nodeName = node.DialogueName.ToLower();

            node.Group = null;

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
                DialogueSystemGroupErrorData error = new();
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
            string oldGroupName = group.OldTitle.ToLower();
            List<BaseGroup> groupList = groups[oldGroupName].Groups;
            groupList.Remove(group);

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

        
        #endregion
    }
}
