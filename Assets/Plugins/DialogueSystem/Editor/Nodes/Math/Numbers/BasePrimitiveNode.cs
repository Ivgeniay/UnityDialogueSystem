using UnityEngine.UIElements;

namespace DialogueSystem.Nodes
{
    internal abstract class BasePrimitiveNode : BaseMathNode
    {
        protected override void DrawExtensionContainer(VisualElement container)
        {
            base.DrawExtensionContainer(container);
            InitializeSettingElement(container);
        }
    }
}
