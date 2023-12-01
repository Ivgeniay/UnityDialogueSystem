﻿using UnityEditor.Experimental.GraphView;
using DialogueSystem.Database.Save;
using DialogueSystem.DialogueType;
using Random = UnityEngine.Random;
using System.Collections.Generic;
using DialogueSystem.Generators;
using DialogueSystem.UIElement;
using DialogueSystem.Utilities;
using DialogueSystem.Abstract;
using UnityEngine.UIElements;
using DialogueSystem.Groups;
using DialogueSystem.Window;
using DialogueSystem.Ports;
using DialogueSystem.Edges;
using DialogueSystem.Text;
using UnityEngine;
using System.Linq;
using System; 

namespace DialogueSystem.Nodes
{
    internal abstract class BaseNode : Node, IDataHolder
    {
        internal DSNodeModel Model { get; private set; }
        internal BaseGroup Group { get; private set; }

        public virtual string Name => Model.NodeName;
        public virtual Type Type => Type.GetType(Model.DialogueType);
        public virtual object Value => Model.Text;
        public virtual bool IsFunctions => false;
        public bool IsSerializedInScript => true;
        public Generators.Visibility Visibility { get => Model.Visibility; set => Model.Visibility = value; }
        public Generators.Attribute Attribute { get => Model.Attribute; set => Model.Attribute = value; }

        protected TextField titleTF { get; set; }
        protected DSGraphView graphView { get; set; }
        protected DSDropdownField dropdownAttributes;
        protected DSDropdownField dropdownVisible;
        protected Foldout settings;

        private Color defaultbackgroundColor;

