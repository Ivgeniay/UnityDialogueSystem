using DialogueSystem.Nodes;
using DialogueSystem.Window;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using System;

namespace DialogueSystem
{
    public class ActorNode : BaseNode
    {
        public Type ActorType { 
            get
            {
                string tyStr = this.Model.Text;
                Type ty = Type.GetType(tyStr);
                if (ty == null)
                {
                    string assemblyFullName = "Assembly-CSharp";
                    Assembly assembly = Assembly.Load(assemblyFullName);
                    ty = assembly.GetType(tyStr);
                }
                return ty;
            }
        }

        private Dictionary<string, Type> publicFields = new(); 
        private Dictionary<string, Type> publicProperties = new();
        internal override void Initialize(DSGraphView graphView, Vector2 position, List<object> portsContext)
        {
            base.Initialize(graphView, position, context: portsContext);
        }

        public void Generate(Type type)
        {
            Model.Text = type.ToString();
            publicFields = GetPublicFields(type);
            publicProperties = GetPublicProperties(type);

            foreach (var item in publicFields)
            {
                AddPortByType(
                    ID: Guid.NewGuid().ToString(),
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
                    }
                );
            }
            foreach (var item in publicProperties)
            {
                AddPortByType(
                    ID: Guid.NewGuid().ToString(),
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
                    }
                );
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
