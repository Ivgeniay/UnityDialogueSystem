using DialogueSystem.Utilities;
using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

namespace DialogueSystem.MiniMaps
{
    internal class DSMiniMap : MiniMap
    {
        //private const string MINIMAP_STYLE_LINK = "Assets/Plugins/DialogueSystem/Resources/Front/DialogueSystemMinimapStyles.uss";
    
        public DSMiniMap()
        {
            AddStyles();
        }

        private void AddStyles()
        {
            //this.LoadAndAddStyleSheets(MINIMAP_STYLE_LINK);
            StyleColor backgroundColor = new StyleColor(new UnityEngine.Color32(29, 29, 30, 255));
            StyleColor borderColor = new StyleColor(new UnityEngine.Color32(29, 29, 30, 255));

            style.backgroundColor = backgroundColor;
            style.borderLeftColor = borderColor;
            style.borderRightColor = borderColor;
            style.borderTopColor = borderColor;
            style.borderBottomColor = borderColor;
        }
    }
}
