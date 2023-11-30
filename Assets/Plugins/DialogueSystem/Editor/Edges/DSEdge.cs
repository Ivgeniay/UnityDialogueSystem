using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

namespace DialogueSystem.Edges
{
    public class DSEdge : Edge
    {
        public DSEdge() 
        {
            AddManipulators();
        }

        public void AddManipulators()
        {
            this.AddManipulator(CreateEdgeContextualMenu());
        }

        private IManipulator CreateEdgeContextualMenu()
        {
            ContextualMenuManipulator contextualMenuManipulator = new(e =>
            {
                //e.menu.AppendAction("Add Token Node", a =>
                //{
                //    //TokenNode tokenNode = new(input, output);
                    
                //});
            });

            return contextualMenuManipulator;
        }
    }
}
