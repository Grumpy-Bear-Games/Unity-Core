using System;
using System.Collections.Generic;
using UnityEngine;

namespace Games.GrumpyBear.Core.Observables.ScriptableObjects
{
    public class ReadOnlyObservable<T> : ScriptableObject, IReadonlyObservable<T>
    {
        public event Action<T> OnChange;

        public T Value {
            get => _value;
            protected set
            {
                if (EqualityComparer<T>.Default.Equals(_value, value)) return;
                _value = value;
                OnChange?.Invoke(_value);
            }
        }

        private void OnDisable() => Set(default);

        protected void Set(T value) => Value = value;

        public void Subscribe(Action<T> subscriber)
        {
            OnChange += subscriber;
            subscriber(Value);
        }

        public void Unsubscribe(Action<T> subscriber) => OnChange -= subscriber;

        public static implicit operator T(ReadOnlyObservable<T> o) => o.Value;

        private T _value;
    }
}
