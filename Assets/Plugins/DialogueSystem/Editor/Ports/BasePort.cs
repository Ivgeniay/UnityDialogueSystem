using UnityEditor.Experimental.GraphView;
using DialogueSystem.Manipulations;
using UnityEngine.UIElements;
using DialogueSystem.Edges;
using System.Reflection;
using UnityEngine;
using System;

namespace DialogueSystem.Ports
{
    public class BasePort : Port
    {
        public string ID { get; set; }
        public object Value;
        public Type[] AvailableTypes;
        public Color capColor;
        
        public BasePort(Orientation portOrientation, Direction portDirection, Capacity portCapacity, Type type) : base(portOrientation, portDirection, portCapacity, type) 
        { }
        public static BasePort CreateBasePort<TEdge>(Orientation orientation, Direction direction, Capacity capacity, Type type) where TEdge : DSEdge, new()
        {
            //Type edgeConnectorType = typeof(EdgeConnector<>).MakeGenericType(typeof(TEdge));
            Type edgeConnectorType = typeof(CustomEdgeConnector<>).MakeGenericType(typeof(TEdge));
            Type outerType = typeof(Port);
            Type innerType = outerType.GetNestedType("DefaultEdgeConnectorListener", BindingFlags.NonPublic);

            object listenerInstance = Activator.CreateInstance(innerType);
            object edgeConnectorInstance = Activator.CreateInstance(edgeConnectorType, listenerInstance);

            BasePort port = new BasePort(orientation, direction, capacity, type);

            Type portType = typeof(BasePort);
            FieldInfo edgeConnectorField = portType.GetField("m_EdgeConnector", BindingFlags.NonPublic | BindingFlags.Instance);

            if (edgeConnectorField != null)
                edgeConnectorField.SetValue(port, edgeConnectorInstance);

            port.AddManipulator(port.m_EdgeConnector);

            StartDragManipulator startDrag= new StartDragManipulator(port);
            port.AddManipulator(startDrag);

            return port;
        }

        public override void Connect(Edge edge)
        {
            base.Connect(edge);
            if (edge.output != null && edge.input == this)
            {
                var other = edge.output as BasePort;
                if (!BasePortManager.HaveCommonTypes(AvailableTypes, other.AvailableTypes))
                {
                    //var bNode = this.node as BaseNode;
                    //Debug.Log($"its not ok from output {this.portName} with {bNode.Model.NodeName}");
                    Disconnect(edge);
                }
            }
        }
        public override void Disconnect(Edge edge)
        {
            base.Disconnect(edge);

            if (edge != null)
            {
                GraphView graphView = GetFirstAncestorOfType<GraphView>();
                if (graphView != null)
                {
                    var t = new GraphViewChange();
                    t.elementsToRemove = new() { edge };
                    graphView.graphViewChanged?.Invoke(t);
                    graphView.RemoveElement(edge);
                }
                
                //edge.parent.Remove(edge);
            }
        }

        public override void OnStartEdgeDragging()
        {
            if (this.m_EdgeConnector?.edgeDragHelper?.draggedPort == this)
                BasePortManager.CallStartDrag(this);
            base.OnStartEdgeDragging();
        }
        public override void OnStopEdgeDragging()
        {
            base.OnStopEdgeDragging();
            
            if (this.m_EdgeConnector?.edgeDragHelper?.draggedPort == this)
                BasePortManager.CallStopDrag(this);
        }


        internal void ChangeType(Type type)
        {
            portType = type;
        }
        internal void ChangeName(string name)
        {
            portName = name;
        }
    }


