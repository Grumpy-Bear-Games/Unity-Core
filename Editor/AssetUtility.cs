using UnityEditor;
using UnityEngine;

namespace Games.GrumpyBear.Core.Editor
{
    public static class AssetUtility
    {
        public static void RenameAsset(Object obj, string newName)
        {
            obj.name = newName;
            EditorUtility.SetDirty(obj);

            if (AssetDatabase.IsSubAsset(obj))
            {
                var assetPath = AssetDatabase.GetAssetPath(obj);
                var mainAsset = AssetDatabase.LoadMainAssetAtPath(assetPath);
                AssetDatabase.RemoveObjectFromAsset(obj);
                EditorUtility.SetDirty(mainAsset);
                AssetDatabase.SaveAssetIfDirty(mainAsset);
                AssetDatabase.AddObjectToAsset(obj, mainAsset);
                EditorUtility.SetDirty(mainAsset);
                AssetDatabase.SaveAssetIfDirty(mainAsset);
                
            }
            else
            {
                AssetDatabase.SaveAssetIfDirty(obj);
                AssetDatabase.Refresh();
            }
        }
    }
}
