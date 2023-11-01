using UnityEditor.Experimental.GraphView;
using DialogueSystem.Database.Save;
using System.Collections.Generic;
using DialogueSystem.Utilities;
using UnityEngine.UIElements;
using DialogueSystem.Groups;
using DialogueSystem.Window;
using DialogueSystem.Ports;
using DialogueSystem.Text;
using UnityEngine;
using System.Linq;
using System;
using Random = UnityEngine.Random;

namespace DialogueSystem.Nodes
{
    public class BaseNode : Node
    {
        public DSNodeModel Model { get; private set; }
        public BaseGroup Group { get; private set; }
        protected TextField titleTF { get; set; }

        protected DSGraphView graphView { get; set; }
        private Color defaultbackgroundColor;

        internal virtual void Initialize(DSGraphView graphView, Vector2 position, List<object> context)
        {
            if (context == null)
            {
                Model = new()
                {
                    ID = Guid.NewGuid().ToString(),
                    Minimal = 1,
                    NodeName = this.GetType().Name + " " + Random.Range(0, 100).ToString(),
                    DialogueType = this.GetType().ToString(),
                    position = position,
                };
            }
            else
            {
                Model = context[0] as DSNodeModel;
            }

            defaultbackgroundColor = new Color(29f / 255f, 29f / 255f, 30f / 255f);
            this.graphView = graphView;
            this.SetPosition(new Rect(position, Vector2.zero));
            AddStyles();
        }

        #region Draw
        protected virtual void DrawTitleContainer(VisualElement container)
        {
            titleTF = DSUtilities.CreateTextField(
                Model.NodeName,
                null,
                callback =>
                {
                    TextField target = callback.target as TextField;
                    target.value = callback.newValue.RemoveWhitespaces().RemoveSpecialCharacters();

                    if (Group is null)
                    {
                        graphView.RemoveUngroupedNode(this);
                        Model.NodeName = target.value;
                        graphView.AddUngroupedNode(this);
                    }
                    else
                    {
                        BaseGroup currentGroup = Group;
                        graphView.RemoveGroupedNode(Group, this);
                        Model.NodeName = target.value;
                        graphView.AddGroupNode(currentGroup, this);
                    }
                },
                styles: new string[]
                    {
                        "ds-node__textfield",
                        "ds-node__filename-textfield",
                        "ds-node__textfield__hidden"
                    }
                );

            container.Insert(0, titleTF);
        }
        protected virtual void DrawInputContainer(VisualElement container)
        {
            foreach (var input in Model.Inputs)
                AddPortByType(input);
        }
        protected virtual void DrawOutputContainer(VisualElement container)
        {
            foreach (DSPortModel output in Model.Outputs)
                AddPortByType(output);
        }
        protected virtual void DrawMainContainer(VisualElement container)
        {

        }
        protected virtual void DrawExtensionContainer(VisualElement container) { }
        protected virtual void Draw()
        {
            DrawTitleContainer(titleContainer);
            DrawMainContainer(mainContainer);
            DrawInputContainer(inputContainer);
            DrawOutputContainer(outputContainer);
            DrawExtensionContainer(extensionContainer);

            RefreshExpandedState();
        }
        #endregion

        #region Overrided
        public override Port InstantiatePort(Orientation orientation, Direction direction, Port.Capacity capacity, Type type) =>
            BasePort.CreateBasePort<Edge>(orientation, direction, capacity, type);
        public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
        {
            evt.menu.AppendAction("Disconnect Input Ports", e => DisconnectInputPorts());
            evt.menu.AppendAction("Disconnect Output Ports", e => DisconnectOutputPorts());
            base.BuildContextualMenu(evt);
        }
        #endregion

        #region Style
        private void AddStyles()
        {
            mainContainer.AddToClassList("ds-node__main-container");
            extensionContainer.AddToClassList("ds-node__extension-container");
        }
        internal virtual void SetErrorStyle(Color color) => mainContainer.style.backgroundColor = color;
        internal virtual void ResetStyle() => mainContainer.style.backgroundColor = defaultbackgroundColor;
        #endregion

