// ReSharper disable EventNeverSubscribedTo.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedMemberInSuper.Global
using System;

namespace Games.GrumpyBear.Core.Observables
{
    public interface IReadonlyObservable<out T>
    {
        event Action<T> OnChange;

        T Value { get; }
        void Subscribe(Action<T> subscriber);
        void Unsubscribe(Action<T> subscriber);
    }
}
