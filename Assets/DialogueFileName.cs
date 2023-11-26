using UnityEngine;	
using System;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using static DialogueSystem.DialogueOption;
using System.Collections.Generic;
using DialogueSystem;
using static UnityEditor.Progress;
using static DialogueFileName;
using static DialogueSystem.DialogueDisposer.DSDialogueOption;
using System.Linq;

public class DialogueFileName : DialogueSystem.DialogueDisposer
{
    #region Fields
    [SerializeField] private DSDialogue DSDialogue_0 = new();
    [SerializeField] private DSDialogue DSDialogue_1 = new();
    [SerializeField] private DSTo_Bool_b_b DSTo_Bool_b_b_0 = new();
    [SerializeField] private DSMore_Or_Equal_d_d_b DSMore_Or_Equal_d_d_b_0 = new();
    [SerializeField] private DSAddition_f_i_d DSAddition_f_i_d_0 = new();
    [SerializeField] private DSFloat_f DSFloat_f_0 = new();
    [SerializeField] private DSInteger_i DSInteger_i_0 = new();
    [SerializeField] private DSDouble_d DSDouble_d_0 = new();
    [SerializeField] private DSDialogue DSDialogue_2 = new();
    #endregion
    #region Methods

    private void Awake()
    {
        Initialize();
    }
    public void Initialize()
    {
        DSDialogue_0.Text = "Multi";
        DSDialogue_1.Text = "StopDialogue";
        DSDialogue_1.DSDialogueOptions = new System.Collections.Generic.List<DialogueSystem.DialogueDisposer.DSDialogueOption>
        {

        };
        DSFloat_f_0.Single_0 = 23f;
        DSAddition_f_i_d_0.Single_0 = DSFloat_f_0.Single_0;
        DSInteger_i_0.Int32_0 = 30;
        DSAddition_f_i_d_0.Int32_1 = DSInteger_i_0.Int32_0;
        DSAddition_f_i_d_0.Double_2 = () => {
            return DSAddition_f_i_d_0.Single_0 + DSAddition_f_i_d_0.Int32_1;
        };
        DSMore_Or_Equal_d_d_b_0.Number_0 = DSAddition_f_i_d_0.Double_2();
        DSDouble_d_0.Double_0 = 55d;
        DSMore_Or_Equal_d_d_b_0.Number_1 = DSDouble_d_0.Double_0;
        DSMore_Or_Equal_d_d_b_0.Boolean_2 = () => {
            return Convert.ToDouble(DSMore_Or_Equal_d_d_b_0.Number_0) >= Convert.ToDouble(DSMore_Or_Equal_d_d_b_0.Number_1);
        };
        DSTo_Bool_b_b_0.Boolean_0 = DSMore_Or_Equal_d_d_b_0.Boolean_2();
        DSTo_Bool_b_b_0.Boolean_1 = () => {
            return DSTo_Bool_b_b_0.Boolean_0;
        };
        DSDialogue_0.DSDialogueOptions = new System.Collections.Generic.List<DialogueSystem.DialogueDisposer.DSDialogueOption>{
            new DialogueSystem.DialogueDisposer.DSDialogueOption(text: "Next Choice", nextDialogues: DSDialogue_1, func:  () => DSTo_Bool_b_b_0.Boolean_1())
        };
        DSDialogue_2.Text = "StartDialogue";
        DSDialogue_2.DSDialogueOptions = new System.Collections.Generic.List<DialogueSystem.DialogueDisposer.DSDialogueOption>{
            new DialogueSystem.DialogueDisposer.DSDialogueOption(text: "Hello", nextDialogues: DSDialogue_0)
        };

        Dialogue(DSDialogue_2);

    }
    #endregion
    #region InnerClasses
    [System.Serializable]
    private class DSTo_Bool_b_b
    {
        #region Fields
        [SerializeField] public System.Boolean Boolean_0;
        [SerializeField] public Func<System.Boolean> Boolean_1;
        #endregion

    }
    [System.Serializable]
    private class DSMore_Or_Equal_d_d_b
    {
        #region Fields
        [SerializeField] public System.Double Number_0;
        [SerializeField] public System.Double Number_1;
        [SerializeField] public Func<System.Boolean> Boolean_2;
        #endregion

    }
    [System.Serializable]
    private class DSAddition_f_i_d
    {
        #region Fields
        [SerializeField] public System.Single Single_0;
        [SerializeField] public System.Int32 Int32_1;
        [SerializeField] public Func<System.Double> Double_2;
        #endregion

    }
    [System.Serializable]
    private class DSFloat_f
    {
        #region Fields
        [SerializeField] public System.Single Single_0;
        #endregion

    }
    [System.Serializable]
    private class DSInteger_i
    {
        #region Fields
        [SerializeField] public System.Int32 Int32_0;
        #endregion

    }
    [System.Serializable]
    private class DSDouble_d
    {
        #region Fields
        [SerializeField] public System.Double Double_0;
        #endregion

    }
    #endregion

}