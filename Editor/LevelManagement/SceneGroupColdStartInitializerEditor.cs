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
        private SerializedProperty _locationProperty;
        private SceneGroupColdStartInitializer _sceneGroupColdStartInitializer;
        private Button _loadButton;
        private HelpBox _helpBox;

        private void OnEnable()
        {
            _locationProperty = serializedObject.FindProperty("_sceneGroup");
            _sceneGroupColdStartInitializer = target as SceneGroupColdStartInitializer;
        }

        public override VisualElement CreateInspectorGUI()
        {
            var root = new VisualElement
            {
                style =
                {
                    flexDirection = FlexDirection.Column
                }
            };
            var sceneGroupField = new PropertyField(_locationProperty);
            sceneGroupField.RegisterValueChangeCallback(evt => UpdateEditor());
            
            root.Add(sceneGroupField);

            _helpBox = new HelpBox
            {
                style =
                {
                    display = DisplayStyle.None
                },
                messageType = HelpBoxMessageType.Error,
            };
            root.Add(_helpBox);
            
            _loadButton = new Button
            {
                text = "Load Scene group",
                style = {
                    alignSelf = Align.Center,
                    marginTop = 20,
                },
            };
            _loadButton.clicked += () => _sceneGroupColdStartInitializer.SceneGroup.LoadInEditor();
            root.Add(_loadButton);

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
                _helpBox.text = "Scene Group missing";
                _helpBox.style.display = DisplayStyle.Flex;
            } else if (!sceneGroup.ContainsScene(scene))
            {
                _loadButton.SetEnabled(false);
                _helpBox.text = "Scene Group does not contain this scene";
                _helpBox.style.display = DisplayStyle.Flex;
            }
            else
            {
                _loadButton.SetEnabled(true);
                _helpBox.style.display = DisplayStyle.None;
            }
        }
    }
}
