using Games.GrumpyBear.Core.LevelManagement;
using UnityEditor;
using UnityEngine;

namespace Games.GrumpyBear.Core.Editor.LevelManagement
{
    [CustomPropertyDrawer(typeof(SceneReference))]
    public class SceneReferenceDrawer: PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            
            var scenePathProperty = property.FindPropertyRelative("_scenePath");
            scenePathProperty.stringValue = EditorSceneReferenceUtils.SceneReferenceField(position, scenePathProperty.stringValue, true);
            
            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var scenePathProperty = property.FindPropertyRelative("_scenePath");
            return EditorSceneReferenceUtils.CalcHeight(scenePathProperty.stringValue, true);
        }
    }
}
