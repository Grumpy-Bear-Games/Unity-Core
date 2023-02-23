﻿using Games.GrumpyBear.Core.LevelManagement;
using UnityEditor;
using UnityEngine;

namespace Games.GrumpyBear.Core.Editor.LevelManagement
{
    
    [CustomEditor(typeof(SceneGroupColdStartInitializer))]
    public class SceneGroupColdStartInitializerEditor: UnityEditor.Editor
    {
        private SerializedProperty _locationProperty;
        private SceneGroupColdStartInitializer _sceneGroupColdStartInitializer;

        private void OnEnable()
        {
            _locationProperty = serializedObject.FindProperty("_sceneGroup");
            _sceneGroupColdStartInitializer = target as SceneGroupColdStartInitializer;
        }

        
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            
            var sceneGroup = _locationProperty.objectReferenceValue as SceneGroup;
            
            if (sceneGroup == null) EditorGUILayout.HelpBox("Scene Group missing", MessageType.Warning);
            if (!sceneGroup.ContainsScene(_sceneGroupColdStartInitializer.gameObject.scene)) EditorGUILayout.HelpBox("Scene Group does not contain this scene", MessageType.Error);
            
            var canLoad = (sceneGroup != null) && (sceneGroup.Scenes.Count > 0);  
            GUI.enabled = canLoad;
            if (GUILayout.Button("Load Scene Group")) sceneGroup.LoadInEditor();
            GUI.enabled = true;
        }
    }
}
