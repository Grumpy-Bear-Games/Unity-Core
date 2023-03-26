namespace Games.GrumpyBear.Core.SaveSystem
{
    public abstract class SerializableScriptableObject<T>: SerializableScriptableObject where T: SerializableScriptableObject<T>
    {
        public new static T GetByGuid(ObjectGuid guid)
        {
            FindAllInstances();
            _instances.TryGetValue(guid, out var instance);
            return instance as T;
        }
    }
}
