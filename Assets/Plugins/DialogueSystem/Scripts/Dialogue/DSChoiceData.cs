﻿using System;
using UnityEngine;

namespace DialogueSystem.Database.Save
{
    [Serializable]
    public class DSChoiceData
    {
        //[field: SerializeField] public string NodeID {  get; set; }
        [field: SerializeField] public string Text {  get; set; }
        [field: SerializeField] public DSDialogueSO NextDialogue {  get; set; }
    }
}