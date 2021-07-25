using System;
using UnityEngine;

namespace Games.GrumpyBear.Core.Events
{
    [CreateAssetMenu(menuName = "Grumpy Bear Games/Core/Events/Void Event", fileName = "Void Event")]
    public class VoidEvent : ScriptableObject
    {
        public event Action Listeners;
        public void Invoke() => Listeners?.Invoke();
    }
}
