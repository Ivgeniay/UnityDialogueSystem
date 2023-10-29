using UnityEngine;
using UnityEngine.UIElements;

namespace DialogueSystem.Manipulations
{
    public class StartDragManipulator : MouseManipulator
    {
        public StartDragManipulator()
        {
            activators.Add(new ManipulatorActivationFilter { button = MouseButton.LeftMouse });
        }

        protected override void RegisterCallbacksOnTarget()
        {
            target.RegisterCallback<MouseDownEvent>(OnMouseDown);
        }

        protected override void UnregisterCallbacksFromTarget()
        {
            target.UnregisterCallback<MouseDownEvent>(OnMouseDown);
        }

        private void OnMouseDown(MouseDownEvent e)
        {
            var result = CanStartManipulation(e);
            if (result)
            {
                Debug.Log("Edge drag started!");
                e.StopPropagation();
            }
        }
    }
}
