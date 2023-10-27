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

namespace DialogueSystem.Window
{
    public sealed class DialogueSystemGraphView : GraphView
    {
        public event Action<BaseNode> OnNodeDeletedEvent;
        public event Action<Group, BaseNode> OnGroupAddedEvent;
        public event Action<Group, BaseNode> OnGroupRemoveEvent;

        private string graphStylesLink = "Assets/Plugins/DialogueSystem/Resources/Front/DialogueSystemStyles.uss";
        private string nodeStylesLink = "Assets/Plugins/DialogueSystem/Resources/Front/DialogueSystemNodeStyles.uss";
        private DialogueSearchWindow searchWindow;
        private DialogueSystemEditorWindow editorWindow;

        //private SerializableDictionary<string, DialogueSystemNodeErrorData> ungroupedNodes;
        private Dictionary<string, DialogueSystemNodeErrorData> ungroupedNodes;
        private Dictionary<Group, Dictionary<string, DialogueSystemNodeErrorData>> groupedNodes;

        internal DialogueSystemGraphView(DialogueSystemEditorWindow editorWindow)
        {
            this.editorWindow = editorWindow;
            ungroupedNodes = new();
            groupedNodes = new();

            AddManipulators();
            AddSearchWindow();
            AddGridBackground();

            OnElementDeleted();
            OnGroupElementAdded();
            OnGroupElementRemoved();

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
                    AddElement(CreateGroup(GetLocalMousePosition(a.eventInfo.mousePosition, false))));

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
                foreach (GraphElement element in selection)
                {
                    if (element is BaseNode node)
                        nodesToDelete.Add(node);
                    
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
                        RemoveUngroupedNode(node);
                        AddGroupNode(group, node);
                        OnGroupAddedEvent?.Invoke(group, node);
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
                        RemoveGroupedNode(group, node);
                        AddUngroupedNode(node);
                        OnGroupRemoveEvent?.Invoke(group, node);
                    }
                    else continue;
                }
            };
        }

        #endregion
        #region Entities
        internal BaseNode CreateNode(Type type, Vector2 position)
        {
            if (typeof(BaseNode).IsAssignableFrom(type))
            {
                BaseNode node = (BaseNode)Activator.CreateInstance(type);
                node.Initialize(this, position);
                node.Draw();

                AddUngroupedNode(node);

                return node;
            }
            else
                throw new ArgumentException("Type must be derived from BaseNode", nameof(type));
        }

        internal Group CreateGroup(Vector2 mousePosition, string title = "DialogueGroup", string tooltip = null)
        {
            Group group = new Group()
            {
                title = title,
                tooltip = tooltip == null ? title : tooltip,
            };
            group.SetPosition(new Rect(mousePosition, Vector2.zero));
            
            return group;
        }

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
            Color errorColor = ungroupedNodes[nodeName].errorData.Color;
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
        public void AddGroupNode(Group group, BaseNode node)
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
            Color errorColor = groupedNodes[group][nodeName].errorData.Color;
            node.SetErrorStyle(errorColor);

            if (groupedNodesList.Count >= 2)
            {
                groupedNodesList[0].SetErrorStyle(errorColor);
            }
        }

        public void RemoveGroupedNode(Group group, BaseNode node)
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

        #endregion
        #region Styles
        private void AddStyles()
        {
            this.LoadAndAddStyleSheets(graphStylesLink, nodeStylesLink);
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
