using static DialogueSystem.DialogueDisposer.DSDialogueOption;	
using System.Globalization;	
using UnityEngine;	
using System;	

[System.Serializable] public class DialogueFileName22 : DialogueSystem.DialogueDisposer{
	#region Fields
	private DSCreate_List_Te_Te_Te_Te_Li DSCreate_List_Te_Te_Te_Te_Li_0 = new();
	private DSDialogue DSDialogue_0 = new(){
		Text = @$"End dialogue",
	};
	private DSDialogue DSDialogue_1 = new(){
		Text = @$"Start dialogue",
	};
	#endregion
	#region Methods
	private void Initialize(){
		DSCreate_List_Te_Te_Te_Te_Li_0.ListOfTestActor_4 = () =>{
			System.Collections.Generic.List<T> GetList<T>(params T[] param) => new System.Collections.Generic.List<T>(param);
			return GetList<DialogueSystem.Assets.TestActor>(null, null, null, null);
		};
		DSDialogue_0.DSDialogueOption = new System.Collections.Generic.List<DialogueSystem.DialogueDisposer.DSDialogueOption>{
			
		};
		DSDialogue_1.DSDialogueOption = new System.Collections.Generic.List<DialogueSystem.DialogueDisposer.DSDialogueOption>{
			new DialogueSystem.DialogueDisposer.DSDialogueOption(text: "Hello", nextDialogues: DSDialogue_0)
		};
		
	}
	public DialogueSystem.DialogueDisposer.DSDialogueOption.DSDialogue StartDialogue(){
		Initialize();
		return DSDialogue_1;
		
	}
	#endregion
	#region InnerClasses
	[System.Serializable] private class DSCreate_List_Te_Te_Te_Te_Li{
		#region Fields
		[SerializeField] public Func<System.Collections.Generic.List<DialogueSystem.Assets.TestActor>> ListOfTestActor_4;
		#endregion
		
	}
	#endregion
	
}