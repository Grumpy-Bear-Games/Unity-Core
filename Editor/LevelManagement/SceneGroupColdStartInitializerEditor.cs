using Games.GrumpyBear.Core.LevelManagement;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Games.GrumpyBear.Core.Editor.LevelManagement
{
    
    [CustomEditor(typeof(SceneGroupColdStartInitializer))]
    public class SceneGroupColdStartInitializerEditor: UnityEditor.Editor
    {
        [SerializeField] private StyleSheet _styleSheet;
        [SerializeField] private VisualTreeAsset _visualTreeAsset;
        
        private SerializedProperty _sceneGroupProperty;
        private SceneGroupColdStartInitializer _sceneGroupColdStartInitializer;

        private Button _loadButton;
        private HelpBox _helpBox;

        private void OnEnable()
        {
            _sceneGroupProperty = serializedObject.FindProperty(SceneGroupColdStartInitializer.Fields.SceneGroup);
            _sceneGroupColdStartInitializer = target as SceneGroupColdStartInitializer;
        }

        public override VisualElement CreateInspectorGUI()
        {
            var root = _visualTreeAsset.Instantiate();
            root.styleSheets.Add(_styleSheet);
            root.Q<PropertyField>("SceneGroup").RegisterValueChangeCallback(_ => UpdateEditor());

            _helpBox = root.Q<HelpBox>();
            _loadButton = root.Q<Button>("LoadSceneGroupButton");
            _loadButton.clicked += () => _sceneGroupColdStartInitializer.SceneGroup.LoadInEditor();

            _helpBox.Q<Button>().clicked += () =>
            {
                var scene = _sceneGroupColdStartInitializer.gameObject.scene;
                var sceneGroup = SceneGroup.FindFirstWithScene(scene);
                if (sceneGroup == null || sceneGroup == _sceneGroupProperty.objectReferenceValue) return;

                serializedObject.UpdateIfRequiredOrScript();
                _sceneGroupProperty.objectReferenceValue = sceneGroup;
                serializedObject.ApplyModifiedProperties();
            };

            UpdateEditor();

            return root;
        }

        private void UpdateEditor()
        {
            var sceneGroup = _sceneGroupColdStartInitializer.SceneGroup;
            var scene = _sceneGroupColdStartInitializer.gameObject.scene;
            
            if (sceneGroup == null)
            {
                _loadButton.SetEnabled(false);
                _helpBox.text = "Scene Group has not been set";
                _helpBox.style.display = DisplayStyle.Flex;
            } else if (!sceneGroup.ContainsScene(scene))
            {
                _loadButton.SetEnabled(false);
                _helpBox.text = "Scene Group does not contain this scene";
                _helpBox.style.display = DisplayStyle.Flex;
            } else {
                _loadButton.SetEnabled(true);
                _helpBox.style.display = DisplayStyle.None;
            }
        }
    }
}
