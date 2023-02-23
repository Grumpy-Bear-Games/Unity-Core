
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Games.GrumpyBear.Core.SaveSystem
{
    public abstract class SerializableScriptableObject<T>: ScriptableObject where T: SerializableScriptableObject<T>
    {
        [SerializeField] private string _guid;
        public string GUID => _guid;

        private static Dictionary<string, T> _instances = null;

        public static T GetByGUID(string guid)
        {
            if (_instances == null) FindAllInstances();
            _instances.TryGetValue(guid, out var instance);
            return instance;
        }

        private static void FindAllInstances()
        {
            _instances = new Dictionary<string, T>();
            foreach (var instance in Resources.FindObjectsOfTypeAll<T>())
            {
                _instances.Add(instance._guid, instance);
            }
        }

        #if UNITY_EDITOR
        private void OnValidate()
        {
            var path = AssetDatabase.GetAssetPath(this);
            var guid = AssetDatabase.AssetPathToGUID(path);
            if (_guid == guid) return;
            _guid = guid;
            AssetDatabase.SaveAssetIfDirty(this);
        }
        #endif
    }
}
