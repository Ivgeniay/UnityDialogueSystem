using UnityEngine;	
using System;	
using DialogueSystem;	
using DialogueSystem.Nodes;	

public class DialogueFileName
{
	private delegate string Funs(int a, int b);
	[field: SerializeField]private string StringNode49_String_0 {get; set;} = "";
	[field: SerializeField]private int IntegerNode81_Int32_1 {get; set;} = 0;
	[field: SerializeField]private int IntegerNode90_Int32_2 {get; set;} = 0;
	[field: SerializeField]private double FloatNode99_Single_3 {get; set;} = 0f;
	private Func<string> fu;

	private void Init()
	{
		fu = () =>
		{
			return (IntegerNode81_Int32_1 + IntegerNode90_Int32_2).ToString();
        };
	}
	public string CustomMethod(int a, double b)
	{
		 return "пошел нахуй";
    }
	
	
	
}