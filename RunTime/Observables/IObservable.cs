// ReSharper disable EventNeverSubscribedTo.Global
// ReSharper disable UnusedMember.Global
using System;

namespace Games.GrumpyBear.Core.Observables
{
    public interface IObservable<T>
    {
        event Action<T> OnChange;
        
        T Value { get; set; }
        void Set(T value);
        void Subscribe(Action<T> subscriber);
        void Unsubscribe(Action<T> subscriber);
    }
}
