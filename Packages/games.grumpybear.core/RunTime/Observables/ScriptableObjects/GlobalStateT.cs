﻿using System;
using Games.GrumpyBear.Core.SaveSystem;

namespace Games.GrumpyBear.Core.Observables.ScriptableObjects
{
    public abstract class GlobalStateT<T> : SerializableScriptableObject<T> where T: GlobalStateT<T>
    {
        private static readonly Games.GrumpyBear.Core.Observables.Observable<T> _current = new();
        public static T Current => _current.Value;
        
        public static void Subscribe(Action<T> subscriber) => _current.Subscribe(subscriber);
        public static void Unsubscribe(Action<T> subscriber) => _current.Unsubscribe(subscriber);
        
        public event Action OnEnter;
        public event Action OnLeave;

        public bool IsActive => _current.Value == this as T;
        
        public void SetActive()
        {
            if (_current.Value == this) return;
            if (_current.Value != null) _current.Value.OnLeave?.Invoke();
            _current.Set(this as T);
            OnEnter?.Invoke();
        }
        
        public static void SetToNull()
        {
            if (_current.Value == null) return;
            _current.Value.OnLeave?.Invoke();
            _current.Set(null);
        }
    }
}
