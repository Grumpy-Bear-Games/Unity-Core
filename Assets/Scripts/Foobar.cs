using Games.GrumpyBear.Core.SaveSystem;
using UnityEngine;

namespace DefaultNamespace
{
    [CreateAssetMenu(menuName = "Create Foobar", fileName = "Foobar", order = 0)]
    public class Foobar: SerializableScriptableObject<Foobar>
    {
        [SerializeField] private float _foo;


        [ContextMenu("Save me")]
        private void SaveMe()
        {
            Debug.Log(ObjectGuid);
            FileSystem.SaveFile("foo", ObjectGuid);
        }

        [ContextMenu("Find me")]
        private void FindMe()
        {
            var objectGuid = FileSystem.LoadFile<ObjectGuid>("foo");
            Debug.Log(ObjectGuid);
            var foo = GetByGuid(objectGuid);
            Debug.Log(foo);
        }

        
        [ContextMenu("Find me (generic)")]
        private void FindMeGeneric()
        {
            var objectGuid = FileSystem.LoadFile<ObjectGuid>("foo");
            Debug.Log(ObjectGuid);
            var foo = SerializableScriptableObject.GetByGuid(objectGuid);
            Debug.Log(foo);
        }
        
    }
}
