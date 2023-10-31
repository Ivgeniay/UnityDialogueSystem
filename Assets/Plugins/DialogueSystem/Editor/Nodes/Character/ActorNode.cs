using DialogueSystem.Nodes;
using DialogueSystem.Window;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace DialogueSystem
{
    public class ActorNode : BaseNode
    {
        private Dictionary<string, Type> publicFields = new(); 
        private Dictionary<string, Type> publicProperties = new(); 
        private Dictionary<string, Type> privateFields = new(); 
        private Dictionary<string, Type> privateProperties = new(); 
        internal override void Initialize(DialogueSystemGraphView graphView, Vector2 position)
        {
            base.Initialize(graphView, position);
        }

        public void Generate(Type type)
        {
            publicFields = GetPublicFields(type);
            publicProperties = GetPublicProperties(type);
            privateFields = GetPrivateFields(type);
            privateProperties = GetPrivateProperties(type);

            foreach (var item in publicFields)
            {
                var t = AddPortByType(
                        portText: $"{item.Key} ({item.Value.Name})",
                        type: item.Value,
                        value: null,
                        isInput: false,
                        isSingle: false,
                        isField: false,
                        cross: false,
                        availableTypes: new Type[]
                        {
                            item.Value
                        });
            }
            foreach (var item in publicProperties)
            {
                var t = AddPortByType(
                        portText: $"{item.Key} ({item.Value.Name})",
                        type: item.Value,
                        value: null,
                        isInput: false,
                        isSingle: false,
                        isField: false,
                        cross: false,
                        availableTypes: new Type[]
                        {
                            item.Value
                        });
            }
            foreach (var item in privateFields)
            {
                var t = AddPortByType(
                        portText: $"{item.Key} ({item.Value.Name})",
                        type: item.Value,
                        value: null,
                        isInput: false,
                        isSingle: false,
                        isField: false,
                        cross: false, 
                        availableTypes: new Type[] { item.Value });
            }
            foreach (var item in privateProperties)
            {
                var t = AddPortByType(
                        portText: $"{item.Key} ({item.Value.Name})",
                        type: item.Value,
                        value: null,
                        isInput: false,
                        isSingle: false,
                        isField: false,
                        cross: false,
                        availableTypes: new Type[] { item.Value });
            }

            RefreshExpandedState();
        }

        public static Dictionary<string, Type> GetPublicFields(Type type)
        {
            Dictionary<string, Type> fieldDictionary = new Dictionary<string, Type>();

            FieldInfo[] fields = type.GetFields(BindingFlags.Public | BindingFlags.Instance);
            foreach (var field in fields)
                fieldDictionary.Add(field.Name, field.FieldType);

            return fieldDictionary;
        }
        public static Dictionary<string, Type> GetPublicProperties(Type type)
        {
            Dictionary<string, Type> propertyDictionary = new Dictionary<string, Type>();

            PropertyInfo[] properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var property in properties)
                propertyDictionary.Add(property.Name, property.PropertyType);
            
            return propertyDictionary;
        }
        public static Dictionary<string, Type> GetPrivateFields(Type type)
        {
            Dictionary<string, Type> result = new Dictionary<string, Type>();

            foreach (FieldInfo field in type.GetFields(BindingFlags.NonPublic | BindingFlags.Instance))
            {
                if (!IsBackingFieldForProperty(field.Name))
                    result.Add(field.Name, field.FieldType);
            }
            
            return result;
        }
        public static Dictionary<string, Type> GetPrivateProperties(Type type)
        {
            Dictionary<string, Type> result = new Dictionary<string, Type>();

            foreach (PropertyInfo prop in type.GetProperties(BindingFlags.NonPublic | BindingFlags.Instance))
                result.Add(prop.Name, prop.PropertyType);

            return result;
        }

        private static bool IsBackingFieldForProperty(string field) => field.StartsWith("<");
    }
}
