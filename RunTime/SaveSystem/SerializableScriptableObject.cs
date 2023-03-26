using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace Games.GrumpyBear.Core.SaveSystem
{
    public abstract class SerializableScriptableObject : ScriptableObject
    {
        [field: SerializeField][field: HideInInspector] public ObjectGuid ObjectGuid { get; private set; }
        
        protected static Dictionary<ObjectGuid, SerializableScriptableObject> _instances;
        
        public static SerializableScriptableObject GetByGuid(ObjectGuid guid)
        {
            FindAllInstances();
            _instances.TryGetValue(guid, out var instance);
            return instance;
        }

        protected static void FindAllInstances()
        {
            if (_instances != null) return;
            _instances = new Dictionary<ObjectGuid, SerializableScriptableObject>();
            foreach (var instance in Resources.FindObjectsOfTypeAll<SerializableScriptableObject>())
            {
                _instances.Add(instance.ObjectGuid, instance);
            }
        }

        protected virtual void OnEnable()
        {
            if (_instances == null) return;
            if (_instances.TryGetValue(ObjectGuid, out var entry) && entry == this) return;
            if (entry != null)
            {
                Debug.Log($"Duplicate ObjectGuid {ObjectGuid}. This should really not happen", this);
                _instances.Remove(ObjectGuid);
            }
            _instances.Add(ObjectGuid, this);
        }

        protected virtual void OnDestroy() => _instances?.Remove(ObjectGuid);

        #if UNITY_EDITOR
        protected virtual void Reset() => OnValidate();

        protected virtual void OnValidate()
        {
            if (!AssetDatabase.TryGetGUIDAndLocalFileIdentifier(this, out var guid, out long localId)) return;
            var objectGuid = new ObjectGuid(guid, localId);
            if (objectGuid == ObjectGuid) return;
            if (_instances != null && _instances.ContainsKey(ObjectGuid)) _instances.Remove(ObjectGuid);
            _instances?.Add(objectGuid, this);
            ObjectGuid = objectGuid;
            EditorUtility.SetDirty(this);
            EditorApplication.delayCall += () => AssetDatabase.SaveAssetIfDirty(this);
        }
        #endif        
    }
}
