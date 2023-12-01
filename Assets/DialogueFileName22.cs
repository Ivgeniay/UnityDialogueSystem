using static DialogueSystem.DialogueDisposer.DSDialogueOption;	
using System.Globalization;	
using UnityEngine;	
using System;
using DialogueSystem.Assets;

[System.Serializable] public class DialogueFileName22 : DialogueSystem.DialogueDisposer{
	#region Fields 
	private System.Int32 Int32_0 = 3;
	private System.Int32 Int32_1 = 2;
	private System.Int32 Int32_2 = 1;
	private DSCreate_List_i_i_i_i_Li DSCreate_List_i_i_i_i_Li_0 = new();
	private ActorNode_TestActor ActorNode_TestActor_0 = new();
	private DSDialogue DSDialogue_0 = new(){
		Text = @$"End dialogue",
	};
	private DSDialogue DSDialogue_1 = new(){
		Text = @$"Start dialogue",
	};
	#endregion
	#region Methods
	private void Initialize(DialogueSystem.Assets.TestActor actor){
		ActorNode_TestActor_0.Actor = actor;
		DSCreate_List_i_i_i_i_Li_0.ListOfInt32_4 = () =>{
			System.Collections.Generic.List<T> GetList<T>(params T[] param) => new System.Collections.Generic.List<T>(param);
			return GetList<System.Int32>(Int32_2, Int32_0, Int32_1, ActorNode_TestActor_0.Actor.Age);
		};
		DSDialogue_0.DSDialogueOption = new System.Collections.Generic.List<DialogueSystem.DialogueDisposer.DSDialogueOption>{
			
		};
		DSDialogue_1.DSDialogueOption = new System.Collections.Generic.List<DialogueSystem.DialogueDisposer.DSDialogueOption>{
			new DialogueSystem.DialogueDisposer.DSDialogueOption(text: "Hello", nextDialogues: DSDialogue_0)
		};
		
	}
	public DialogueSystem.DialogueDisposer.DSDialogueOption.DSDialogue StartDialogue(DialogueSystem.Assets.TestActor actor){
		Initialize(actor);
		return DSDialogue_1;
		
	}
	#endregion
	#region InnerClasses
	[System.Serializable] private class DSCreate_List_i_i_i_i_Li{
		#region Fields
		[SerializeField] public Func<System.Collections.Generic.List<System.Int32>> ListOfInt32_4;
		#endregion
		
	}
	[System.Serializable] private class ActorNode_TestActor{
		#region Fields
		[SerializeField] public DialogueSystem.Assets.TestActor Actor;
		#endregion
		
	}
	#endregion
	
}