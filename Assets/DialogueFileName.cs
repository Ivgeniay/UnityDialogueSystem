using UnityEngine;	
using System;	
using DialogueSystem;	
using DialogueSystem.Nodes;
using UnityEngine.Events;

public class DialogueFileName : MonoBehaviour
{
	[field: SerializeField]private string StringNode49_String_0 {get; set;}
	[field: SerializeField]private DSFloat FloatNode81_Int32_1 {get; set;}
	[field: SerializeField]private DSAddition Addition1 {get; set;}
	[field: SerializeField]private int IntegerNode90_Int32_2 {get; set;}
	[field: SerializeField]private double FloatNode99_Single_3 {get; set;}
	private Func<string> fu;

	private void Initialize()
	{
		FloatNode81_Int32_1 = new()
		{
			FloatNode70_Single_0 = 554f,
        };
		Addition1 = new()
		{
			AdditionNode65_Double_0 = () => { return FloatNode81_Int32_1.FloatNode70_Single_0 + 5; },
        };

		Addition1.AdditionNode65_Double_0();
    }

    [System.Serializable]
    private class DSFloat
    {
		public UnityEvent OnAction;
        [field: SerializeField] public float FloatNode70_Single_0 { get; set; } = 565f;

    }

	[System.Serializable]
	private class DSAddition
	{
		[field: SerializeField] public Func<float> AdditionNode65_Double_0 { get; set; }

	}

}