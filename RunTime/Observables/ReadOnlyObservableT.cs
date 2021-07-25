// ReSharper disable UnusedMember.Global
using System;
using System.Collections.Generic;

namespace Games.GrumpyBear.Core.Observables
{
    public class ReadOnlyObservable<T> : IReadonlyObservable<T>
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

        protected void Set(T value) => Value = value;

        public void Subscribe(Action<T> subscriber)
        {
            OnChange += subscriber;
            subscriber(Value);
        }

        public void Unsubscribe(Action<T> subscriber) => OnChange -= subscriber;

        protected ReadOnlyObservable() => _value = default;
        protected ReadOnlyObservable(T initialValue) => _value = initialValue;
        protected ReadOnlyObservable(Action<T> onChange) => OnChange += onChange;

        protected ReadOnlyObservable(T initialValue, Action<T> onChange, bool trigger = false) {
            _value = initialValue;
            OnChange = onChange;
            if (trigger) OnChange?.Invoke(_value);
        }

        public static implicit operator T(ReadOnlyObservable<T> o) => o.Value;

        private T _value;
    }
}
