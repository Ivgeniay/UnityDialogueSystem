using static DialogueSystem.DialogueDisposer.DSDialogueOption;	
using System.Globalization;	
using UnityEngine;	
using System;	

[System.Serializable] public class DialogueFileName22 : DialogueSystem.DialogueDisposer{
	#region Fields
	[SerializeField] private DSCreate_List_Te_Te_Te_Li DSCreate_List_Te_Te_Te_Li_0 = new();
    [SerializeField] private DialogueSystem.Assets.TestActor TestGameObject2;
    [SerializeField]
    private DSDialogue DSDialogue_0 = new(){
		Text = @$"End dialogue",
	};
    [SerializeField]
    private DSDialogue DSDialogue_1 = new(){
		Text = @$"Start dialogue",
	};
	#endregion
	#region Methods
	private void Initialize(){
		DSCreate_List_Te_Te_Te_Li_0.ListOfTestActor_3 = () =>{
			System.Collections.Generic.List<T> GetList<T>(params T[] param) => new System.Collections.Generic.List<T>(param);
			return GetList<DialogueSystem.Assets.TestActor>(TestGameObject2, null, TestGameObject2);
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
	[System.Serializable] private class DSCreate_List_Te_Te_Te_Li{
		#region Fields
		[SerializeField] public Func<System.Collections.Generic.List<DialogueSystem.Assets.TestActor>> ListOfTestActor_3;
		#endregion
		
	}
	#endregion
	
}