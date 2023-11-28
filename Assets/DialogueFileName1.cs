using UnityEngine;	
using System;	
using static DialogueSystem.DialogueDisposer.DSDialogueOption;	

public class DialogueFileName1 : DialogueSystem.DialogueDisposer{
	#region Fields
	[SerializeField] private DSDialogue DSDialogue_0 = new();
	[SerializeField] private DSDialogue DSDialogue_1 = new();
	[SerializeField] private DSMore_Or_Equal_d_i_b DSMore_Or_Equal_d_i_b_0 = new();
	[SerializeField] private DSAddition_i_i_d DSAddition_i_i_d_0 = new();
	[SerializeField] private ActorNode_TestActor ActorNode_TestActor_0 = new();
	[SerializeField] private DSInteger_i DSInteger_i_0 = new();
	[SerializeField] private DSInteger_i DSInteger_i_1 = new();
	[SerializeField] private DSDialogue DSDialogue_2 = new();
	[SerializeField] private DSEqual_s_s_b DSEqual_s_s_b_0 = new();
	[SerializeField] private DSString_s DSString_s_0 = new();
	[SerializeField] private DSDialogue DSDialogue_3 = new();
	[SerializeField] private DSDialogue DSDialogue_4 = new();
	#endregion
	#region Methods
	private void Initialize(DialogueSystem.Assets.TestActor actor){
		DSDialogue_0.Text = $"End dialogue hi!";
		DSDialogue_0.DSDialogueOption = new System.Collections.Generic.List<DialogueSystem.DialogueDisposer.DSDialogueOption>{
			
		};
		DSDialogue_1.Text = $"Middle dialogue";
		ActorNode_TestActor_0.Actor = actor;
		DSAddition_i_i_d_0.Int32_0 = ActorNode_TestActor_0.Actor.Age;
		DSInteger_i_0.Int32_0 = 1;
		DSAddition_i_i_d_0.Int32_1 = DSInteger_i_0.Int32_0;
		DSAddition_i_i_d_0.Double_2 = () =>{
			return DSAddition_i_i_d_0.Int32_0 + DSAddition_i_i_d_0.Int32_1;
		};
		DSMore_Or_Equal_d_i_b_0.Double_0 = DSAddition_i_i_d_0.Double_2();
		DSInteger_i_1.Int32_0 = 15;
		DSMore_Or_Equal_d_i_b_0.Int32_1 = DSInteger_i_1.Int32_0;
		DSMore_Or_Equal_d_i_b_0.Boolean_2 = () =>{
			return Convert.ToDouble(DSMore_Or_Equal_d_i_b_0.Double_0) >= Convert.ToDouble(DSMore_Or_Equal_d_i_b_0.Int32_1);
		};
		DSDialogue_2.Text = $"End dialogue helloPlayer";
		DSDialogue_2.DSDialogueOption = new System.Collections.Generic.List<DialogueSystem.DialogueDisposer.DSDialogueOption>{
			
		};
		DSEqual_s_s_b_0.String_0 = ActorNode_TestActor_0.Actor.tag;
		DSString_s_0.String_0 = "Player";
		DSEqual_s_s_b_0.String_1 = DSString_s_0.String_0;
		DSEqual_s_s_b_0.Boolean_2 = () =>{
			return DSEqual_s_s_b_0.String_0.ToString() == DSEqual_s_s_b_0.String_1.ToString();
		};
		DSDialogue_3.Text = $"End dialogue kek";
		DSDialogue_3.DSDialogueOption = new System.Collections.Generic.List<DialogueSystem.DialogueDisposer.DSDialogueOption>{
			
		};
		DSDialogue_1.DSDialogueOption = new System.Collections.Generic.List<DialogueSystem.DialogueDisposer.DSDialogueOption>{
			new DialogueSystem.DialogueDisposer.DSDialogueOption(text: "Hi!!", nextDialogues: DSDialogue_0, func:  () => DSMore_Or_Equal_d_i_b_0.Boolean_2()),
			new DialogueSystem.DialogueDisposer.DSDialogueOption(text: "HelloPlayer", nextDialogues: DSDialogue_2, func:  () => DSEqual_s_s_b_0.Boolean_2()),
			new DialogueSystem.DialogueDisposer.DSDialogueOption(text: "Kek", nextDialogues: DSDialogue_3)
		};
		DSDialogue_4.Text = $"Start dialogue";
		DSDialogue_4.DSDialogueOption = new System.Collections.Generic.List<DialogueSystem.DialogueDisposer.DSDialogueOption>{
			new DialogueSystem.DialogueDisposer.DSDialogueOption(text: "Hello fellow", nextDialogues: DSDialogue_1)
		};
		
	}
	public DialogueSystem.DialogueDisposer.DSDialogueOption.DSDialogue StartDialogue(DialogueSystem.Assets.TestActor actor){
		Initialize(actor);
		return DSDialogue_4;
		
	}
	#endregion
	#region InnerClasses
	[System.Serializable] private class DSMore_Or_Equal_d_i_b{
		#region Fields
		[SerializeField] public System.Double Double_0;
		[SerializeField] public System.Int32 Int32_1;
		[SerializeField] public Func<System.Boolean> Boolean_2;
		#endregion
		
	}
	[System.Serializable] private class DSAddition_i_i_d{
		#region Fields
		[SerializeField] public System.Int32 Int32_0;
		[SerializeField] public System.Int32 Int32_1;
		[SerializeField] public Func<System.Double> Double_2;
		#endregion
		
	}
	[System.Serializable] private class ActorNode_TestActor{
		#region Fields
		[SerializeField] public DialogueSystem.Assets.TestActor Actor;
		#endregion
		
	}
	[System.Serializable] private class DSInteger_i{
		#region Fields
		[SerializeField] public System.Int32 Int32_0;
		#endregion
		
	}
	[System.Serializable] private class DSEqual_s_s_b{
		#region Fields
		[SerializeField] public System.String String_0;
		[SerializeField] public System.String String_1;
		[SerializeField] public Func<System.Boolean> Boolean_2;
		#endregion
		
	}
	[System.Serializable] private class DSString_s{
		#region Fields
		[SerializeField] public System.String String_0;
		#endregion
		
	}
	#endregion
	
}