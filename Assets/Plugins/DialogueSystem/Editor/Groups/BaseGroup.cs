using DialogueSystem.Nodes;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace DialogueSystem.Groups
{
    public class BaseGroup : Group
    {
        public string OldTitle { get; set; }
        private Color defaultBorderColor;
        private float defaultBorderWidth;

        public BaseGroup(string groupTitle, Vector2 position)
        {
            title = groupTitle;
            OldTitle = groupTitle;

            SetPosition(new Rect(position, Vector2.zero));

            defaultBorderColor = contentContainer.style.borderBottomColor.value;
            defaultBorderWidth = contentContainer.style.borderBottomWidth.value;
        }

        #region Styles
        public void SetErrorStyle(Color color)
        {
            contentContainer.style.borderBottomColor = color;
            contentContainer.style.borderBottomWidth = 2f;
        }

        public void ResetStyle()
        {
            contentContainer.style.borderBottomColor = defaultBorderColor;
            contentContainer.style.borderBottomWidth = defaultBorderWidth;
        }
        #endregion

        #region Mono
        public virtual void OnCreate(List<BaseNode> innerNode)
        {

        }

        public virtual void OnDestroy()
        {

        }
        #endregion 
    }
}
