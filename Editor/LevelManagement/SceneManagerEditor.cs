using System.Collections.Generic;
using System.Linq;
using Games.GrumpyBear.Core.LevelManagement;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using Debug = UnityEngine.Debug;
using SerializedObject = UnityEditor.SerializedObject;

namespace Games.GrumpyBear.Core.Editor.LevelManagement
{
    [CustomEditor(typeof(SceneManager))]
    public class SceneManagerEditor: UnityEditor.Editor
    {
        [SerializeField] private VisualTreeAsset _visualTreeAsset;
        [SerializeField] private VisualTreeAsset _itemVisualTreeAsset;

        private SceneManager _sceneManager;
        private SerializedProperty _sceneGroupsProperty;
        private ListView _scenes;
        
        private void OnEnable()
        {
            _sceneManager = target as SceneManager;
            _sceneGroupsProperty = serializedObject.FindProperty(SceneManager.SceneGroupsPropertyName);
        }

        public override VisualElement CreateInspectorGUI()
        {
            var myInspector = _visualTreeAsset.CloneTree();

            _scenes = myInspector.Q<ListView>("SceneGroups");
            _scenes.makeItem += MakeSceneGroupVisualElement;
            _scenes.bindItem += OnBindScenesGroup;

            _scenes.showAddRemoveFooter = true;
            _scenes.itemsRemoved += OnSceneGroupsDeleted;
            _scenes.itemsAdded += OnSceneGroupsAdded;
            return myInspector;
        }

        private void OnSceneGroupsAdded(IEnumerable<int> indicies)
        {
            serializedObject.UpdateIfRequiredOrScript();
            foreach (var index in indicies)
            {
                var sceneGroup = SceneGroup.CreateInstance(_sceneManager);
                AssetDatabase.AddObjectToAsset(sceneGroup, target);
                sceneGroup.name = $"<new scene group {index}>";
                _sceneGroupsProperty.GetArrayElementAtIndex(index).objectReferenceValue = sceneGroup;
            }
            AssetDatabase.SaveAssetIfDirty(target);
            serializedObject.ApplyModifiedProperties();
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
            var sceneGroupVisualElement = _itemVisualTreeAsset.CloneTree();
            var button = sceneGroupVisualElement.Q<Button>("RenameButton");
            var field = sceneGroupVisualElement.Q<TextField>("NameField");
            button.RegisterCallback<ClickEvent>(evt =>
            {
                var sceneGroup = field.userData as SceneGroup;
                var so = new SerializedObject(sceneGroup);
                var sp = so.FindProperty("m_Name");
                
                sp.stringValue = field.value;
                so.ApplyModifiedProperties();
            
                EditorUtility.SetDirty(sceneGroup);
                AssetDatabase.RemoveObjectFromAsset(sceneGroup);
                EditorUtility.SetDirty(_sceneManager);
                AssetDatabase.SaveAssetIfDirty(_sceneManager);
                AssetDatabase.AddObjectToAsset(sceneGroup, _sceneManager);
                EditorUtility.SetDirty(_sceneManager);
                AssetDatabase.SaveAssetIfDirty(_sceneManager);
            
                button.SetEnabled(false);
                
            });
            field.RegisterValueChangedCallback(evt =>
            {
                var sceneGroup = field.userData as SceneGroup;
                button.SetEnabled(!string.IsNullOrWhiteSpace(field.value) && field.value != sceneGroup.name);
            });
            return sceneGroupVisualElement;
        }

        private void OnBindScenesGroup(VisualElement element, int i)
        {
            if (i >= _sceneGroupsProperty.arraySize) return;
            
            var sceneGroup = _sceneGroupsProperty.GetArrayElementAtIndex(i).objectReferenceValue as SceneGroup;
            var so = new SerializedObject(sceneGroup);
            element.Bind(so);
            var field = element.Q<TextField>("NameField");
            field.userData = sceneGroup;
            field.value = sceneGroup.name;
        }

        /*
        public override void OnInspectorGUI()
        {
            var hasProblems = false;
            foreach (var scene in _sceneManager.GlobalScenes)
            {
                if (string.IsNullOrEmpty(scene.ScenePath)) continue;
                if (scene.BuildIndex != -1) continue;
                EditorGUILayout.HelpBox($"{scene.ScenePath} is missing from the build", MessageType.Warning);
                hasProblems = true;
            }
            
            if (hasProblems && GUILayout.Button("Fix all problems")) FixAllProblems();
        }

        private void FixAllProblems()
        {
            var editorBuildSettingsScenes = EditorBuildSettings.scenes.ToList();
            editorBuildSettingsScenes.AddRange(
                from scene in _sceneManager.GlobalScenes
                where scene.BuildIndex == -1
                select new EditorBuildSettingsScene(scene.ScenePath, true)
            );
            EditorBuildSettings.scenes = editorBuildSettingsScenes.ToArray();
        }
        */
    }
}
