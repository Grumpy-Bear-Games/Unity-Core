using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Games.GrumpyBear.Core.SaveSystem
{
    [ExecuteAlways]
    public class SaveableEntity : MonoBehaviour
    {
        [Tooltip("The unique ID is automatically generated in a scene file if " +
                 "left empty. Do not set in a prefab unless you want all instances to " + 
                 "be linked.")]        
        [SerializeField] private string _id;

        private Dictionary<string, object> CaptureState()
        {
            var state = new Dictionary<string, object>();
            foreach (var saveableComponent in GetComponents<ISaveableComponent>())
            {
                state[GetComponentID(saveableComponent)] = saveableComponent.CaptureState();
            }
            return state;
        }

        private void RestoreState(Dictionary<string, object> state)
        {
            foreach (var saveableComponent in GetComponents<ISaveableComponent>())
            {
                var componentID = GetComponentID(saveableComponent);
                if (!state.ContainsKey(componentID)) continue;
                saveableComponent.RestoreState(state[componentID]);
            }
        }
        
        public static void CaptureEntityStates(Dictionary<string, Dictionary<string, object>> state)
        {
            foreach (var saveableEntity in FindObjectsOfType<SaveableEntity>(true))
            {
                state[saveableEntity._id] = saveableEntity.CaptureState();
            }
        }

        public static void RestoreEntityStates(Dictionary<string, Dictionary<string, object>> state)
        {
            foreach (var saveableEntity in FindObjectsOfType<SaveableEntity>(true))
            {
                if (!state.ContainsKey(saveableEntity._id)) continue;
                
                saveableEntity.RestoreState(state[saveableEntity._id]);
            }
        }

        private static string GetComponentID(ISaveableComponent saveableComponent) =>
            saveableComponent.GetType().ToString();
        
        #if UNITY_EDITOR
        private static readonly Dictionary<string, SaveableEntity> _globalLookup = new();
        
        private void Update() {
            if (Application.IsPlaying(gameObject)) return;
            if (string.IsNullOrEmpty(gameObject.scene.path)) return;

            var serializedObject = new SerializedObject(this);
            var property = serializedObject.FindProperty("_id");
            
            if (string.IsNullOrEmpty(property.stringValue) || !IsUnique(property.stringValue))
            {
                property.stringValue = System.Guid.NewGuid().ToString();
                serializedObject.ApplyModifiedProperties();
            }

            _globalLookup[property.stringValue] = this;
        }

        private bool IsUnique(string candidate)
        {
            if (!_globalLookup.ContainsKey(candidate)) return true;

            if (_globalLookup[candidate] == this) return true;

            if (_globalLookup[candidate] is null)
            {
                _globalLookup.Remove(candidate);
                return true;
            }

            if (_globalLookup[candidate]._id == candidate) return false;
            _globalLookup.Remove(candidate);
            return true;

        }
        #endif
    }
}
