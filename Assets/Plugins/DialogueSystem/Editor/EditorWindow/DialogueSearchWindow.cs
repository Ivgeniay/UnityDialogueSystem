using DialogueSystem.Characters;
using DialogueSystem.Groups;
using DialogueSystem.Nodes;
using DialogueSystem.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
            var dtos = GenerateExtendedDOList(typeof(BaseNode), listNodeTypes);

            List<SearchTreeEntry> searchTreeEntries = new();

            CreateMenuTitle(searchTreeEntries, "Create Element");
            CreateMenuItem(searchTreeEntries, "Nodes", 1);

            foreach (ExtendedDO dto in dtos)
            {
                if (dto.Type.Name == "TestNodes")
                {

                }
                if (dto.IsAbstract)
                {
                    CreateMenuItem(searchTreeEntries, DialogueSystemUtilities.GenerateWindowSearchNameFromType(dto.Type), dto.Depth);
                    foreach (ExtendedDO items in dtos)
                    {
                        if (items.FatherType == dto.Type && !items.IsAbstract)
                        {
                            CreateMenuChoice(searchTreeEntries, DialogueSystemUtilities.GenerateWindowSearchNameFromType(items.Type), items.Depth, new Type[] { items.Type }, indentationIcon);
                        }
                    }
                }
                else if (!dto.IsAbstract && dto.Types.Count > 0)
                {
                    CreateMenuItem(searchTreeEntries, DialogueSystemUtilities.GenerateWindowSearchNameFromType(dto.Type) + " Based", dto.Depth);
                    foreach (ExtendedDO items in dtos)
                    {
                        if (items.FatherType == dto.Type && !items.IsAbstract)
                        {
                            CreateMenuChoice(searchTreeEntries, DialogueSystemUtilities.GenerateWindowSearchNameFromType(items.Type), items.Depth, new Type[] { items.Type }, indentationIcon);
                        }
                    }
                }
            }

            CreateMenuItem(searchTreeEntries, "Group", 1);
            CreateMenuChoice(searchTreeEntries, "Simple Group", 2, new Type[] { typeof(BaseGroup) }, indentationIcon);

            var actors = DialogueSystemUtilities.GetListExtendedIntefaces(typeof(IDialogueActor), Assembly.Load("Assembly-CSharp"));
            if (actors != null && actors.Count > 0)
            {
                CreateMenuItem(searchTreeEntries, "Actors", 1);
                actors.ForEach(a =>
                {
                    CreateMenuChoice(searchTreeEntries, DialogueSystemUtilities.GenerateWindowSearchNameFromType(a), 2, new Type[] { typeof(IDialogueActor), a }, indentationIcon);
                });
            }

            return searchTreeEntries;
        }

        public bool OnSelectEntry(SearchTreeEntry SearchTreeEntry, SearchWindowContext context)
        {
            var localMousePosition = graphView.GetLocalMousePosition(context.screenMousePosition, true);
            var data = SearchTreeEntry.userData as Type[];
            Type type = data[0];

            if (type == typeof(BaseGroup))
            {
                graphView.CreateGroup(type, localMousePosition);
                return true;
            }
            else if (type == typeof(IDialogueActor))
            {
                var node = graphView.CreateNode<ActorNode>(localMousePosition);
                node.Generate(data[1]);
                graphView.AddElement(node);
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

        static List<ExtendedDO> GenerateExtendedDOList(Type baseType, List<Type> types)
        {
            List<ExtendedDO> extendedDOList = new List<ExtendedDO>();
            foreach (Type type in types)
            {
                int depth = 2;
                Type currentType = type.BaseType;

                while (currentType != baseType)
                {
                    depth++;
                    currentType = currentType.BaseType;
                }

                List<Type> derivedTypes = types
                    .Where(t => t.BaseType == type)
                    .ToList();

                ExtendedDO extendedDO = new ExtendedDO
                {
                    IsAbstract = type.IsAbstract,
                    Depth = depth,
                    Type = type,
                    FatherType = type.BaseType,
                    Types = derivedTypes
                };

                extendedDOList.Add(extendedDO);
            }

            return extendedDOList;
        }
    }

    public class ExtendedDO
    {
        public int Depth { get; set; }
        public bool IsAbstract { get; set; } = false;
        public Type Type { get; set; }
        public Type FatherType { get; set; }
        public List<Type> Types { get; set; } = new List<Type>();
    }
}
