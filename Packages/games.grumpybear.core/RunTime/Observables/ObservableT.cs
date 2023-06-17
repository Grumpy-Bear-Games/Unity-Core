// ReSharper disable UnusedType.Global
namespace Games.GrumpyBear.Core.Observables
{
    public class Observable<T>: ReadOnlyObservable<T>, IObservable<T> {
        public new T Value
        {
            get => base.Value;
            set => base.Value = value;
        }

        public new void Set(T value) => base.Set(value);
    }
}
