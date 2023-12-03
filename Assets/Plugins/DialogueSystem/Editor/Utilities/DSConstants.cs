﻿using DialogueSystem.DialogueType;
using System.Collections.Generic;
using UnityEngine;
using System;
using DialogueSystem.Generators;
using UnityEditorInternal;

namespace DialogueSystem.Utilities
{
    public static class DSConstants
    {
        public static Type[] AvalilableTypes { get; private set; }
        public static Type[] NumberTypes { get; private set; }
        public static Type[] PrimitiveTypes { get; private set; }
        public static Type[] DialogueTypes { get; private set; }
        public static Type[] CollectionsTypes { get; private set; }
        public static Type[] TypeTypes { get; private set; } 
        public static Type[] AllTypes { get; private set; } 

        public static string DEFAULT_ASSEMBLY = "Assembly-CSharp"; 

        public readonly static string All;
        public readonly static string Number;
        public readonly static string Dialogue;
        public readonly static string Int;
        public readonly static string String;
        public readonly static string Float;
        public readonly static string Double;
        public readonly static string Bool;

        static DSConstants()
        {
            AvalilableTypes = new Type[]
            {
                typeof(string),
                typeof(int),
                typeof(Int16),
                typeof(Int32),
                typeof(Int64),
                typeof(float),
                typeof(Single),
                typeof(double),
                typeof(bool),
                typeof(Dialogue),
                typeof(DialogueDisposer.DSDialogueOption.DSDialogue)
            };

            PrimitiveTypes = new Type[]
            {
                typeof(string),
                typeof(int),
                typeof(Int16),
                typeof(Int32),
                typeof(Int64),
                typeof(float),
                typeof(Single),
                typeof(double),
                typeof(bool),
            };

            NumberTypes = new Type[]
            {
                typeof(int),
                typeof(Int16),
                typeof(Int32),
                typeof(Int64),
                typeof(float),
                typeof(Single),
                typeof(double),
            };

            DialogueTypes = new Type[] { typeof(Dialogue), typeof(DialogueDisposer.DSDialogueOption.DSDialogue) };
            TypeTypes = new Type[] { typeof(Type) };
            AllTypes = new Type[] { typeof(AllTypes) };

            CollectionsTypes = new Type[]
            {
                typeof(List<object>),
                typeof(Dictionary<object,object>)
            };

            All = "All";
            Int = typeof(int).Name;// GHelper.GetVarType(typeof(int));
            String = typeof(string).Name; //GHelper.GetVarType(typeof(string));
            Float = typeof(float).Name; //GHelper.GetVarType(typeof(float));
            Double = typeof(double).Name;// GHelper.GetVarType(typeof(double));
            Bool = typeof(bool).Name; //GHelper.GetVarType(typeof(bool));
            Number = "Number";
            Dialogue = "Dialogue";
        }
    }
}