﻿using DialogueSystem.Database.Save;
using DialogueSystem.Nodes;
using System;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace DialogueSystem.Groups
{
    public class BaseGroup : Group
    {
        private Color defaultBorderColor;
        private float defaultBorderWidth;
        public DSGroupModel Model { get; protected set; }

        public BaseGroup(string groupTitle, Vector2 position)
        {
            title = groupTitle;

            Model = new()
            {
                ID = Guid.NewGuid().ToString(),
                Type = GetType().ToString(),
                GroupName = groupTitle,
                Position = this.GetPosition().position
            };

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
        public virtual void OnTitleChanged(string title) 
        {
            Model.GroupName = title;
        }
        public virtual void OnCreate(List<BaseNode> innerNode){}
        public virtual void OnDestroy(){}
        public void OnChangePosition(Vector2 position, Vector2 delta) 
        {
            Model.Position = position;
        }
        #endregion
    }
}
