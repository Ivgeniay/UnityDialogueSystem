using UnityEditor.Experimental.GraphView;
using DialogueSystem.Abstract;
using UnityEngine.UIElements;
using DialogueSystem.Window;
using UnityEngine;
using System;
using System.Collections.Generic;
using DialogueSystem.Ports;
using System.Linq;
using DialogueSystem.Anchor;
using DialogueSystem.Generators;
using Unity.Android.Gradle;
using DialogueSystem.Nodes;
using System.Text.RegularExpressions;
using UnityEngine.Windows;

namespace DialogueSystem.TextFields
{
    public class DSTextField : TextField, IDataHolder, IAnchorObserver
    {
        public Type Type { get => typeof(string); }
        public object Value { get; set; } = "";
        public bool IsFunctions { get; set; } = false;
        public string Name { get; set; } = "Text";
        public bool IsSerializedInScript { get; set; }

        private DSGraphView graphView;
        private ObservableDictionary<BasePort, string> anchors = new();
        TextElement anchorsTElement;

        public void Initialize(DSGraphView graphView)
        {
            this.graphView = graphView;
            AddManipulators();
            anchorsTElement = new TextElement();
            this.Add(anchorsTElement);
            this.RegisterValueChangedCallback(OnValueChange);
            anchors.OnDictionaryChangedEvent += OnAnchorsDictionaryChanged;
        }


        internal void OnDistroy()
        {
            graphView.AnchorVisitorsUnRegister(this);
        }

        private void AddManipulators()
        {
            this.AddManipulator(CreateContextualMenuAnchors());
        }

        private IManipulator CreateContextualMenuAnchors()
        {
            ContextualMenuManipulator contextualMenuManipulator = new(e =>
            {
                if (graphView.Anchors.Count > 0)
                {
                    e.menu.ClearItems();
                    foreach (var anchor in graphView.Anchors)
                    {
                        if (!anchors.Contains(anchor))
                        {
                            e.menu.AppendAction($"Anchor:{anchor.Value}", a => 
                            {
                                anchors.Add(anchor.Key, anchor.Value);
                                graphView.AnchorVisitorsRegister(this);
                                this.value = this.value.Insert(this.cursorIndex, $"{{{anchor.Value}}}");
                            });
                        }
                    }
                }
            });
            return contextualMenuManipulator;
        }
        private void OnValueChange(ChangeEvent<string> evt)
        {
            DSTextField target = evt.target as DSTextField;
            if (target != null)
            {
                List<BasePort> presentAnchors = anchors
                    .Where(kv => !evt.newValue.Contains(kv.Value))
                    .Select(kv => kv.Key)
                    .ToList();

                if (presentAnchors.Count > 0)
                {
                    foreach (var anchor in presentAnchors)
                        anchors.Remove(anchor);
                }
            }
        }
        private void OnAnchorsDictionaryChanged(object sender, DictionaryChangedEventArgs<BasePort, string> e)
        {
            List<TextElement> childs = anchorsTElement.GetElementsByType<TextElement>(e => e != anchorsTElement);
            foreach (var child in childs) anchorsTElement.Remove(child);
                
            if (anchors.Count > 0)
            {
                foreach (var item in anchors)
                {
                    TextElement newChild = new TextElement();
                    newChild.text = $"<color=#FDD057>{item.Value}</color>";
                    newChild.AddManipulator(new Clickable(() => AnchorClickEvent(newChild), 0, 0));
                    anchorsTElement.Add(newChild);
                }
            }
            else anchorsTElement.text = string.Empty;
        }

        private void AnchorClickEvent(TextElement textElement)
        {
            Regex regex = new Regex(@"<color=[^>]+>([^<]+)</color>");
            Match match = regex.Match(textElement.text);
            string data = "d_a_t_a";
            if (match.Success) data = match.Groups[1].Value;

            BasePort port = anchors.FirstOrDefault(e => e.Value.Contains($"{data}")).Key;
            if (port != null)
            {
                graphView.AddToSelection(port.node);
                graphView.FrameSelection();
            }
        }

        public void OnAnchorUpdate(BasePort port, string newRegex)
        {
            if (anchors.TryGetValue(port, out string regex))
            {
                this.text = this.text.Replace(regex, newRegex);
                anchors[port] = newRegex;
            }
        }

        public void OnAnchorDelete(BasePort port)
        {
            if (anchors.TryGetValue(port, out string regex))
            {
                this.text = this.text.Replace(regex, port.Value != null ? port.Value.ToString() : "");
                anchors.Remove(port);
            }
        }

    }
}