        internal virtual void Initialize(DSGraphView graphView, Vector2 position, List<object> context)
        {
            if (context == null)
            {
                Model = new()
                {
                    ID = Guid.NewGuid().ToString(),
                    Minimal = 1,
                    NodeName = this.GetType().Name + "_" + Random.Range(0, 100).ToString(),
                    DialogueType = this.GetType().ToString(),
                    Position = position,
                };
            }
            else
            {
                var modelSo = context[0] as DSNodeModelSO;
                Model = modelSo.Deconstruct();
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
        protected virtual void DrawMainContainer(VisualElement container) { }
        protected virtual void DrawExtensionContainer(VisualElement container) { }

        protected void InitializeSettingElement(VisualElement container)
        {
            settings = DSUtilities.CreateFoldout("Settings", true);

            dropdownAttributes = new();
            dropdownAttributes.label = "Attriputes";
            dropdownAttributes.choices.Add(Generators.Attribute.None.ToString());
            dropdownAttributes.choices.Add(Generators.Attribute.SerializeField.ToString());
            dropdownAttributes.value = Model.Attribute.ToString();
            dropdownAttributes.RegisterValueChangedCallback<string>((e) =>
            {
                if (Enum.TryParse<Generators.Attribute>(e.newValue, true, out var result)) Attribute = result;
            });

            dropdownVisible = new();
            dropdownVisible.label = "Visibility";
            dropdownVisible.choices.Add(Generators.Visibility.@private.ToString());
            dropdownVisible.choices.Add(Generators.Visibility.@public.ToString());
            dropdownVisible.value = Model.Visibility.ToString();
            dropdownVisible.RegisterValueChangedCallback<string>((e) =>
            {
                if (Enum.TryParse<Generators.Visibility>(e.newValue, true, out var result)) Visibility = result;
            });

            settings.Add(dropdownAttributes);
            settings.Add(dropdownVisible);

            container.Add(settings);
        }
        protected void SetEnableSetting(bool enable)
        {
            settings.SetEnabled(enable);
            if (enable)
            {
                if (Enum.TryParse<Generators.Attribute>(dropdownAttributes.value, true, out var result)) Attribute = result;
                if (Enum.TryParse<Generators.Visibility>(dropdownVisible.value, true, out var res)) Visibility = res;
            }
            else
            {
                Attribute = Generators.Attribute.None;
                Visibility = Generators.Visibility.@private;
            }
        }

        protected virtual void Draw()
        {
            DrawTitleContainer(titleContainer);
            DrawOutputContainer(outputContainer);
            DrawInputContainer(inputContainer);
            DrawMainContainer(mainContainer);
            DrawExtensionContainer(extensionContainer);

            RefreshExpandedState();
        }
        #endregion

        #region Overrided
        public override Port InstantiatePort(Orientation orientation, Direction direction, Port.Capacity capacity, Type type) =>
            BasePort.CreateBasePort<DSEdge>(orientation, direction, capacity, type);
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
        internal protected List<BasePort> GetInputPorts()
        {
            //из-за того что if инпут порт находится в Output контейнере приходится делать такую замудренную шляпу
            var inputs = inputContainer.Children().Cast<BasePort>().ToList();
            var ifPortModel = Model.Inputs.Where(e => e.IsIfPort).ToList();
            if (ifPortModel!= null && ifPortModel.Count > 0)
            {
                var outputs = GetOutputPorts();
                if (outputs != null && outputs.Count > 0)
                {
                    foreach (var output in outputs)
                    {
                        foreach (var ifNode in ifPortModel)
                        {
                            if (output.ID == ifNode.PortID)
                            {
                                inputs.Add(output);
                            }
                        }
                    }
                }
            }
            return inputs;
        }
        internal protected List<BasePort> GetOutputPorts()
        {
            List<BasePort> outputPorts = new List<BasePort>();

            foreach (VisualElement child in outputContainer.Children())
            {
                if (child is BasePort basePort)
                    outputPorts.Add(basePort);
                
                if (child.childCount > 0)
                    outputPorts.AddRange(GetAllOutputPortsRecursive(child));
            }

            return outputPorts;
        }
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
        private List<BasePort> GetAllOutputPortsRecursive(VisualElement element)
        {
            List<BasePort> outputPorts = new List<BasePort>();

            foreach (VisualElement child in element.Children())
            {
                if (child is BasePort basePort)
                {
                    outputPorts.Add(basePort);
                }

                if (child.childCount > 0)
                {
                    outputPorts.AddRange(GetAllOutputPortsRecursive(child));
                }
            }
            return outputPorts;
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
        #endregion

        #region MonoEvents
        public virtual void OnConnectOutputPort(BasePort port, Edge edge)
        {
            BasePort connectedPort = edge.output as BasePort;
            if (!BasePortManager.HaveCommonTypes(connectedPort.AvailableTypes, port.AvailableTypes)) return;

            var tt = Model.Outputs.Where(el => el.PortID == port.ID).FirstOrDefault();
            if (tt != null) tt.AddPort(edge.input.node as BaseNode, edge.input as BasePort);
        }
        public virtual void OnConnectInputPort(BasePort port, Edge edge)
        {
            BasePort connectedPort = edge.output as BasePort;
            bool continues = BasePortManager.HaveCommonTypes(connectedPort.AvailableTypes, port.AvailableTypes);
            if (!continues) return;

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
            Model.Position += delta;
        }
        public virtual void OnCreate() => Draw();
        public virtual void OnDestroy() 
        {
            List<BasePort> ports = new();
            ports.AddRange(GetInputPorts());
            ports.AddRange(GetOutputPorts());
            foreach (var item in ports) item.OnDistroy();

            List<DSTextField> dsText = this.GetElementsByType<DSTextField>();
            foreach (var item in dsText) item.OnDistroy();
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

        public virtual void Do(PortInfo[] portInfos) { }
        #endregion

        #region Ports
        protected virtual (BasePort port, DSPortModel data) AddPortByType(
            string portText, 
            string ID, 
            Type type, 
            object value, 
            bool isInput, 
            bool isSingle, 
            Type[] availableTypes, 
            PortSide portSide, 
            bool isField = false,
            bool cross = false, 
            int minimal = 1, 
            bool isIfPort = false, 
            bool plusIf = false, 
            bool isFunction = false, 
            string ifPortSourceId = null, 
            bool isAnchorable = false, 
            Generators.Visibility visibility = Generators.Visibility.@public, 
            Generators.Attribute attribute = Generators.Attribute.None)
        {
            var data = new DSPortModel(availableTypes, portSide)
            {
                PortID = ID,
                PortText = portText,
                Type = type,
                Value = value == null ? string.Empty : value.ToString(),
                IsIfPort = isIfPort,
                IsInput = isInput,
                IsField = isField,
                IsSingle = isSingle,
                Cross = cross,
                PlusIf = plusIf,
                IsFunction = isFunction,
                IfPortSourceId = ifPortSourceId,
                PortSide = portSide,
                IsAnchorable = isAnchorable,
                AvailableTypes = availableTypes == null ? new string[] { type.ToString() } : availableTypes.Select(el => el.ToString()).ToArray(),
                Attribute = attribute,
                Visibility = visibility,
            };


            //if (!DSUtilities.IsAvalilableType(type)) return (null, data);

            if (isIfPort)
            {
                data.IsInput = true;
                data.IsFunction = true;
                var ifports = Model.Inputs.Where(e => e.IsIfPort).ToList();
                var isIt = ifports.Any(e => e.IfPortSourceId == ifPortSourceId);
                if (isIt)
                    return (null, null);
            }
            Model.AddPort(data);
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
            port.SetValue(data.Value);
            port.ChangeName(data.PortText);
            port.IsFunctions = data.IsFunction;
            port.PortSide = data.PortSide;
            port.IsAnchorable = data.IsAnchorable;
            port.GrathView = graphView;
            port.Visibility = data.Visibility;
            port.Attribute = data.Attribute;
            port.AssetSource = data.AssetSource;

            if (!string.IsNullOrWhiteSpace(data.Anchor) && data.IsAnchorable) port.Anchor = data.Anchor;

            if (data.IsField && data.Type != null)
            {
                switch (data.Type)
                {
                    case Type t when t == typeof(float):
                        float def = 0;
                        if (float.TryParse(data.Value, out def)) {}
                        FloatField floatField = DSUtilities.CreateFloatField(
                        value: def,
                        onChange: callback =>
                        {
                            FloatField target = callback.target as FloatField;
                            target.value = callback.newValue;
                            data.Value = callback.newValue.ToString();
                            port.SetValue(callback.newValue);
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

                    case Type t when t == typeof(bool):
                        bool bDef = data.Value.ToLower() == "true" ? true : false;
                        Toggle toggle = DSUtilities.CreateToggle(
                            "",
                            "",
                            onChange: callBack =>
                            {
                                Toggle target = callBack.target as Toggle;
                                target.value = callBack.newValue;
                                data.Value = callBack.newValue.ToString();
                                port.SetValue(data.Value);
                            },
                            styles: new string[]
                            {
                                "ds-node__toglefield",
                                "ds-node__choice-textfield",
                                "ds-node__textfield__hidden"
                            },
                            value: bDef);
                        port.Add(toggle);
                        break;

                    case Type t when t == typeof(int):
                        int iDef = 0;
                        if (int.TryParse(data.Value, out iDef)) { }
                        IntegerField integetField = DSUtilities.CreateIntegerField(
                        value: iDef,
                        onChange: callback =>
                        {
                            IntegerField target = callback.target as IntegerField;
                            target.value = callback.newValue;
                            data.Value = callback.newValue.ToString();
                            port.SetValue(callback.newValue);

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

                    case Type t when t == typeof(string) ||  t == typeof(Dialogue):
                        TextField Text = DSUtilities.CreateTextField(
                        data.Value,
                        onChange: callback =>
                        {
                            TextField target = callback.target as TextField;
                            target.value = callback.newValue;
                            data.Value = callback.newValue;
                            port.SetValue(callback.newValue);
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

                    case Type t when t == typeof(double):
                        def = 0;
                        if (float.TryParse(data.Value, out def)) { }
                        FloatField floatField2 = DSUtilities.CreateFloatField(
                        def,
                        onChange: callback =>
                        {
                            FloatField target = callback.target as FloatField;
                            target.value = callback.newValue;
                            data.Value = callback.newValue.ToString();
                            port.SetValue(callback.newValue);
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
                    port.OnDistroy();
                    if (!string.IsNullOrEmpty(data.IfPortSourceId) && !string.IsNullOrWhiteSpace(data.IfPortSourceId))
                    {
                        BasePort sourcePort = graphView.GetPortById(data.IfPortSourceId);
                        sourcePort.IfPortSource = null;
                    }
                    if (data.IsInput)
                    {
                        if (Model.Inputs.Count == Model.Minimal)
                            return;
                    }
                    else if (Model.Outputs.Count == Model.Minimal) return;

                    if (port.connected)
                    {
                        var connect = port.connections.ToList();
                        for (int i = 0; i < connect.Count(); i++)
                        {
                            BasePort inp = connect[i].input as BasePort;
                            BasePort outp = connect[i].output as BasePort;
                            inp.Disconnect(connect[i]);
                            outp.Disconnect(connect[i]);
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
            if (data.PlusIf)
            {
                Button plusBtn = DSUtilities.CreateButton(
                "+if",
                () =>
                {
                    var t = AddPortByType(
                        ID: Guid.NewGuid().ToString(),
                        portText: $"If({DSConstants.Bool})",
                        portSide: PortSide.Input,
                        type: typeof(bool),
                        value: "Choice",
                        isInput: false,
                        isSingle: false,
                        isField: false,
                        cross: true,
                        isIfPort: true,
                        availableTypes: new Type[] { typeof(bool) },
                        ifPortSourceId: port.ID);
                },
                styles: new string[]
                {
                    "ds-node__button"
                });

                port.Add(plusBtn);
            }
            if (data.IsIfPort)
            {
                if (string.IsNullOrEmpty(data.IfPortSourceId))
                {
                    var outputs = outputContainer.Children().ToList();
                    BasePort lastPort = outputs[outputs.Count - 1] as BasePort;
                    port.IfPortSource = lastPort;
                    lastPort.IfPortSource = port;
                    lastPort.Add(port);
                    data.IfPortSourceId = lastPort.ID;
                }
                else
                {
                    BasePort outPort = GetOutputPorts().Where(x => x != null && x.ID == data.IfPortSourceId).FirstOrDefault();
                    if (outPort != null)
                    {
                        outPort.Add(port);
                        port.IfPortSource = outPort;
                        outPort.IfPortSource = port;
                    }
                    
                }
            }
            else
            {
                if (data.IsInput) inputContainer.Add(port);
                else outputContainer.Add(port);
            }
            if (!string.IsNullOrWhiteSpace(data.Anchor)) port.AddOrUpdateAnchor(data.Anchor);

            return (port, data);
        }

        protected void ChangePortValueAndType(BasePort port, Type type, string portname = null)
        {
            port.SetPortType(type);
            port.ChangeName(portname == null ? GHelper.GetShortVarType(type) : portname);
            var model_port = Model.Inputs.FirstOrDefault(p => p.PortID == port.ID);
            if (model_port == null) model_port = Model.Outputs.FirstOrDefault(p => p.PortID == port.ID);
            if (model_port != null)
                model_port.Type = type;
        }

        protected void ChangeOutputPortsTypeAndName(Type type, string portname = null)
        {
            List<BasePort> outs = GetOutputPorts();
            if (outs != null)
            {
                foreach (var outPort in outs)
                    ChangePortValueAndType(outPort, type, portname);
            }
        }
        #endregion


        #region Lambdas
        internal virtual string LambdaGenerationContext(MethodParamsInfo[] inputVariables, MethodParamsInfo[] outputVariables) => string.Empty;
        #endregion
    }

    internal class PortInfo
    {
        internal object Value;
        internal Type Type;
        internal BasePort port;
        internal BaseNode node;
    }
}
