using UnityEngine.UIElements;

namespace DialogueSystem.Nodes
{
    internal abstract class BaseLetterNode : BaseNode
    {
        protected override void DrawExtensionContainer(VisualElement container)
        {
            base.DrawExtensionContainer(container);
            InitializeSettingElement(container);
        }
    }
}
