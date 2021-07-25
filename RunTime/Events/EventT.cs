using System;
using UnityEngine;

namespace Games.GrumpyBear.Core.Events
{
    public class EventT<T> : ScriptableObject
    {
        public event Action<T> Listeners;
        public void Invoke(T value) => Listeners?.Invoke(value);
    }
}
