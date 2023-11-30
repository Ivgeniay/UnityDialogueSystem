using UnityEngine;	
using System;	
using static DialogueSystem.DialogueDisposer.DSDialogueOption;	
using System.Globalization;	

public class DialogueFileNametest : DialogueSystem.DialogueDisposer{
	#region Fields
	 private DSAddition_i_f_d DSAddition_i_f_d_0 = new();
	 private System.Int32 Int32_0 = 0;
	 private System.Single Single_0 = 2f;
	 private DSDialogue DSDialogue_0 = new();
	 private System.String String_0 = "YOYOY";
	 private DSDialogue DSDialogue_1 = new();
	 private DSTo_Double_i_d DSTo_Double_i_d_0 = new();
	 private System.Int32 Int32_1 = 22;
	 private DSTo_Bool_i_b DSTo_Bool_i_b_0 = new();
	 private System.Int32 Int32_2 = 1;
	 private DSTo_Float_i_f DSTo_Float_i_f_0 = new();
	 private System.Int32 Int32_3 = 4;
	 private DSTo_String_i_s DSTo_String_i_s_0 = new();
	 private System.Int32 Int32_4 = 15;
	 private DSDivide_f_i_d DSDivide_f_i_d_0 = new();
	 private System.Single Single_1 = 23f;
	 private System.Int32 Int32_5 = 33;
	 private DSMultiply_d_i_d DSMultiply_d_i_d_0 = new();
	 private System.Double Double_0 = 0.2323d;
	 private System.Int32 Int32_6 = 10;
	 private DSSubtract_f_i_d DSSubtract_f_i_d_0 = new();
	 private System.Single Single_2 = 2000f;
	 private System.Int32 Int32_7 = 4;
	 private DSTo_Double_d_d DSTo_Double_d_d_0 = new();
	 private System.Double Double_1 = 22d;
	 private DSTo_Double_f_d DSTo_Double_f_d_0 = new();
	 private System.Single Single_3 = 22f;
	 private DSTo_Double_s_d DSTo_Double_s_d_0 = new();
	 private System.String String_1 = "22.54745";
	 private DSTo_Bool_d_b DSTo_Bool_d_b_0 = new();
	 private System.Double Double_2 = 1d;
	 private DSTo_Bool_s_b DSTo_Bool_s_b_0 = new();
	 private System.String String_2 = "true";
	 private DSTo_Bool_f_b DSTo_Bool_f_b_0 = new();
	 private System.Single Single_4 = 1f;
	 private DSTo_Float_s_f DSTo_Float_s_f_0 = new();
	 private System.String String_3 = "4";
	 private DSTo_Float_f_f DSTo_Float_f_f_0 = new();
	 private System.Single Single_5 = 4f;
	 private DSTo_Float_d_f DSTo_Float_d_f_0 = new();
	 private System.Double Double_3 = 4d;
	 private DSTo_String_s_s DSTo_String_s_s_0 = new();
	 private System.String String_4 = "sdafasdf#342";
	 private DSTo_String_d_s DSTo_String_d_s_0 = new();
	 private System.Double Double_4 = 15d;
	 private DSTo_String_f_s DSTo_String_f_s_0 = new();
	 private System.Single Single_6 = 15f;
	 private DSTo_String_b_s DSTo_String_b_s_0 = new();
	 private System.Boolean Boolean_0 = false;
	 private DSTo_Float_b_f DSTo_Float_b_f_0 = new();
	 private System.Boolean Boolean_1 = false;
	 private DSTo_Bool_b_b DSTo_Bool_b_b_0 = new();
	 private System.Boolean Boolean_2 = false;
	 private DSMore_Or_Equal_i_f_b DSMore_Or_Equal_i_f_b_0 = new();
	 private System.Int32 Int32_8 = 0;
	 private System.Single Single_7 = 0f;
	 private DSMore_Or_Equal_i_d_b DSMore_Or_Equal_i_d_b_0 = new();
	 private System.Int32 Int32_9 = 0;
	 private System.Double Double_5 = 0d;
	 private DSMore_Or_Equal_i_i_b DSMore_Or_Equal_i_i_b_0 = new();
	 private System.Int32 Int32_10 = 0;
	 private System.Int32 Int32_11 = 0;
	 private DSDialogue DSDialogue_2 = new();
	 private DSDialogue DSDialogue_3 = new();
	 private DSEqual_i_i_b DSEqual_i_i_b_0 = new();
	 private DSLess_d_d_b DSLess_d_d_b_0 = new();
	 private DSLess_Or_Equal_d_d_b DSLess_Or_Equal_d_d_b_0 = new();
	 private DSMore_d_d_b DSMore_d_d_b_0 = new();
	 private System.Single Single_8 = 222f;
	#endregion
	#region Methods
	private void Initialize(){
		DSAddition_i_f_d_0.Int32_0 = Int32_0;
		DSAddition_i_f_d_0.Single_1 = Single_0;
		DSAddition_i_f_d_0.Double_2 = () =>{
			return DSAddition_i_f_d_0.Int32_0 + DSAddition_i_f_d_0.Single_1;
		};
		DSDialogue_0.Text = @$"Start {
			String_0
		} dialogue";
		DSTo_Double_i_d_0.Int32_0 = Int32_1;
		DSTo_Double_i_d_0.Double_1 = () =>{
			return (double)DSTo_Double_i_d_0.Int32_0;
		};
		DSTo_Bool_i_b_0.Int32_0 = Int32_2;
		DSTo_Bool_i_b_0.Boolean_1 = () =>{
			return Convert.ToBoolean(DSTo_Bool_i_b_0.Int32_0);
		};
		DSTo_Float_i_f_0.Int32_0 = Int32_3;
		DSTo_Float_i_f_0.Single_1 = () =>{
			return (float)DSTo_Float_i_f_0.Int32_0;
		};
		DSTo_String_i_s_0.Int32_0 = Int32_4;
		DSTo_String_i_s_0.String_1 = () =>{
			return DSTo_String_i_s_0.Int32_0.ToString();
		};
		DSDivide_f_i_d_0.Single_0 = Single_1;
		DSDivide_f_i_d_0.Int32_1 = Int32_5;
		DSDivide_f_i_d_0.Double_2 = () =>{
			return DSDivide_f_i_d_0.Single_0 / DSDivide_f_i_d_0.Int32_1;
		};
		DSMultiply_d_i_d_0.Double_0 = Double_0;
		DSMultiply_d_i_d_0.Int32_1 = Int32_6;
		DSMultiply_d_i_d_0.Double_2 = () =>{
			return DSMultiply_d_i_d_0.Double_0 * DSMultiply_d_i_d_0.Int32_1;
		};
		DSSubtract_f_i_d_0.Single_0 = Single_2;
		DSSubtract_f_i_d_0.Int32_1 = Int32_7;
		DSSubtract_f_i_d_0.Double_2 = () =>{
			return DSSubtract_f_i_d_0.Single_0 - DSSubtract_f_i_d_0.Int32_1;
		};
		DSTo_Double_d_d_0.Double_0 = Double_1;
		DSTo_Double_d_d_0.Double_1 = () =>{
			return DSTo_Double_d_d_0.Double_0;
		};
		DSTo_Double_f_d_0.Single_0 = Single_3;
		DSTo_Double_f_d_0.Double_1 = () =>{
			return (double)DSTo_Double_f_d_0.Single_0;
		};
		DSTo_Double_s_d_0.String_0 = String_1;
		DSTo_Double_s_d_0.Double_1 = () =>{
			double SafeParseToDouble(string input)
			{
				
				double result;
				if (double.TryParse(input, out result)) return result;
				return 0;
				
			}
			return SafeParseToDouble(DSTo_Double_s_d_0.String_0);
		};
		DSTo_Bool_d_b_0.Double_0 = Double_2;
		DSTo_Bool_d_b_0.Boolean_1 = () =>{
			return Convert.ToBoolean(DSTo_Bool_d_b_0.Double_0);
		};
		DSTo_Bool_s_b_0.String_0 = String_2;
		DSTo_Bool_s_b_0.Boolean_1 = () =>{
			return DSTo_Bool_s_b_0.String_0 == "true";
		};
		DSTo_Bool_f_b_0.Single_0 = Single_4;
		DSTo_Bool_f_b_0.Boolean_1 = () =>{
			return Convert.ToBoolean(DSTo_Bool_f_b_0.Single_0);
		};
		DSTo_Float_s_f_0.String_0 = String_3;
		DSTo_Float_s_f_0.Single_1 = () =>{
			float SafeParseToFloat(string input)
			{
				
				input = input.Replace(',', '.');
				CultureInfo culture = CultureInfo.InvariantCulture;
				NumberStyles style = NumberStyles.Float;
				if (float.TryParse(input, style, culture, out float result)) return result;
				else return 0f;
				
			}
			return SafeParseToFloat(DSTo_Float_s_f_0.String_0);
		};
		DSTo_Float_f_f_0.Single_0 = Single_5;
		DSTo_Float_f_f_0.Single_1 = () =>{
			return DSTo_Float_f_f_0.Single_0;
		};
		DSTo_Float_d_f_0.Double_0 = Double_3;
		DSTo_Float_d_f_0.Single_1 = () =>{
			return (float)DSTo_Float_d_f_0.Double_0;
		};
		DSTo_String_s_s_0.String_0 = String_4;
		DSTo_String_s_s_0.String_1 = () =>{
			return DSTo_String_s_s_0.String_0.ToString();
		};
		DSTo_String_d_s_0.Double_0 = Double_4;
		DSTo_String_d_s_0.String_1 = () =>{
			return DSTo_String_d_s_0.Double_0.ToString();
		};
		DSTo_String_f_s_0.Single_0 = Single_6;
		DSTo_String_f_s_0.String_1 = () =>{
			return DSTo_String_f_s_0.Single_0.ToString();
		};
		DSTo_String_b_s_0.Boolean_0 = Boolean_0;
		DSTo_String_b_s_0.String_1 = () =>{
			return DSTo_String_b_s_0.Boolean_0.ToString();
		};
		DSTo_Float_b_f_0.Boolean_0 = Boolean_1;
		DSTo_Float_b_f_0.Single_1 = () =>{
			return DSTo_Float_b_f_0.Boolean_0 ? 1f : 0f;
		};
		DSTo_Bool_b_b_0.Boolean_0 = Boolean_2;
		DSTo_Bool_b_b_0.Boolean_1 = () =>{
			return DSTo_Bool_b_b_0.Boolean_0;
		};
		DSMore_Or_Equal_i_f_b_0.Int32_0 = Int32_8;
		DSMore_Or_Equal_i_f_b_0.Single_1 = Single_7;
		DSMore_Or_Equal_i_f_b_0.Boolean_2 = () =>{
			return Convert.ToDouble(DSMore_Or_Equal_i_f_b_0.Int32_0) >= Convert.ToDouble(DSMore_Or_Equal_i_f_b_0.Single_1);
		};
		DSMore_Or_Equal_i_d_b_0.Int32_0 = Int32_9;
		DSMore_Or_Equal_i_d_b_0.Double_1 = Double_5;
		DSMore_Or_Equal_i_d_b_0.Boolean_2 = () =>{
			return Convert.ToDouble(DSMore_Or_Equal_i_d_b_0.Int32_0) >= Convert.ToDouble(DSMore_Or_Equal_i_d_b_0.Double_1);
		};
		DSMore_Or_Equal_i_i_b_0.Int32_0 = Int32_10;
		DSMore_Or_Equal_i_i_b_0.Int32_1 = Int32_11;
		DSMore_Or_Equal_i_i_b_0.Boolean_2 = () =>{
			return Convert.ToDouble(DSMore_Or_Equal_i_i_b_0.Int32_0) >= Convert.ToDouble(DSMore_Or_Equal_i_i_b_0.Int32_1);
		};
		DSDialogue_1.Text = @$"SUM INT FLOAT: {
			DSAddition_i_f_d_0.Double_2()
		}
		DIV FLOAT INTEGER {
			DSDivide_f_i_d_0.Double_2()
		}
		MULT DOUBLE INT {
			DSMultiply_d_i_d_0.Double_2()
		}
		SUBS FLOAT INT {
			DSSubtract_f_i_d_0.Double_2()
		}
		
		INT TO DOUBL {
			DSTo_Double_i_d_0.Double_1()
		}
		DOUBL TO DOUBLE {
			DSTo_Double_d_d_0.Double_1()
		}
		FLOAT TO DOUBLE {
			DSTo_Double_f_d_0.Double_1()
		}
		STRING TO DOUBLE {
			DSTo_Double_s_d_0.Double_1()
		}
		
		Int ToBool {
			DSTo_Bool_i_b_0.Boolean_1()
		}
		Double ToBool {
			DSTo_Bool_d_b_0.Boolean_1()
		}
		Float ToBool {
			DSTo_Bool_f_b_0.Boolean_1()
		}
		String ToBool {
			DSTo_Bool_s_b_0.Boolean_1()
		}
		Bool ToBool {
			DSTo_Bool_b_b_0.Boolean_1()
		}
		
		Int ToFloat {
			DSTo_Float_i_f_0.Single_1()
		}
		Double ToFloat {
			DSTo_Float_d_f_0.Single_1()
		}
		Float ToFloat {
			DSTo_Float_f_f_0.Single_1()
		}
		String ToFloat {
			DSTo_Float_s_f_0.Single_1()
		}
		Bool ToFloat {
			DSTo_Float_b_f_0.Single_1()
		}
		
		Int ToString {
			DSTo_String_i_s_0.String_1()
		}
		Float ToString {
			DSTo_String_f_s_0.String_1()
		}
		Double ToString {
			DSTo_String_d_s_0.String_1()
		}
		String ToString {
			DSTo_String_s_s_0.String_1()
		}
		Bool ToString {
			DSTo_String_b_s_0.String_1()
		}
		
		MOE INTFLOAT {
			DSMore_Or_Equal_i_f_b_0.Boolean_2()
		}
		MOE INTDOUBLE {
			DSMore_Or_Equal_i_d_b_0.Boolean_2()
		}
		MOE INTINT {
			DSMore_Or_Equal_i_i_b_0.Boolean_2()
		}";
		DSDialogue_2.Text = @$" End dialogue {
			DSAddition_i_f_d_0.Double_2()
		}";
		DSDialogue_2.DSDialogueOption = new System.Collections.Generic.List<DialogueSystem.DialogueDisposer.DSDialogueOption>{
			
		};
		DSDialogue_3.Text = @$"End dialogue 222";
		DSDialogue_3.DSDialogueOption = new System.Collections.Generic.List<DialogueSystem.DialogueDisposer.DSDialogueOption>{
			
		};
		DSDialogue_1.DSDialogueOption = new System.Collections.Generic.List<DialogueSystem.DialogueDisposer.DSDialogueOption>{
			new DialogueSystem.DialogueDisposer.DSDialogueOption(text: "Next Choice", nextDialogues: DSDialogue_2),
			new DialogueSystem.DialogueDisposer.DSDialogueOption(text: "Choice", nextDialogues: DSDialogue_3)
		};
		DSDialogue_0.DSDialogueOption = new System.Collections.Generic.List<DialogueSystem.DialogueDisposer.DSDialogueOption>{
			new DialogueSystem.DialogueDisposer.DSDialogueOption(text: "Hello", nextDialogues: DSDialogue_1)
		};
		DSEqual_i_i_b_0.All_0 = 0;
		DSEqual_i_i_b_0.All_1 = 0;
		DSEqual_i_i_b_0.Boolean_2 = () =>{
			return DSEqual_i_i_b_0.All_0.ToString() == DSEqual_i_i_b_0.All_1.ToString();
		};
		DSLess_d_d_b_0.Number_0 = 0d;
		DSLess_d_d_b_0.Number_1 = 0d;
		DSLess_d_d_b_0.Boolean_2 = () =>{
			return Convert.ToDouble(DSLess_d_d_b_0.Number_0) < Convert.ToDouble(DSLess_d_d_b_0.Number_1);
		};
		DSLess_Or_Equal_d_d_b_0.Number_0 = 0d;
		DSLess_Or_Equal_d_d_b_0.Number_1 = 0d;
		DSLess_Or_Equal_d_d_b_0.Boolean_2 = () =>{
			return Convert.ToDouble(DSLess_Or_Equal_d_d_b_0.Number_0) <= Convert.ToDouble(DSLess_Or_Equal_d_d_b_0.Number_1);
		};
		DSMore_d_d_b_0.Number_0 = 0d;
		DSMore_d_d_b_0.Number_1 = 0d;
		DSMore_d_d_b_0.Boolean_2 = () =>{
			return Convert.ToDouble(DSMore_d_d_b_0.Number_0) > Convert.ToDouble(DSMore_d_d_b_0.Number_1);
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
		 public System.Int32 Int32_0;
		 public System.Single Single_1;
		 public Func<System.Double> Double_2;
		#endregion
		
	}
	[System.Serializable] private class DSTo_Double_i_d{
		#region Fields
		 public System.Int32 Int32_0;
		 public Func<System.Double> Double_1;
		#endregion
		
	}
	[System.Serializable] private class DSTo_Bool_i_b{
		#region Fields
		 public System.Int32 Int32_0;
		 public Func<System.Boolean> Boolean_1;
		#endregion
		
	}
	[System.Serializable] private class DSTo_Float_i_f{
		#region Fields
		 public System.Int32 Int32_0;
		 public Func<System.Single> Single_1;
		#endregion
		
	}
	[System.Serializable] private class DSTo_String_i_s{
		#region Fields
		 public System.Int32 Int32_0;
		 public Func<System.String> String_1;
		#endregion
		
	}
	[System.Serializable] private class DSDivide_f_i_d{
		#region Fields
		 public System.Single Single_0;
		 public System.Int32 Int32_1;
		 public Func<System.Double> Double_2;
		#endregion
		
	}
	[System.Serializable] private class DSMultiply_d_i_d{
		#region Fields
		 public System.Double Double_0;
		 public System.Int32 Int32_1;
		 public Func<System.Double> Double_2;
		#endregion
		
	}
	[System.Serializable] private class DSSubtract_f_i_d{
		#region Fields
		 public System.Single Single_0;
		 public System.Int32 Int32_1;
		 public Func<System.Double> Double_2;
		#endregion
		
	}
	[System.Serializable] private class DSTo_Double_d_d{
		#region Fields
		 public System.Double Double_0;
		 public Func<System.Double> Double_1;
		#endregion
		
	}
	[System.Serializable] private class DSTo_Double_f_d{
		#region Fields
		 public System.Single Single_0;
		 public Func<System.Double> Double_1;
		#endregion
		
	}
	[System.Serializable] private class DSTo_Double_s_d{
		#region Fields
		 public System.String String_0;
		 public Func<System.Double> Double_1;
		#endregion
		
	}
	[System.Serializable] private class DSTo_Bool_d_b{
		#region Fields
		 public System.Double Double_0;
		 public Func<System.Boolean> Boolean_1;
		#endregion
		
	}
	[System.Serializable] private class DSTo_Bool_s_b{
		#region Fields
		 public System.String String_0;
		 public Func<System.Boolean> Boolean_1;
		#endregion
		
	}
	[System.Serializable] private class DSTo_Bool_f_b{
		#region Fields
		 public System.Single Single_0;
		 public Func<System.Boolean> Boolean_1;
		#endregion
		
	}
	[System.Serializable] private class DSTo_Float_s_f{
		#region Fields
		 public System.String String_0;
		 public Func<System.Single> Single_1;
		#endregion
		
	}
	[System.Serializable] private class DSTo_Float_f_f{
		#region Fields
		 public System.Single Single_0;
		 public Func<System.Single> Single_1;
		#endregion
		
	}
	[System.Serializable] private class DSTo_Float_d_f{
		#region Fields
		 public System.Double Double_0;
		 public Func<System.Single> Single_1;
		#endregion
		
	}
	[System.Serializable] private class DSTo_String_s_s{
		#region Fields
		 public System.String String_0;
		 public Func<System.String> String_1;
		#endregion
		
	}
	[System.Serializable] private class DSTo_String_d_s{
		#region Fields
		 public System.Double Double_0;
		 public Func<System.String> String_1;
		#endregion
		
	}
	[System.Serializable] private class DSTo_String_f_s{
		#region Fields
		 public System.Single Single_0;
		 public Func<System.String> String_1;
		#endregion
		
	}
	[System.Serializable] private class DSTo_String_b_s{
		#region Fields
		 public System.Boolean Boolean_0;
		 public Func<System.String> String_1;
		#endregion
		
	}
	[System.Serializable] private class DSTo_Float_b_f{
		#region Fields
		 public System.Boolean Boolean_0;
		 public Func<System.Single> Single_1;
		#endregion
		
	}
	[System.Serializable] private class DSTo_Bool_b_b{
		#region Fields
		 public System.Boolean Boolean_0;
		 public Func<System.Boolean> Boolean_1;
		#endregion
		
	}
	[System.Serializable] private class DSMore_Or_Equal_i_f_b{
		#region Fields
		 public System.Int32 Int32_0;
		 public System.Single Single_1;
		 public Func<System.Boolean> Boolean_2;
		#endregion
		
	}
	[System.Serializable] private class DSMore_Or_Equal_i_d_b{
		#region Fields
		 public System.Int32 Int32_0;
		 public System.Double Double_1;
		 public Func<System.Boolean> Boolean_2;
		#endregion
		
	}
	[System.Serializable] private class DSMore_Or_Equal_i_i_b{
		#region Fields
		 public System.Int32 Int32_0;
		 public System.Int32 Int32_1;
		 public Func<System.Boolean> Boolean_2;
		#endregion
		
	}
	[System.Serializable] private class DSEqual_i_i_b{
		#region Fields
		 public System.Int32 All_0;
		 public System.Int32 All_1;
		 public Func<System.Boolean> Boolean_2;
		#endregion
		
	}
	[System.Serializable] private class DSLess_d_d_b{
		#region Fields
		 public System.Double Number_0;
		 public System.Double Number_1;
		 public Func<System.Boolean> Boolean_2;
		#endregion
		
	}
	[System.Serializable] private class DSLess_Or_Equal_d_d_b{
		#region Fields
		 public System.Double Number_0;
		 public System.Double Number_1;
		 public Func<System.Boolean> Boolean_2;
		#endregion
		
	}
	[System.Serializable] private class DSMore_d_d_b{
		#region Fields
		 public System.Double Number_0;
		 public System.Double Number_1;
		 public Func<System.Boolean> Boolean_2;
		#endregion
		
	}
	#endregion
	
}