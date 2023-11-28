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
            anchors.OnDictionaryChangedEvent += OnAnchorsDicChanged;
        }

        private void OnAnchorsDicChanged(object sender, DictionaryChangedEventArgs<BasePort, string> e)
        {
            if (anchors.Count > 0)
            {
                anchorsTElement.text = "Anchors contains: \n";
                foreach(var item in anchors)
                    anchorsTElement.text += $"<color=#FDD057>{item.Value}</color>\n";
            }
            else anchorsTElement.text = string.Empty;
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
                        for (int i = 0; i < 100; i++)
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
