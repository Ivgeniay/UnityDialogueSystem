using UnityEngine;	
using System;	
using DialogueSystem;	
using DialogueSystem.Nodes;
using UnityEngine.Events;
using System.Globalization;
using System.Linq;

public class DialogueFileNameTest : MonoBehaviour
{
    private void Awake()
    {
        Initialize();
        Debug.Log(SubtractNode_2.SubtractNode80_Double_0());

        Debug.Log(ConvertTypeToString(typeof(int)));
        Debug.Log(ConvertTypeToString(typeof(string)));
        Debug.Log(ConvertTypeToString(typeof(Func<int>)));
        Debug.Log(ConvertTypeToString(typeof(Func<int, int>)));
        Debug.Log(ConvertTypeToString(typeof(Action<int, int>)));
        Debug.Log(ConvertTypeToString(typeof(Action<int, int, string>)));

    }
    public string ConvertTypeToString(Type type)
    {
        string typeName = type.ToString();
        int backtickIndex = typeName.IndexOf('`');
        if (backtickIndex >= 0)
        {
            typeName = typeName.Remove(backtickIndex, 2);
            typeName += "<";

            Type[] genericArguments = type.GetGenericArguments();
            for (int i = 0; i < genericArguments.Length; i++)
            {
                typeName += i > 0 ? ", " : "";
                typeName += ConvertTypeToString(genericArguments[i]);
            }

            typeName += ">";
        }
        if (typeName.IndexOf("[") != -1)
        {
            var startIndex = typeName.IndexOf("[");
            var finishIndex = typeName.IndexOf("]") + 1;
            typeName = typeName.Remove(startIndex, finishIndex - startIndex);
        }
        return typeName;
    }


    #region Properties
    [field: SerializeField] private DSInteger IntegerNode_0 { get; set; }
    [field: SerializeField] private DSFloat FloatNode_1 { get; set; }
    [field: SerializeField] private DSSubtract SubtractNode_2 { get; set; }
    #endregion
    #region Methods
    private void Initialize()
    {
        IntegerNode_0 = new()
        {
            IntegerNode99_Int32_0 = 2323,
        };
        FloatNode_1 = new()
        {
            FloatNode70_Single_0 = 2f,
        };
        SubtractNode_2 = new()
        {
            SubtractNode80_Double_0 = () => {
                return IntegerNode_0.IntegerNode99_Int32_0 - FloatNode_1.FloatNode70_Single_0;
            },
        };

    }
    #endregion
    #region ClassDeclaration

    [System.Serializable]
    private class DSInteger
    {
        [field: SerializeField] public int IntegerNode99_Int32_0 { get; set; }

    }

    [System.Serializable]
    private class DSFloat
    {
        [field: SerializeField] public float FloatNode70_Single_0 { get; set; }

    }

    [System.Serializable]
    private class DSSubtract
    {
        [field: SerializeField] public Func<Double> SubtractNode80_Double_0 { get; set; }

    }
    #endregion

}