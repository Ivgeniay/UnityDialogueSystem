using DialogueSystem.Nodes;
using UnityEditor.Experimental.GraphView;
using System.Collections.Generic;
using DialogueSystem.Utilities;
using UnityEngine.UIElements;
using UnityEngine;
using System;
using DialogueSystem.Database.Error;
using DialogueSystem.SDictionary;
using UnityEditor.Graphs;
using DialogueSystem.Groups;
using System.Text.RegularExpressions;

namespace DialogueSystem.Window
{
    public sealed class DialogueSystemGraphView : GraphView
    {
        public event Action<BaseNode> OnNodeDeletedEvent;
        public event Action<BaseGroup> OnGroupDeleteEvent;
        public event Action<BaseGroup, BaseNode> OnGroupElementAddedEvent;
        public event Action<BaseGroup, BaseNode> OnGroupElementRemoveEvent;

        private const string GRAPH_STYLE_LINK = "Assets/Plugins/DialogueSystem/Resources/Front/DialogueSystemStyles.uss";
        private const string NODE_STYLE_LINK = "Assets/Plugins/DialogueSystem/Resources/Front/DialogueSystemNodeStyles.uss";

        private DialogueSearchWindow searchWindow;
        private DialogueSystemEditorWindow editorWindow;

        private SerializableDictionary<string, DialogueSystemNodeErrorData> ungroupedNodes;
        private SerializableDictionary<string, DialogueSystemGroupErrorData> groups;
        private SerializableDictionary<BaseGroup, Dictionary<string, DialogueSystemNodeErrorData>> groupedNodes;

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
        private IManipulator CreateGroupContextualMenu()
        {
            ContextualMenuManipulator contextualMenuManipulator = new(e =>
            {
                e.menu.AppendAction("Add Group", a =>
                    AddElement(CreateGroup(typeof(BaseGroup), GetLocalMousePosition(a.eventInfo.mousePosition, false))));

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
                        RemoveGroup(group);
                        continue;
                    }
                }

                nodesToDelete.ForEach(node =>
                {
                    OnNodeDeletedEvent?.Invoke(node);

                    if (node.Group is not null)
                    {
                        node.Group.RemoveElement(node);
                    }

                    RemoveUngroupedNode(node);
                    RemoveElement(node);
                });
                groupToDelete.ForEach(group =>
                {
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
                        OnGroupElementAddedEvent?.Invoke(nodeGroup, node);
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
                        OnGroupElementRemoveEvent?.Invoke(nodeGroup, node);
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
                RemoveGroup(baseGroup);

                baseGroup.OldTitle = newTitle;
                AddGroup(baseGroup);
            };
        }

        #endregion
        #region Entities Manipulations
        internal BaseNode CreateNode(Type type, Vector2 position) =>
            DialogueSystemUtilities.CreateNode(this, type, position);
        internal BaseGroup CreateGroup(Type type, Vector2 mousePosition, string title = "DialogueGroup", string tooltip = null) =>
            DialogueSystemUtilities.CreateGroup(this, type, mousePosition, title, tooltip);

        public void AddUngroupedNode(BaseNode node)
        {
            string nodeName = node.DialogueName;

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

            if (ungroupedNodeList.Count >= 2)
            {
                ungroupedNodeList[0].SetErrorStyle(errorColor);
            }
        }
        public void RemoveUngroupedNode(BaseNode node)
        {
            var nodeName = node.DialogueName;
            List<BaseNode> ungroupedNodeList = ungroupedNodes[nodeName].Nodes;
            ungroupedNodeList.Remove(node);
            node.ResetStyle();

            if (ungroupedNodeList.Count == 1)
            {
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
            string nodeName = node.DialogueName;

            node.Group = group;

            if (!groupedNodes.ContainsKey(group))
            {
                groupedNodes.Add(group, new Dictionary<string, DialogueSystemNodeErrorData>());
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

            if (groupedNodesList.Count >= 2)
            {
                groupedNodesList[0].SetErrorStyle(errorColor);
            }
        }
        public void RemoveGroupedNode(BaseGroup group, BaseNode node)
        {
            string nodeName = node.DialogueName;

            node.Group = null;

            List<BaseNode> groupedNodeList = groupedNodes[group][nodeName].Nodes;
            groupedNodeList.Remove(node);

            node.ResetStyle();
            if (groupedNodeList.Count == 1)
            {
                groupedNodeList[0].ResetStyle();
                return;
            }

            if (groupedNodeList.Count == 0)
            {
                groupedNodes[group].Remove(nodeName);
                if (groupedNodes[group].Count == 0)
                    groupedNodes.Remove(group);
            }
        }
        public void AddGroup(BaseGroup group)
        {
            string groupName = group.title;
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

            if (groupsList.Count >= 2)
            {
                groupsList[0].SetErrorStyle(errorColor);
            }
        }
        private void RemoveGroup(BaseGroup group)
        {
            string oldGroupName = group.OldTitle;
            var groupList = groups[oldGroupName].Groups;
            groupList.Remove(group);

            group.ResetStyle();

            if (groupList.Count == 1)
            {
                groupList[0].ResetStyle();
            }
            else if (groupList.Count == 0)
            {
                groups.Remove(oldGroupName);
            }

            OnGroupDeleteEvent?.Invoke(group);
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
