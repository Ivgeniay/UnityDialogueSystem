using static DialogueSystem.DialogueDisposer.DSDialogueOption;	
using System.Globalization;	
using UnityEngine;	
using System;	

[System.Serializable] public class DialogueFileName22 : DialogueSystem.DialogueDisposer{
	#region Fields
	private DSAdd_To_List_Li_Te_Li DSAdd_To_List_Li_Te_Li_0 = new();
	private DSCreate_List_Te_Li DSCreate_List_Te_Li_0 = new();
	private DialogueSystem.Assets.TestActor TestGameObject2;
	#endregion
	#region Methods
	private void Initialize(){
		DSCreate_List_Te_Li_0.ListOfTestActor_1 = () =>{
			System.Collections.Generic.List<T> GetList<T>(params T[] param) => new System.Collections.Generic.List<T>(param);
			return GetList<DialogueSystem.Assets.TestActor>(TestGameObject2);
		};
		DSAdd_To_List_Li_Te_Li_0.ListOfTestActor_2 = () =>{
			System.Collections.Generic.List<T> GetList<T>(params T[] param) => new System.Collections.Generic.List<T>(param);
			var result = DSCreate_List_Te_Li_0.ListOfTestActor_1();
			result.AddRange(GetList<DialogueSystem.Assets.TestActor>(TestGameObject2));
			return result;
		};
		
	}
	public DialogueSystem.DialogueDisposer.DSDialogueOption.DSDialogue StartDialogue(){
		Initialize();
		return null;
		
	}
	#endregion
	#region InnerClasses
	[System.Serializable] private class DSAdd_To_List_Li_Te_Li{
		[SerializeField] public Func<System.Collections.Generic.List<DialogueSystem.Assets.TestActor>> ListOfTestActor_2;
		
	}
	[System.Serializable] private class DSCreate_List_Te_Li{
		#region Fields
		[SerializeField] public Func<System.Collections.Generic.List<DialogueSystem.Assets.TestActor>> ListOfTestActor_1;
		#endregion
		
	}
	#endregion
	
}