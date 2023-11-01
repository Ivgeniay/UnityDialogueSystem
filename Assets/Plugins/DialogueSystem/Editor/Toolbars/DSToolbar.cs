using DialogueSystem.Text;
using DialogueSystem.Utilities;
using DialogueSystem.Window;
using UnityEngine;
using UnityEngine.UIElements;

namespace DialogueSystem.Toolbars
{
    internal class DSToolbar : BaseToolbar
    {
        private const string TOOLBAR_STYLE_LINK = "Assets/Plugins/DialogueSystem/Resources/Front/DialogueSystemToolbarStyles.uss";
        private TextField textField;
        private Button saveButton;
        private Button cleanButton;
        private Button generateAssetButton;
        private DSGraphView graphView;
        public DSToolbar(DSGraphView graphView) 
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

            textField = DSUtilities.CreateTextField(fileName, label, callback =>
            {
                TextField target = callback.target as TextField;
                target.value = callback.newValue.RemoveWhitespaces().RemoveSpecialCharacters();
            },
            styles: new string[]
            {
                "ds-textField"
            });
            saveButton = DSUtilities.CreateButton("Save", Save, new string[]
            {
                "ds-toolbar__button"
            });
            cleanButton = DSUtilities.CreateButton("Clean Graph", CleanGraph, new string[]
            {
                "ds-toolbar__button"
            });
            generateAssetButton = DSUtilities.CreateButton("Generate Asset", GenerateAsset, new string[]
            {
                "ds-toolbar__button"
            });
            this.Add(textField);
            this.Add(saveButton);
            this.Add(cleanButton);
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
        private void CleanGraph()
        {
            graphView.CleanGraph();
        }

        private void GenerateAsset()
        {
            Debug.Log("Asset was generated");
        }
    }
}
