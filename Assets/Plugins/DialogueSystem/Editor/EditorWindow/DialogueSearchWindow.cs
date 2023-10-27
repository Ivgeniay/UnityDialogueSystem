using DialogueSystem.Groups;
using DialogueSystem.Nodes;
using DialogueSystem.Utilities;
using System;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace DialogueSystem.Window
{
    internal class DialogueSearchWindow : ScriptableObject, ISearchWindowProvider
    {
        private DialogueSystemGraphView graphView;
        private Texture2D indentationIcon;

        public void Initialize(DialogueSystemGraphView graphView)
        {
            this.graphView = graphView;

            indentationIcon = new Texture2D(1, 1);
            indentationIcon.SetPixel(0, 0, Color.clear);
            indentationIcon.Apply();
        }

        public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
        {
            var listNodeTypes = DialogueSystemUtilities.GetListExtendedClasses(typeof(BaseNode));
            List<SearchTreeEntry> searchTreeEntries = new();
            CreateMenuTitle(searchTreeEntries, "Create Element");
            CreateMenuItem(searchTreeEntries, "Dialogue Node", 1);
            listNodeTypes.ForEach(t =>
            {
                CreateMenuChoice(searchTreeEntries, t.Name, 2, t, indentationIcon);
            });
            CreateMenuItem(searchTreeEntries, "Dialogue Group", 1);
            CreateMenuChoice(searchTreeEntries, "Simple Group", 2, typeof(BaseGroup), indentationIcon);

            return searchTreeEntries;
        }

        public bool OnSelectEntry(SearchTreeEntry SearchTreeEntry, SearchWindowContext context)
        {
            var localMousePosition = graphView.GetLocalMousePosition(context.screenMousePosition, true);
            Type type = (Type)SearchTreeEntry.userData;

            if (type == typeof(BaseGroup))
            {
                graphView.CreateGroup(type, localMousePosition);
                return true;
            }
            else if (typeof(BaseNode).IsAssignableFrom(type))
            {
                var node =
                    graphView.CreateNode(type, localMousePosition);
                graphView.AddElement(node);
                return true;
            }

            return false;
        }

        private List<SearchTreeEntry> CreateMenuTitle(List<SearchTreeEntry> entries, string title)
        {
            entries.Add(new SearchTreeGroupEntry(new GUIContent(title)));
            return entries;
        }
        private List<SearchTreeEntry> CreateMenuItem(List<SearchTreeEntry> entries, string itemName, int layer)
        {
            entries.Add(new SearchTreeGroupEntry(new GUIContent(itemName), layer));
            return entries;
        }
        private List<SearchTreeEntry> CreateMenuChoice(List<SearchTreeEntry> entries, string itemName, int layer, object context, Texture2D indentationIcon = null)
        {
            entries.Add(new SearchTreeEntry(new GUIContent(new GUIContent(itemName, indentationIcon)))
            {
                level = layer,
                userData = context
            });
            return entries;
        }

    }
}
