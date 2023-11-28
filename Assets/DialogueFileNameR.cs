using UnityEngine;
using System;
using static DialogueSystem.DialogueDisposer.DSDialogueOption;

public class DialogueFileNameR : DialogueSystem.DialogueDisposer
{
    #region Fields
    [SerializeField] private DSDialogue DSDialogue_0 = new();
    [SerializeField] private DSDialogue DSDialogue_1 = new();
    [SerializeField] private DSMore_Or_Equal_d_d_b DSMore_Or_Equal_d_d_b_0 = new();
    [SerializeField] private ActorNode_TestActor ActorNode_TestActor_0 = new();
    [SerializeField] private DSFloat_f DSFloat_f_0 = new();
    [SerializeField] private DSDialogue DSDialogue_2 = new();
    [SerializeField] private DSDialogue DSDialogue_3 = new();
    #endregion
    #region Methods
    public void Initialize(DialogueSystem.Assets.TestActor actor)
    {
        DSDialogue_0.Text = "Dialogue text";
        DSDialogue_1.Text = "End";
        DSDialogue_1.DSDialogueOption = new System.Collections.Generic.List<DialogueSystem.DialogueDisposer.DSDialogueOption>
        {

        };
        ActorNode_TestActor_0.Actor = actor;
        DSMore_Or_Equal_d_d_b_0.Number_0 = ActorNode_TestActor_0.Actor.Age;
        DSFloat_f_0.Single_0 = 15f;
        DSMore_Or_Equal_d_d_b_0.Number_1 = DSFloat_f_0.Single_0;
        DSMore_Or_Equal_d_d_b_0.Boolean_2 = () => {
            return Convert.ToDouble(DSMore_Or_Equal_d_d_b_0.Number_0) >= Convert.ToDouble(DSMore_Or_Equal_d_d_b_0.Number_1);
        };
        DSDialogue_2.Text = "End2";
        DSDialogue_2.DSDialogueOption = new System.Collections.Generic.List<DialogueSystem.DialogueDisposer.DSDialogueOption>
        {

        };
        DSDialogue_0.DSDialogueOption = new System.Collections.Generic.List<DialogueSystem.DialogueDisposer.DSDialogueOption>{
            new DialogueSystem.DialogueDisposer.DSDialogueOption(text: "Next Choice", nextDialogues: DSDialogue_1, func:  () => DSMore_Or_Equal_d_d_b_0.Boolean_2()),
            new DialogueSystem.DialogueDisposer.DSDialogueOption(text: "Choice", nextDialogues: DSDialogue_2)
        };
        DSDialogue_3.Text = "Start";
        DSDialogue_3.DSDialogueOption = new System.Collections.Generic.List<DialogueSystem.DialogueDisposer.DSDialogueOption>{
            new DialogueSystem.DialogueDisposer.DSDialogueOption(text: "Hello", nextDialogues: DSDialogue_0)
        };

        TestDialogue(DSDialogue_3);
    }
    #endregion
    #region InnerClasses
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
    private class ActorNode_TestActor
    {
        #region Fields
        [SerializeField] public DialogueSystem.Assets.TestActor Actor;
        #endregion

    }
    [System.Serializable]
    private class DSFloat_f
    {
        #region Fields
        [SerializeField] public System.Single Single_0;
        #endregion

    }
    #endregion

}