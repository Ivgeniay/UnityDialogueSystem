using DialogueSystem.Nodes;
using System;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace DialogueSystem.Groups
{
    public class BaseGroup : Group
    {
        public string ID { get; set; }
        public string OldTitle { get; set; }
        private Color defaultBorderColor;
        private float defaultBorderWidth;

        public BaseGroup(string groupTitle, Vector2 position)
        {
            ID = Guid.NewGuid().ToString();

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
        public virtual void OnCreate(List<BaseNode> innerNode){}
        public virtual void OnDestroy(){}
        public void OnChangePosition(Vector2 position){}
        #endregion
    }
}
