using System.Runtime.Serialization;
using UnityEngine;

namespace Games.GrumpyBear.Core.SaveSystem
{
    public class Vector3Surrogate : ISerializationSurrogate
    {
        public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
        {
            var vector3 = (Vector3) obj;
            info.AddValue("x", (double) vector3.x);
            info.AddValue("y", (double) vector3.y);
            info.AddValue("z", (double) vector3.z);
        }

        public object SetObjectData(object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
        {
            var x = (float) info.GetDouble("x");
            var y = (float) info.GetDouble("y");
            var z = (float) info.GetDouble("z");
            return new Vector3(x, y, z);
        }
    }
}
