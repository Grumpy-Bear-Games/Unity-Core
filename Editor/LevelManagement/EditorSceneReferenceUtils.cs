using UnityEditor;
using UnityEngine;

namespace Games.GrumpyBear.Core.Editor.LevelManagement
{
    public static class EditorSceneReferenceUtils
    {
        private const string NoSceneSelected = "No scene selected!";
        private static float NoSceneHelpBoxHeight => EditorStyles.helpBox.CalcHeight(new GUIContent(NoSceneSelected), EditorGUIUtility.currentViewWidth);
        private static float ObjectFieldHeight => EditorStyles.objectField.CalcHeight(GUIContent.none, EditorGUIUtility.currentViewWidth);

        public static string AssetPathField<T>(Rect position, string assetPath) where T: Object
        {
            var asset = string.IsNullOrEmpty(assetPath)
                ? null
                : AssetDatabase.LoadAssetAtPath<T>(assetPath);
            asset = EditorGUI.ObjectField(position, asset, typeof(T), false) as T;
            return asset != null ? AssetDatabase.GetAssetPath(asset) : null;
        }

        public static string SceneReferenceField(Rect position, string scenePath, bool required) {
            var sceneAsset = string.IsNullOrEmpty(scenePath)
                ? null
                : AssetDatabase.LoadAssetAtPath<SceneAsset>(scenePath);
            
            var objectPickerPosition = position;
            objectPickerPosition.height = ObjectFieldHeight;
            
            sceneAsset = EditorGUI.ObjectField(objectPickerPosition, sceneAsset, typeof(SceneAsset), false) as SceneAsset;
            scenePath = sceneAsset != null ? AssetDatabase.GetAssetPath(sceneAsset) : null;

            if (!required || !string.IsNullOrEmpty(scenePath)) return scenePath;
            
            var helpBoxPosition = position;
            helpBoxPosition.y += objectPickerPosition.height + EditorGUIUtility.standardVerticalSpacing;
            helpBoxPosition.height = NoSceneHelpBoxHeight;
            EditorGUI.HelpBox(helpBoxPosition, NoSceneSelected, MessageType.Error);

            return scenePath;
        }
        
        public static float CalcHeight(string scenePath, bool required)
        {
            var height = ObjectFieldHeight;
            if (required && string.IsNullOrEmpty(scenePath))
            {
                height += EditorGUIUtility.standardVerticalSpacing + NoSceneHelpBoxHeight;
            }
            
            return height;
        }
    }
}
