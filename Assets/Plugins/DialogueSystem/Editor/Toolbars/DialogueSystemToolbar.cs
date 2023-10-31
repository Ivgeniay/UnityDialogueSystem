﻿using DialogueSystem.Text;
using DialogueSystem.Utilities;
using DialogueSystem.Window;
using UnityEngine;
using UnityEngine.UIElements;

namespace DialogueSystem.Toolbars
{
    internal class DialogueSystemToolbar : BaseToolbar
    {
        private const string TOOLBAR_STYLE_LINK = "Assets/Plugins/DialogueSystem/Resources/Front/DialogueSystemToolbarStyles.uss";
        private TextField textField;
        private Button saveButton;
        private Button generateAssetButton;
        private DialogueSystemGraphView graphView;
        public DialogueSystemToolbar(DialogueSystemGraphView graphView) 
        {
            this.graphView = graphView;
            graphView.OnCanSaveGraphEvent += OnCanSaveGraphHandler;
        }

        private void OnCanSaveGraphHandler(bool obj)
        {
            saveButton.SetEnabled(obj);
            generateAssetButton.SetEnabled(obj);
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
            saveButton = DialogueSystemUtilities.CreateButton("Save", Save, new string[]
            {
                "ds-toolbar__button"
            });
            generateAssetButton = DialogueSystemUtilities.CreateButton("Generate Asset", GenerateAsset, new string[]
            {
                "ds-toolbar__button"
            });
            this.Add(textField);
            this.Add(saveButton);
            this.Add(generateAssetButton);
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

        private void GenerateAsset()
        {
            Debug.Log("Asset was generated");
        }
    }
}
