using static DialogueSystem.DialogueDisposer.DSDialogueOption;	
using System.Globalization;	
using UnityEngine;	
using System;	

[System.Serializable] public class DialogueFileName22 : DialogueSystem.DialogueDisposer{
	#region Fields
	private System.Int32 Int32_0 = 24;
	private DSDialogue DSDialogue_0 = new();
	private DSDialogue DSDialogue_1 = new(){
		Text = @$"Start dialogue",
	};
	#endregion
	#region Methods
	private void Initialize(){
		DSDialogue_0.Text = @$"End {
			Int32_0
		} dialogue";
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
	
}