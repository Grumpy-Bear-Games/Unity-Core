using System;
using Games.GrumpyBear.Core.LevelManagement;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Games.GrumpyBear.Core.Editor.LevelManagement
{
    [CustomPropertyDrawer(typeof(SceneReference))]
    public class SceneReferenceDrawer: PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            var root = Resources.Load<VisualTreeAsset>("SceneReferenceDrawer").Instantiate();
            root.styleSheets.Add(Resources.Load<StyleSheet>("SceneReferenceDrawer"));

            var updateLogic = new UpdateLogic(property);
            root.userData = updateLogic;
            
            var sceneSelector = root.Q<ObjectField>();
            sceneSelector.objectType = typeof(SceneAsset); 
            sceneSelector.RegisterValueChangedCallback(SceneSelected);
            sceneSelector.SetValueWithoutNotify(updateLogic.SceneAsset);
            
            var scenePathField = root.Q<TextField>("ScenePath");
            scenePathField.BindProperty(updateLogic.ScenePathSerializedProperty);
            scenePathField.SetEnabled(false);

            root.Q<Button>("AddToBuildButton").RegisterCallback<ClickEvent>(AddToBuild);

            updateLogic.OnChanged += () => Validate(sceneSelector.parent);
            Validate(sceneSelector.parent);

            return root;
        }

        private void AddToBuild(ClickEvent evt)
        {
            if (evt.target is not VisualElement target) return;
            if (target.FindAncestorUserData() is not UpdateLogic updateLogic) return;
            updateLogic.AddToBuild();
        }

        private static void Validate(VisualElement root)
        {
            if (root.FindAncestorUserData() is not UpdateLogic updateLogic) return;

            var sceneReferenceStatus = updateLogic.SceneReference.Validate();
            root.EnableInClassList("NoSceneSelected", sceneReferenceStatus == ExtensionMethods.SceneReferenceStatus.NoSceneSelected);
            root.EnableInClassList("InvalidScenePath",  sceneReferenceStatus == ExtensionMethods.SceneReferenceStatus.InvalidScenePath);
            root.EnableInClassList("MissingFromBuild",  sceneReferenceStatus == ExtensionMethods.SceneReferenceStatus.SceneMissingFromBuild);
        }

        private static void SceneSelected(ChangeEvent<UnityEngine.Object> evt)
        {
            if (evt.target is not ObjectField sceneSelector) return;
            if (sceneSelector.FindAncestorUserData() is not UpdateLogic updateLogic) return;
            
            if (evt.newValue == evt.previousValue) return;
            var sceneAsset = evt.newValue as SceneAsset;
            updateLogic.SceneAssetPath = sceneAsset ? AssetDatabase.GetAssetPath(sceneAsset) : null;
        }
        
        
        private class UpdateLogic
        {
            private readonly SerializedProperty _sceneReferenceProperty;
            public readonly SerializedProperty ScenePathSerializedProperty;

            public string SceneAssetPath
            {
                get => ScenePathSerializedProperty.stringValue;
                set
                {
                    ScenePathSerializedProperty.stringValue = value;
                    ScenePathSerializedProperty.serializedObject.ApplyModifiedProperties();
                    ScenePathSerializedProperty.serializedObject.UpdateIfRequiredOrScript();
                    OnChanged?.Invoke();
                }
            }

            public SceneAsset SceneAsset => AssetDatabase.LoadAssetAtPath<SceneAsset>(SceneAssetPath);
            public SceneReference SceneReference => _sceneReferenceProperty.boxedValue as SceneReference;

            public void AddToBuild()
            {
                SceneReference.AddToBuild();
                OnChanged?.Invoke();
            }

            public event Action OnChanged;

            public UpdateLogic(SerializedProperty sceneReferenceProperty)
            {
                _sceneReferenceProperty = sceneReferenceProperty;
                ScenePathSerializedProperty = sceneReferenceProperty.FindPropertyRelative(SceneReference.Fields.ScenePath);
            }
        }
    }
}