        #region Utilits
        internal void DisconnectAllPorts()
        {
            DisconnectInputPorts();
            DisconnectOutputPorts();
        }
        private void DisconnectInputPorts() => DisconnectPort(inputContainer);
        private void DisconnectOutputPorts() => DisconnectPort(outputContainer);
        private void DisconnectPort(VisualElement container)
        {
            foreach (Port port in container.Children())
            {
                if (port.connected)
                {
                    graphView.DeleteElements(port.connections);
                }
            }
        }
        internal protected List<BasePort> GetInputPorts() => inputContainer.Children().Cast<BasePort>().ToList();
        internal protected List<BasePort> GetOutputPorts() => outputContainer.Children().Cast<BasePort>().ToList();
        internal virtual string GetLetterFromNumber(int number)
        {
            number = Math.Abs(number);

            string result = "";
            do
            {
                result = (char)('A' + (number % 26)) + result;
                number /= 26;
            } while (number-- > 0);

            return result;
        }

        #endregion

        #region MonoEvents
        public virtual void OnConnectOutputPort(BasePort port, Edge edge)
        {
            var tt = Model.Outputs.Where(el => el.PortID == port.ID).FirstOrDefault();
            if (tt != null) tt.AddPort(edge.input.node as BaseNode, edge.input as BasePort);
        }
        public virtual void OnConnectInputPort(BasePort port, Edge edge)
        {
            var tt = Model.Inputs.Where(el => el.PortID == port.ID).FirstOrDefault();
            if (tt != null) tt.AddPort(edge.output.node as BaseNode, edge.output as BasePort);
        }
        public virtual void OnDestroyConnectionOutput(BasePort port, Edge edge)
        {
            var tt = Model.Outputs.Where(el => el.PortID == port.ID).FirstOrDefault();
            if (tt != null) tt.RemovePort(edge.input.node as BaseNode, edge.input as BasePort);
        }
        public virtual void OnDestroyConnectionInput(BasePort port, Edge edge)
        {
            var tt = Model.Inputs.Where(el => el.PortID == port.ID).FirstOrDefault();
            if (tt != null) tt.RemovePort(edge.output.node as BaseNode, edge.output as BasePort);
        }

        public virtual void OnChangePosition(Vector2 position, Vector2 delta)
        {
            Model.position = position;
        }
        public virtual void OnCreate() => Draw();
        public virtual void OnDestroy()
        {

        }
        public virtual void OnGroupUp(BaseGroup group)
        {
            Model.GroupID = group.Model.ID;
            Group = group;
        }
        public virtual void OnUnGroup(BaseGroup group)
        {
            Model.GroupID = "";
            Group = null;
        }

        public virtual void Do(List<object> values) { }
        #endregion

