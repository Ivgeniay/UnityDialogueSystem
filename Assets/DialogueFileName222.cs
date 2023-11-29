using UnityEngine;	
using System;	
using static DialogueSystem.DialogueDisposer.DSDialogueOption;	

public class DialogueFileName222 : DialogueSystem.DialogueDisposer{
	#region Fields
	[SerializeField] private DSDialogue DSDialogue_0 = new();
	[SerializeField] private DSDialogue DSDialogue_1 = new();
	[SerializeField] private DSAddition_i_f_d DSAddition_i_f_d_0 = new();
	[SerializeField] private DSInteger_i DSInteger_i_0 = new();
	[SerializeField] private DSFloat_f DSFloat_f_0 = new();
	#endregion
	#region Methods
	private void Initialize(){
		DSDialogue_0.Text = @$"Start dialogue";
		DSInteger_i_0.Int32_0 = 22;
		DSAddition_i_f_d_0.Int32_0 = DSInteger_i_0.Int32_0;
		DSFloat_f_0.Single_0 = 1.23f;
		DSAddition_i_f_d_0.Single_1 = DSFloat_f_0.Single_0;
		DSAddition_i_f_d_0.Double_2 = () =>{
			return DSAddition_i_f_d_0.Int32_0 + DSAddition_i_f_d_0.Single_1;
		};
		DSDialogue_1.Text = @$"End {
			DSAddition_i_f_d_0.Double_2()
		} dialogue";
		DSDialogue_1.DSDialogueOption = new System.Collections.Generic.List<DialogueSystem.DialogueDisposer.DSDialogueOption>{
			
		};
		DSDialogue_0.DSDialogueOption = new System.Collections.Generic.List<DialogueSystem.DialogueDisposer.DSDialogueOption>{
			new DialogueSystem.DialogueDisposer.DSDialogueOption(text: "Hello", nextDialogues: DSDialogue_1)
		};
		
	}
	public DialogueSystem.DialogueDisposer.DSDialogueOption.DSDialogue StartDialogue(){
		Initialize();
		return DSDialogue_0;
		
	}
	#endregion
	#region InnerClasses
	[System.Serializable] private class DSAddition_i_f_d{
		#region Fields
		[SerializeField] public System.Int32 Int32_0;
		[SerializeField] public System.Single Single_1;
		[SerializeField] public Func<System.Double> Double_2;
		#endregion
		
	}
	[System.Serializable] private class DSInteger_i{
		#region Fields
		[SerializeField] public System.Int32 Int32_0;
		#endregion
		
	}
	[System.Serializable] private class DSFloat_f{
		#region Fields
		[SerializeField] public System.Single Single_0;
		#endregion
		
	}
	#endregion
	
}