﻿using DialogueSystem.Text;
using DialogueSystem.Utilities;
using DialogueSystem.Window;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace DialogueSystem.Toolbars
{
    internal class DSToolbar : BaseToolbar
    {
        private const string TOOLBAR_STYLE_LINK = "Assets/Plugins/DialogueSystem/Resources/Front/DialogueSystemToolbarStyles.uss";
        private ProgressBar progressBar;
        private TextField textField;
        private Button saveButton;
        private Button loadButton;
        private Button cleanButton;
        private Button generateScriptButton;
        private Button minimapButton;
        private DSGraphView graphView;
        public DSToolbar(DSGraphView graphView) 
        {
            this.graphView = graphView;
            graphView.OnCanSaveGraphEvent += OnCanSaveGraphHandler;
            graphView.OnSaveEvent += OnSaveHandler;
        }


        private void OnCanSaveGraphHandler(bool obj)
        {
            saveButton.SetEnabled(obj);
            generateScriptButton.SetEnabled(obj);
        }

        public void Initialize(string fileName, string label)
        {
            this.LoadAndAddStyleSheets(TOOLBAR_STYLE_LINK);
            this.AddToClassList("ds-toolbar");

            progressBar = DSUtilities.CreateProgressBar(0, 0, 1, "SAVING", callBack =>
            {
                ProgressBar progressBar = callBack.target as ProgressBar;
                progressBar.value = callBack.newValue;

                if (progressBar.value == 1f)
                    progressBar.style.display = DisplayStyle.None;
                else
                    progressBar.style.display = DisplayStyle.Flex;
                
                MarkDirtyRepaint();
            }, styles: new string[]
            {
                "ds-progressBar"
            });
            progressBar.style.display = DisplayStyle.None;

            textField = DSUtilities.CreateTextField(fileName, label, callback =>
            {
                TextField target = callback.target as TextField;
                target.value = callback.newValue.RemoveWhitespaces().RemoveSpecialCharacters();
                saveButton.SetEnabled(!string.IsNullOrWhiteSpace(callback.newValue));
            },
            styles: new string[]
            {
                "ds-textField"
            });
            saveButton = DSUtilities.CreateButton("Save", Save, new string[]
            {
                "ds-toolbar__button"
            });
            loadButton = DSUtilities.CreateButton("Load", Load, new string[]
            {
                "ds-toolbar__button"
            });
            cleanButton = DSUtilities.CreateButton("Clean Graph", CleanGraph, new string[]
            {
                "ds-toolbar__button"
            });
            generateScriptButton = DSUtilities.CreateButton("Generate DialogueScript", GenerateScript, new string[]
            {
                "ds-toolbar__button"
            });
            minimapButton = DSUtilities.CreateButton("Minimap", MinimapToggle, new string[]
            {
                "ds-toolbar__button"
            });

            this.Add(progressBar);
            this.Add(textField);
            this.Add(saveButton);
            this.Add(loadButton);
            this.Add(cleanButton);
            this.Add(minimapButton);
            this.Add(generateScriptButton);

            saveButton.SetEnabled(false);
            generateScriptButton.SetEnabled(false);
        }

        private void MinimapToggle() => graphView.MiniMap.visible = !graphView.MiniMap.visible;
        private void Save()
        {
            string path = EditorUtility.SaveFilePanel("Select a graph file", Application.dataPath, textField.value, "asset");
            textField.value = graphView.Save(path);
        }
        private void Load()
        {
            string path = EditorUtility.OpenFilePanel("Select a graph file", Application.dataPath, "asset");
            textField.value = graphView.Load(path);
        }
        private void CleanGraph() => graphView.CleanGraph();
        private void GenerateScript()
        {
            string path = EditorUtility.SaveFilePanel("Select a graph file", Application.dataPath, textField.value, "cs");
            graphView.GenerateAsset(path);
        }
        private void OnSaveHandler(float obj) => progressBar.value = obj;
        
    }
}
