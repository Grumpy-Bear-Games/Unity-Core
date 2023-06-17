using Games.GrumpyBear.Core.LevelManagement;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Games.GrumpyBear.Core.Editor.LevelManagement
{
    [CustomEditor(typeof(SceneGroup))]
    public class SceneGroupEditor: UnityEditor.Editor
    {
        [SerializeField] private VisualTreeAsset _visualTreeAsset;
        [SerializeField] private StyleSheet _styleSheet;
        
        private SceneGroup _sceneGroup;
        private TextField _nameProperty;

        private void OnEnable()
        {
            _sceneGroup = target as SceneGroup;
        }

        public override VisualElement CreateInspectorGUI()
        {
            var root = _visualTreeAsset.Instantiate();
            root.styleSheets.Add(_styleSheet);
            _nameProperty = root.Q<TextField>("NameField");
            _nameProperty.value = target.name;
            _nameProperty.RegisterCallback<FocusOutEvent>(_ =>
            {
                if (_sceneGroup.name == _nameProperty.value) return;
                if (EditorUtility.DisplayDialog("Rename Scene Group?",
                        $"Are you sure you want to rename Scene Group from '{_sceneGroup.name}' to '{_nameProperty.value}'?", "Rename", "Cancel"))
                {
                    AssetUtility.RenameAsset(_sceneGroup, _nameProperty.value);
                }
                else
                {
                    _nameProperty.SetValueWithoutNotify(_sceneGroup.name);
                }
            });

            var button = root.Q<Button>("LoadInEditorButton");
            button.clicked += () =>
            {
                _sceneGroup.LoadInEditor();
            };
            button.SetEnabled(!EditorApplication.isPlayingOrWillChangePlaymode);

            root.Bind(serializedObject);
            return root;
        }
    }
}
