using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Games.GrumpyBear.Core.Observables.ScriptableObjects
{
    public class ReadOnlyObservable<T> : ScriptableObject, IReadonlyObservable<T>
    {
        public enum NotificationOrder {
            [InspectorName("Execute Unity events before script events")]
            ScriptsBeforeUnityEvents,
            [InspectorName("Execute script events before Unity events")]
            UnityEventsBeforeScripts,
        }

        [Header("Unity events (this can not reference scene objects)")]

        [SerializeField] private UnityEvent<T> _onChange;
        
        [Tooltip("In which order should events be executed")]
        [SerializeField] private NotificationOrder _notificationOrder;
        public event Action<T> OnChange;

        public T Value {
            get => _value;
            protected set
            {
                if (EqualityComparer<T>.Default.Equals(_value, value)) return;
                _value = value;
                switch (_notificationOrder)
                {
                    case NotificationOrder.ScriptsBeforeUnityEvents:
                        OnChange?.Invoke(_value);
                        _onChange.Invoke(_value);
                        break;
                    case NotificationOrder.UnityEventsBeforeScripts:
                        _onChange.Invoke(_value);
                        OnChange?.Invoke(_value);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
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
