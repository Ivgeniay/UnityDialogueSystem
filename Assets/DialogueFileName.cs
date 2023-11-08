using UnityEngine;	
using System;	
using DialogueSystem;

public class DialogueFileName
{
	
	#region Properties
	[field: SerializeField]private DSInteger IntegerNode_0 {get; set;}
	[field: SerializeField]private DSFloat FloatNode_1 {get; set;}
	[field: SerializeField]private DSSubtract SubtractNode_2 {get; set;}
	#endregion
	#region Methods
	private void Initialize()
	{
		 IntegerNode_0 =new()
		{
			IntegerNode99_Int32_0= 2323,
		};
		FloatNode_1 =new()
		{
			FloatNode70_Single_0= 2f,
		};
		SubtractNode_2 =new()
		{
			SubtractNode80_Double_0= ()=>{
				return IntegerNode_0.IntegerNode99_Int32_0 - FloatNode_1.FloatNode70_Single_0;
			},
		};
		
	}
	#endregion
	#region ClassDeclaration
	
	[System.Serializable]
	private class DSInteger
	{
		[field: SerializeField]public int IntegerNode99_Int32_0 {get; set;}
		
	}
	
	[System.Serializable]
	private class DSFloat
	{
		[field: SerializeField]public float FloatNode70_Single_0 {get; set;}
		
	}
	
	[System.Serializable]
	private class DSSubtract
	{
		[field: SerializeField]public Func<Double> SubtractNode80_Double_0 {get; set;}
		
	}
	#endregion
	
	
}