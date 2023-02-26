using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Games.GrumpyBear.Core.SaveSystem
{
    public abstract class SerializableScriptableObject<T>: ScriptableObject where T: SerializableScriptableObject<T>
    {
        
        [field: SerializeField] public ObjectGuid ObjectGuid { get; private set; }

        private static Dictionary<ObjectGuid, T> _instances;

        public static T GetByGuid(ObjectGuid guid)
        {
            if (_instances == null) FindAllInstances();
            _instances.TryGetValue(guid, out var instance);
            return instance;
        }

        private static void FindAllInstances()
        {
            _instances = new Dictionary<ObjectGuid, T>();
            foreach (var instance in Resources.FindObjectsOfTypeAll<T>())
            {
                _instances.Add(instance.ObjectGuid, instance);
            }
        }

        #if UNITY_EDITOR
        protected virtual void OnValidate()
        {
            if (!AssetDatabase.TryGetGUIDAndLocalFileIdentifier(this, out var guid, out long localId)) return;
            var objectGuid = new ObjectGuid(guid, localId);
            if (objectGuid == ObjectGuid) return;
            if (_instances != null && _instances.TryGetValue(ObjectGuid, out var entry) && entry == this as T) _instances.Remove(ObjectGuid);
            ObjectGuid = objectGuid;
            _instances?.Add(ObjectGuid, this as T);
            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssetIfDirty(this);
        }
        #endif
    }
}