        #region Ports
        protected virtual (BasePort port, DSPortModel data) AddPortByType(string portText, string ID, Type type, object value, bool isInput, bool isSingle, Type[] availableTypes, bool isField = false, bool cross = false, int minimal = 1)
        {
            var data = new DSPortModel(availableTypes)
            {
                PortID = ID,
                PortText = portText,
                Type = type,
                Value = value,
                IsInput = isInput,
                IsField = isField,
                IsSingle = isSingle,
                Cross = cross,
                AvailableTypes = availableTypes == null ? new string[] { type.ToString() } : availableTypes.Select(el => el.ToString()).ToArray()
            };

            if (!DSUtilities.IsAvalilableType(type))
                return (null, data);

            if (data.IsInput) Model.Inputs.Add(data);
            else Model.Outputs.Add(data);
            
            return AddPortByType(data);
        }
        protected virtual (BasePort port, DSPortModel data) AddPortByType(DSPortModel data)
        {
            BasePort port = null;
            Port.Capacity capacity = data.IsSingle == true ? Port.Capacity.Single : Port.Capacity.Multi;
            Direction direction = data.IsInput == true ? Direction.Input : Direction.Output;

            port = this.CreatePort(
                ID: data.PortID,
                portname: data.PortText,
                orientation: Orientation.Horizontal,
                direction: direction,
                capacity: capacity,
                type: data.Type);
            port.AvailableTypes = data.AvailableTypes.Select(el => Type.GetType(el)).ToArray();
            port.Value = data.Value;

            if (data.IsField && data.Type != null)
            {
                switch (Type.GetTypeCode(data.Type))
                {
                    case TypeCode.Single:
                        FloatField floatField = DSUtilities.CreateFloatField(
                        0,
                        onChange: callback =>
                        {
                            FloatField target = callback.target as FloatField;
                            target.value = callback.newValue;
                            data.Value = callback.newValue;
                            port.Value = callback.newValue;
                        },
                        styles: new string[]
                            {
                                "ds-node__floatfield",
                                "ds-node__choice-textfield",
                                "ds-node__textfield__hidden"
                            }
                        );
                        port.Add(floatField);
                        break;

                    case TypeCode.Boolean:
                        Toggle toggle = DSUtilities.CreateToggle(
                            "",
                            "",
                            onChange: callBack =>
                            {
                                Toggle target = callBack.target as Toggle;
                                target.value = callBack.newValue;
                                data.Value = callBack.newValue;
                                port.Value = callBack.newValue;
                            },
                            styles: new string[]
                            {
                                "ds-node__toglefield",
                                "ds-node__choice-textfield",
                                "ds-node__textfield__hidden"
                            });
                        port.Add(toggle);
                        break;

                    case TypeCode.Int32:
                        IntegerField integetField = DSUtilities.CreateIntegerField(
                        0,
                        onChange: callback =>
                        {
                            IntegerField target = callback.target as IntegerField;
                            target.value = callback.newValue;
                            data.Value = callback.newValue;
                            port.Value = callback.newValue;
                        },
                        styles: new string[]
                            {
                                "ds-node__integerfield",
                                "ds-node__choice-textfield",
                                "ds-node__textfield__hidden"
                            }
                        );
                        port.Add(integetField);
                        break;

                    case TypeCode.String:
                        TextField Text = DSUtilities.CreateTextField(
                        (string)data.Value,
                        onChange: callback =>
                        {
                            TextField target = callback.target as TextField;
                            target.value = callback.newValue;
                            data.Value = callback.newValue;
                            port.Value = callback.newValue;
                        },
                        styles: new string[]
                            {
                                "ds-node__textfield",
                                "ds-node__choice-textfield",
                                "ds-node__textfield__hidden"
                            }
                        );
                        port.Add(Text);
                        break;

                    case TypeCode.Double:
                        FloatField floatField2 = DSUtilities.CreateFloatField(
                        0,
                        onChange: callback =>
                        {
                            FloatField target = callback.target as FloatField;
                            target.value = callback.newValue;
                            data.Value = callback.newValue;
                            port.Value = callback.newValue;
                        },
                        styles: new string[]
                            {
                                "ds-node__floatfield",
                                "ds-node__choice-textfield",
                                "ds-node__textfield__hidden"
                            }
                        );
                        port.Add(floatField2);
                        break;

                    default:
                        Console.WriteLine("Неизвестный тип");
                        break;
                }
            }
            if (data.Cross)
            {
                Button crossBtn = DSUtilities.CreateButton(
                "X",
                () =>
                {
                    if (data.IsInput)
                    {
                        if (Model.Inputs.Count == Model.Minimal) return;
                    }
                    else
                    {
                        if (Model.Outputs.Count == Model.Minimal) return;
                    }
                    if (port.connected)
                    {
                        var edges = port.connections;
                        foreach (Edge edge in edges)
                        {
                            //var input = edge.input.node as BaseNode;
                            //var ouptut = edge.output.node as BaseNode;
                            //input?.OnDestroyConnectionInput(edge.input as BasePort, edge);
                            //ouptut?.OnDestroyConnectionOutput(edge.output as BasePort, edge);
                        }
                        graphView.DeleteElements(port.connections);
                    }
                    if (data.IsInput) Model.Inputs.Remove(data);
                    else Model.Outputs.Remove(data);
                    graphView.RemoveElement(port);
                },
                styles: new string[]
                {
                    "ds-node__button"
                });

                port.Add(crossBtn);
            }

            if (data.IsIfPort)
            {
                var outputs = outputContainer.Children().ToList();
                outputs[outputs.Count - 1].Add(port);
            }
            else
            {
                if (data.IsInput) inputContainer.Add(port);
                else outputContainer.Add(port);
            }

            return (port, data);
        }
        #endregion
    }
}
