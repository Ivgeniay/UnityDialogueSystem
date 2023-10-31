using DialogueSystem.Ports;
using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace DialogueSystem.Manipulations
{
    public class StartDragManipulator : MouseManipulator
    {
        private BasePort basePort;
        public StartDragManipulator(BasePort basePort)
        {
            BasePortManager.Register(basePort);
            this.basePort = basePort;

            activators.Add(new ManipulatorActivationFilter { button = MouseButton.LeftMouse });
        }

        protected override void RegisterCallbacksOnTarget()
        {
            target.RegisterCallback<MouseDownEvent>(OnMouseDown);
            target.RegisterCallback<MouseUpEvent>(OnMouseUp);
        }


        protected override void UnregisterCallbacksFromTarget()
        {
            target.UnregisterCallback<MouseDownEvent>(OnMouseDown);
            target.RegisterCallback<MouseUpEvent>(OnMouseUp);
        }

        private void OnMouseUp(MouseUpEvent e)
        {
            var result = CanStartManipulation(e);
            if (result)
            {
                //Debug.Log($"Edge drag stop from {basePort.portName}");
                //BasePortManager.CallStopDrag(basePort);
                e.StopPropagation();
            }
        }

        private void OnMouseDown(MouseDownEvent e)
        {
            var result = CanStartManipulation(e);
            if (result)
            {
                //Debug.Log($"Edge drag started from {basePort.portName}");
                //BasePortManager.CallStartDrag(basePort);
                e.StopPropagation();
            }
        }
    }
}
