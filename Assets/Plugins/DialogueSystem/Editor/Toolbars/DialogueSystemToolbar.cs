using DialogueSystem.Text;
using DialogueSystem.Utilities;
using DialogueSystem.Window;
using System;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace DialogueSystem.Toolbars
{
    internal class DialogueSystemToolbar : BaseToolbar
    {
        private const string TOOLBAR_STYLE_LINK = "Assets/Plugins/DialogueSystem/Resources/Front/DialogueSystemToolbarStyles.uss";
        private TextField textField;
        private Button button;
        private DialogueSystemGraphView graphView;
        public DialogueSystemToolbar(DialogueSystemGraphView graphView) 
        {
            this.graphView = graphView;
            graphView.OnCanSaveGraphEvent += OnCanSaveGraphHandler;
        }

        private void OnCanSaveGraphHandler(bool obj)
        {
            button.SetEnabled(obj);
        }

        public void Initialize(string fileName, string label)
        {
            this.LoadAndAddStyleSheets(TOOLBAR_STYLE_LINK);
            this.AddToClassList("ds-toolbar");

            textField = DialogueSystemUtilities.CreateTextField(fileName, label, callback =>
            {
                TextField target = callback.target as TextField;
                target.value = callback.newValue.RemoveWhitespaces().RemoveSpecialCharacters();
            },
            styles: new string[]
            {
                "ds-textField"
            });
            button = DialogueSystemUtilities.CreateButton("Save", Save, new string[]
            {
                "ds-toolbar__button"
            });
            //button.RegisterCallback<ClickEvent>(Safe);
            this.Add(textField);
            this.Add(button);
        }

        private void Save()
        {
            if (graphView.IsCanSave)
            {
                graphView.Save(textField.value);
            }
            else
            {
                Debug.Log("Dont save");
            }
        }
    }
}
