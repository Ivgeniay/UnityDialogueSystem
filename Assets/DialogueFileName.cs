using UnityEngine;	
using System;
using Unity.VisualScripting.Antlr3.Runtime.Tree;

public class DialogueFileName : DialogueSystem.DialogueDisposer
{
    #region Fields
    [SerializeField] private ActorNode_TestActor2 ActorNode_TestActor2_0 = new();
    [SerializeField] private DSString_s DSString_s_0 = new();
    [SerializeField] private DSMultiply_d_s_s DSMultiply_d_s_s_0 = new();
    #endregion
    #region Methods
    public void Initialize(DialogueSystem.Assets.TestActor2 actor)
    {
        ActorNode_TestActor2_0.Actor = actor;
        DSString_s_0.String_0 = "Alarm";
        DSMultiply_d_s_s_0.AAll_0 = ActorNode_TestActor2_0.Actor.Ammo;
        DSMultiply_d_s_s_0.String_1 = DSString_s_0.String_0;
        DSMultiply_d_s_s_0.String_2 = () => {
            string LambdaGen(params object[] parameters)
            {

                string concatenatedString = "";
                foreach (var param in parameters)
                {
                    if (param is string str) concatenatedString += str;
                    else if (param is int num) concatenatedString = string.Concat(System.Linq.Enumerable.Repeat(concatenatedString, num));
                    else if (param is double dbl) concatenatedString = string.Concat(System.Linq.Enumerable.Repeat(concatenatedString, (int)dbl));
                    else if (param is float fl) concatenatedString = string.Concat(System.Linq.Enumerable.Repeat(concatenatedString, (int)fl));
                    else if (param is bool b) concatenatedString += string.Concat(System.Linq.Enumerable.Repeat(concatenatedString, b == false ? 0 : 1));

                }
                return concatenatedString;
            }
            return LambdaGen(DSMultiply_d_s_s_0.AAll_0, DSMultiply_d_s_s_0.String_1);
        };

        var result = DSMultiply_d_s_s_0.String_2();
        Debug.Log(result);
    }
    #endregion
    #region InnerClasses
    [System.Serializable]
    private class ActorNode_TestActor2
    {
        #region Fields
        [SerializeField] public DialogueSystem.Assets.TestActor2 Actor;
        #endregion

    }
    [System.Serializable]
    private class DSString_s
    {
        #region Fields
        [SerializeField] public System.String String_0;
        #endregion

    }
    [System.Serializable]
    private class DSMultiply_d_s_s
    {
        #region Fields
        [SerializeField] public System.Double AAll_0;
        [SerializeField] public System.String String_1;
        [SerializeField] public Func<System.String> String_2;
        #endregion

    }
    #endregion

}