using System.Collections.Generic;
using System.Linq;
using Games.GrumpyBear.Core.LevelManagement;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UIElements;
using Debug = UnityEngine.Debug;
using SerializedObject = UnityEditor.SerializedObject;

namespace Games.GrumpyBear.Core.Editor.LevelManagement
{
    [CustomEditor(typeof(SceneManager))]
    public class SceneManagerEditor: UnityEditor.Editor
    {

        [Header("Scene Manager")]
        [SerializeField] private StyleSheet _sceneManagerStyleSheet;
        [SerializeField] private VisualTreeAsset _sceneManagerVisualTreeAsset;

        [Header("Scene Groups (embedded)")]
        [SerializeField] private StyleSheet _sceneGroupStyleSheet;
        [SerializeField] private VisualTreeAsset _sceneGroupVisualTreeAsset;

        private SceneManager _sceneManager;
        private SerializedProperty _sceneGroupsProperty;
        private ListView _scenes;
        
        private void OnEnable()
        {
            _sceneManager = target as SceneManager;
            _sceneGroupsProperty = serializedObject.FindProperty(SceneManager.Fields.SceneGroups);
        }

        public override VisualElement CreateInspectorGUI()
        {
            var sceneManagerRoot = _sceneManagerVisualTreeAsset.Instantiate();
            sceneManagerRoot.styleSheets.Add(_sceneManagerStyleSheet);
            sceneManagerRoot.styleSheets.Add(_sceneGroupStyleSheet);

            _scenes = sceneManagerRoot.Q<ListView>("SceneGroups");
            _scenes.makeItem += MakeSceneGroupVisualElement;
            _scenes.bindItem += OnBindScenesGroup;

            _scenes.showAddRemoveFooter = true;
            _scenes.itemsRemoved += OnSceneGroupsDeleted;
            _scenes.itemsAdded += OnSceneGroupsAdded;
            
            sceneManagerRoot.RegisterCallback<SerializedPropertyChangeEvent>(_ => ValidateSceneManager(sceneManagerRoot));
            ValidateSceneManager(sceneManagerRoot);
            
            return sceneManagerRoot;
        }

        private void ValidateSceneManager(VisualElement root)
        {
            root.EnableInClassList("ScenesMissingFromBuild", !_sceneManager.AllScenesInBuild());
        }

        private void OnSceneGroupsAdded(IEnumerable<int> indicies)
        {
            serializedObject.UpdateIfRequiredOrScript();
            foreach (var index in indicies)
            {
                var sceneGroup = SceneGroup.CreateInstance(_sceneManager);
                sceneGroup.name = $"<new scene group {index}>";
                AssetDatabase.AddObjectToAsset(sceneGroup, target);
                _sceneGroupsProperty.GetArrayElementAtIndex(index).objectReferenceValue = sceneGroup;
            }
            serializedObject.ApplyModifiedProperties();
            AssetDatabase.SaveAssetIfDirty(target);
        }

        private void OnSceneGroupsDeleted(IEnumerable<int> ints)
        {
            serializedObject.ApplyModifiedProperties();
            foreach (var obj in AssetDatabase.LoadAllAssetRepresentationsAtPath(AssetDatabase.GetAssetPath(target)))
            {
                var sceneGroup = obj as SceneGroup;
                if (sceneGroup == null || _sceneManager.SceneGroups.Contains(sceneGroup)) continue;
                Debug.Log($"Deleting {sceneGroup}");
                AssetDatabase.RemoveObjectFromAsset(sceneGroup);
                DestroyImmediate(sceneGroup, true);
                EditorUtility.SetDirty(target);
                AssetDatabase.SaveAssetIfDirty(target);
            }

            // Better implementations, but currently failing due to bugs
            /*
            foreach (var index in ints)
            {
                var sceneGroup = _sceneManager.SceneGroups[index];
                Debug.Log($"Deleting {sceneGroup}");
                AssetDatabase.RemoveObjectFromAsset(sceneGroup);
                DestroyImmediate(sceneGroup, true);
                EditorUtility.SetDirty(target);
                AssetDatabase.SaveAssetIfDirty(target);
            }
            */
        }

        private VisualElement MakeSceneGroupVisualElement()
        {
            var sceneGroupRoot = _sceneGroupVisualTreeAsset.Instantiate();
            var nameField = sceneGroupRoot.Q<TextField>("NameField");
            nameField.RegisterCallback<FocusOutEvent>(_ =>
            {
                if (sceneGroupRoot.userData is not SceneGroup sceneGroup) return;
                if (sceneGroup.name == nameField.value) return;
                if (EditorUtility.DisplayDialog("Rename Scene Group?",
                        $"Are you sure you want to rename Scene Group from '{sceneGroup.name}' to '{nameField.value}'?", "Rename", "Cancel"))
                {
                    AssetUtility.RenameAsset(sceneGroup, nameField.value);
                }
                else
                {
                    nameField.SetValueWithoutNotify(sceneGroup.name);
                }
            });
            
            var button = sceneGroupRoot.Q<Button>("LoadInEditorButton");
            button.clicked += () =>
            {
                if (sceneGroupRoot.userData is not SceneGroup sceneGroup) return;
                sceneGroup.LoadInEditor();
            };
            button.SetEnabled(!EditorApplication.isPlayingOrWillChangePlaymode);
            
            
            return sceneGroupRoot;
        }

        private void OnBindScenesGroup(VisualElement sceneGroupRoot, int i)
        {
            if (i >= _sceneGroupsProperty.arraySize) return;
            
            var sceneGroup = _sceneGroupsProperty.GetArrayElementAtIndex(i).objectReferenceValue as SceneGroup;
            Assert.IsNotNull(sceneGroup);
            var so = new SerializedObject(sceneGroup);
            sceneGroupRoot.Bind(so);
            sceneGroupRoot.Q<TextField>("NameField").value = sceneGroup.name;
            sceneGroupRoot.userData = sceneGroup;
        }
    }
}