    /*
    public class Edge : GraphElement
    {
        private const float k_EndPointRadius = 4f;

        private const float k_InterceptWidth = 6f;

        private static CustomStyleProperty<int> s_EdgeWidthProperty = new CustomStyleProperty<int>("--edge-width");

        private static CustomStyleProperty<Color> s_SelectedEdgeColorProperty = new CustomStyleProperty<Color>("--selected-edge-color");

        private static CustomStyleProperty<Color> s_GhostEdgeColorProperty = new CustomStyleProperty<Color>("--ghost-edge-color");

        private static CustomStyleProperty<Color> s_EdgeColorProperty = new CustomStyleProperty<Color>("--edge-color");

        private static readonly int s_DefaultEdgeWidth = 2;

        private static readonly Color s_DefaultSelectedColor = new Color(0.9411765f, 0.9411765f, 0.9411765f);

        private static readonly Color s_DefaultColor = new Color(146f / 255f, 146f / 255f, 146f / 255f);

        private static readonly Color s_DefaultGhostColor = new Color(0.333333343f, 0.333333343f, 0.333333343f);

        private GraphView m_GraphView;

        private BasePort m_OutputPort;

        private BasePort m_InputPort;

        private Vector2 m_CandidatePosition;

        private Vector2 m_GlobalCandidatePosition;

        private EdgeControl m_EdgeControl;

        private int m_EdgeWidth = s_DefaultEdgeWidth;

        private Color m_SelectedColor = s_DefaultSelectedColor;

        private Color m_DefaultColor = s_DefaultColor;

        private Color m_GhostColor = s_DefaultGhostColor;

        private bool m_EndPointsDirty;

        //
        // Сводка:
        //     Is this edge a ghost edge, which is the edge that appears snapped to a nearby
        //     port while an edge is being created.
        public bool isGhostEdge { get; set; }

        //
        // Сводка:
        //     Connected output port.
        public BasePort output
        {
            get
            {
                return m_OutputPort;
            }
            set
            {
                if (m_OutputPort != null && value != m_OutputPort)
                {
                    m_OutputPort.UpdateCapColor();
                    UntrackGraphElement(m_OutputPort);
                }

                if (value != m_OutputPort)
                {
                    m_OutputPort = value;
                    if (m_OutputPort != null)
                    {
                        TrackGraphElement(m_OutputPort);
                    }
                }

                edgeControl.drawFromCap = m_OutputPort == null;
                m_EndPointsDirty = true;
                OnPortChanged(isInput: false);
            }
        }

        //
        // Сводка:
        //     Whether the GraphElement is shown in the minimap. For Edge, this property is
        //     always set to false.
        public override bool showInMiniMap => false;

        //
        // Сводка:
        //     Connected input port.
        public BasePort input
        {
            get
            {
                return m_InputPort;
            }
            set
            {
                if (m_InputPort != null && value != m_InputPort)
                {
                    m_InputPort.UpdateCapColor();
                    UntrackGraphElement(m_InputPort);
                }

                if (value != m_InputPort)
                {
                    m_InputPort = value;
                    if (m_InputPort != null)
                    {
                        TrackGraphElement(m_InputPort);
                    }
                }

                edgeControl.drawToCap = m_InputPort == null;
                m_EndPointsDirty = true;
                OnPortChanged(isInput: true);
            }
        }

        //
        // Сводка:
        //     The VisualElement child of Edge that draws the lines and does the hit detection.
        public EdgeControl edgeControl
        {
            get
            {
                if (m_EdgeControl == null)
                {
                    m_EdgeControl = CreateEdgeControl();
                }

                return m_EdgeControl;
            }
        }

        //
        // Сводка:
        //     The edge's end position while it's being created.
        public Vector2 candidatePosition
        {
            get
            {
                return m_CandidatePosition;
            }
            set
            {
                if (!Approximately(m_CandidatePosition, value))
                {
                    m_CandidatePosition = value;
                    m_GlobalCandidatePosition = this.WorldToLocal(m_CandidatePosition);
                    if (m_InputPort == null)
                    {
                        edgeControl.to = m_GlobalCandidatePosition;
                    }

                    if (m_OutputPort == null)
                    {
                        edgeControl.from = m_GlobalCandidatePosition;
                    }

                    UpdateEdgeControl();
                }
            }
        }

        //
        // Сводка:
        //     Edge width.
        public int edgeWidth => m_EdgeWidth;

        //
        // Сводка:
        //     Color of edge while selected.
        public Color selectedColor => m_SelectedColor;

        //
        // Сводка:
        //     Default edge color.
        public Color defaultColor => m_DefaultColor;

        //
        // Сводка:
        //     The color of the ghost edge, which is the edge that appears snapped to a nearby
        //     port while an edge is being created.
        public Color ghostColor => m_GhostColor;

        //
        // Сводка:
        //     The edge's points and tangents.
        protected Vector2[] PointsAndTangents => edgeControl.controlPoints;

        //
        // Сводка:
        //     Edge's constructor.
        public Edge()
        {
            ClearClassList();
            AddToClassList("edge");
            base.style.position = Position.Absolute;
            Add(edgeControl);
            base.capabilities |= Capabilities.Selectable | Capabilities.Deletable;
            this.AddManipulator(new EdgeManipulator());
            this.AddManipulator(new ContextualMenuManipulator(null));
            RegisterCallback<AttachToPanelEvent>(OnEdgeAttach);
            RegisterCallback<GeometryChangedEvent>(OnGeometryChanged);
            //AddStyleSheetPath("StyleSheets/GraphView/Edge.uss");
        }

        //
        // Сводка:
        //     Check if edge overlaps rectangle.
        //
        // Параметры:
        //   rectangle:
        //     The rectangle.
        //
        // Возврат:
        //     True if edge overlaps the rectangle.
        public override bool Overlaps(Rect rectangle)
        {
            if (!UpdateEdgeControl())
            {
                return false;
            }

            return edgeControl.Overlaps(this.ChangeCoordinatesTo(edgeControl, rectangle));
        }

        //
        // Сводка:
        //     Checks if point is on top of edge.
        //
        // Параметры:
        //   localPoint:
        //     Point position.
        //
        // Возврат:
        //     True if point is on top of edge. False otherwise.
        public override bool ContainsPoint(Vector2 localPoint)
        {
            Profiler.BeginSample("Edge.ContainsPoint");
            bool result = UpdateEdgeControl() && edgeControl.ContainsPoint(this.ChangeCoordinatesTo(edgeControl, localPoint));
            Profiler.EndSample();
            return result;
        }

        //
        // Сводка:
        //     Called when a port on the edge is changed.
        //
        // Параметры:
        //   isInput:
        //     True if the input port was changed. False if the output port was changed.
        public virtual void OnPortChanged(bool isInput)
        {
            edgeControl.outputOrientation = m_OutputPort?.orientation ?? m_InputPort?.orientation ?? Orientation.Horizontal;
            edgeControl.inputOrientation = m_InputPort?.orientation ?? m_OutputPort?.orientation ?? Orientation.Horizontal;
            UpdateEdgeControl();
        }

        internal bool ForceUpdateEdgeControl()
        {
            m_EndPointsDirty = true;
            return UpdateEdgeControl();
        }

        //
        // Сводка:
        //     Update the edge's EdgeControl.
        //
        // Возврат:
        //     False if it failed to update the control. True if it succeeded.
        public virtual bool UpdateEdgeControl()
        {
            if (m_OutputPort == null && m_InputPort == null)
            {
                return false;
            }

            if (m_GraphView == null)
            {
                m_GraphView = GetFirstOfType<GraphView>();
            }

            if (m_GraphView == null)
            {
                return false;
            }

            UpdateEdgeControlEndPoints();
            edgeControl.UpdateLayout();
            UpdateEdgeControlColorsAndWidth();
            return true;
        }

        //
        // Сводка:
        //     Draw the edge's lines.
        protected virtual void DrawEdge()
        {
        }

        private void UpdateEdgeControlColorsAndWidth()
        {
            if (base.selected)
            {
                if (isGhostEdge)
                {
                    Debug.Log("Selected Ghost Edge: this should never be");
                }

                edgeControl.inputColor = selectedColor;
                edgeControl.outputColor = selectedColor;
                edgeControl.edgeWidth = edgeWidth;
                if (m_InputPort != null)
                {
                    m_InputPort.capColor = selectedColor;
                }

                if (m_OutputPort != null)
                {
                    m_OutputPort.capColor = selectedColor;
                }

                return;
            }

            if (m_InputPort != null)
            {
                m_InputPort.UpdateCapColor();
            }

            if (m_OutputPort != null)
            {
                m_OutputPort.UpdateCapColor();
            }

            if (m_InputPort != null)
            {
                edgeControl.inputColor = m_InputPort.portColor;
            }
            else if (m_OutputPort != null)
            {
                edgeControl.inputColor = m_OutputPort.portColor;
            }

            if (m_OutputPort != null)
            {
                edgeControl.outputColor = m_OutputPort.portColor;
            }
            else if (m_InputPort != null)
            {
                edgeControl.outputColor = m_InputPort.portColor;
            }

            edgeControl.edgeWidth = edgeWidth;
            edgeControl.toCapColor = edgeControl.inputColor;
            edgeControl.fromCapColor = edgeControl.outputColor;
            if (isGhostEdge)
            {
                edgeControl.inputColor = new Color(edgeControl.inputColor.r, edgeControl.inputColor.g, edgeControl.inputColor.b, 0.5f);
                edgeControl.outputColor = new Color(edgeControl.outputColor.r, edgeControl.outputColor.g, edgeControl.outputColor.b, 0.5f);
            }
        }

        //
        // Сводка:
        //     Called when the custom style properties are resolved.
        //
        // Параметры:
        //   styles:
        protected override void OnCustomStyleResolved(ICustomStyle styles)
        {
            base.OnCustomStyleResolved(styles);
            int value = 0;
            Color value2 = Color.clear;
            Color value3 = Color.clear;
            Color value4 = Color.clear;
            if (styles.TryGetValue(s_EdgeWidthProperty, out value))
            {
                m_EdgeWidth = value;
            }

            if (styles.TryGetValue(s_SelectedEdgeColorProperty, out value2))
            {
                m_SelectedColor = value2;
            }

            if (styles.TryGetValue(s_EdgeColorProperty, out value3))
            {
                m_DefaultColor = value3;
            }

            if (styles.TryGetValue(s_GhostEdgeColorProperty, out value4))
            {
                m_GhostColor = value4;
            }

            UpdateEdgeControlColorsAndWidth();
        }

        public override void OnSelected()
        {
            UpdateEdgeControlColorsAndWidth();
        }

        public override void OnUnselected()
        {
            UpdateEdgeControlColorsAndWidth();
        }

        //
        // Сводка:
        //     Create the EdgeControl.
        //
        // Возврат:
        //     The created EdgeControl.
        protected virtual EdgeControl CreateEdgeControl()
        {
            return new EdgeControl
            {
                capRadius = 4f,
                interceptWidth = 6f
            };
        }

        private Vector2 GetPortPosition(Port p)
        {
            Vector2 p2 = p.GetGlobalCenter();
            return this.WorldToLocal(p2);
        }

        private void TrackGraphElement(Port port)
        {
            if (port.panel != null)
            {
                DoTrackGraphElement(port);
            }

            port.RegisterCallback<AttachToPanelEvent>(OnPortAttach);
            port.RegisterCallback<DetachFromPanelEvent>(OnPortDetach);
        }

        private void OnPortDetach(DetachFromPanelEvent e)
        {
            Port port = (Port)e.target;
            DoUntrackGraphElement(port);
        }

        private void OnPortAttach(AttachToPanelEvent e)
        {
            Port port = (Port)e.target;
            DoTrackGraphElement(port);
        }

        private void OnEdgeAttach(AttachToPanelEvent e)
        {
            UpdateEdgeControl();
        }

        private void UntrackGraphElement(Port port)
        {
            port.UnregisterCallback<AttachToPanelEvent>(OnPortAttach);
            port.UnregisterCallback<DetachFromPanelEvent>(OnPortDetach);
            DoUntrackGraphElement(port);
        }

        private void DoTrackGraphElement(Port port)
        {
            port.RegisterCallback<GeometryChangedEvent>(OnPortGeometryChanged);
            VisualElement visualElement = port.hierarchy.parent;
            while (visualElement != null && !(visualElement is GraphView.Layer))
            {
                if (visualElement != port.node)
                {
                    visualElement.RegisterCallback<GeometryChangedEvent>(OnGeometryChanged);
                }

                visualElement = visualElement.hierarchy.parent;
            }

            if (port.node != null)
            {
                port.node.RegisterCallback<GeometryChangedEvent>(OnGeometryChanged);
            }
        }

        private void DoUntrackGraphElement(Port port)
        {
            port.UnregisterCallback<GeometryChangedEvent>(OnPortGeometryChanged);
            VisualElement visualElement = port.hierarchy.parent;
            while (visualElement != null && !(visualElement is GraphView.Layer))
            {
                if (visualElement != port.node)
                {
                    port.UnregisterCallback<GeometryChangedEvent>(OnGeometryChanged);
                }

                visualElement = visualElement.hierarchy.parent;
            }

            if (port.node != null)
            {
                port.node.UnregisterCallback<GeometryChangedEvent>(OnGeometryChanged);
            }
        }

        private void OnPortGeometryChanged(GeometryChangedEvent evt)
        {
            Port port = evt.target as Port;
            if (port != null)
            {
                if (port == m_InputPort)
                {
                    edgeControl.to = GetPortPosition(port);
                }
                else if (port == m_OutputPort)
                {
                    edgeControl.from = GetPortPosition(port);
                }
            }

            UpdateEdgeControl();
        }

        private void OnGeometryChanged(GeometryChangedEvent evt)
        {
            ForceUpdateEdgeControl();
        }

        private void UpdateEdgeControlEndPoints()
        {
            if (m_EndPointsDirty)
            {
                Profiler.BeginSample("Edge.UpdateEdgeControlEndPoints");
                m_GlobalCandidatePosition = this.WorldToLocal(m_CandidatePosition);
                if (m_OutputPort != null || m_InputPort != null)
                {
                    edgeControl.to = ((m_InputPort != null) ? GetPortPosition(m_InputPort) : m_GlobalCandidatePosition);
                    edgeControl.from = ((m_OutputPort != null) ? GetPortPosition(m_OutputPort) : m_GlobalCandidatePosition);
                }

                m_EndPointsDirty = false;
                Profiler.EndSample();
            }
        }

        private static bool Approximately(Vector2 v1, Vector2 v2)
        {
            return Mathf.Approximately(v1.x, v2.x) && Mathf.Approximately(v1.y, v2.y);
        }
    }
    */
}
