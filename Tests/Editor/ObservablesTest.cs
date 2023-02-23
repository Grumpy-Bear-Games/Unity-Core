using Games.GrumpyBear.Core.Observables;
using NUnit.Framework;

namespace Games.GrumpyBear.Core
{
    public partial class ObservablesTest
    {
        [Test]
        public void ObservableTOnChange()
        {
            var o = new Observable<int>();
            var observer = new CallbackObserver<int>();

            o.OnChange += observer.Callback;
            Assert.AreEqual(observer.CallCount, 0);  // Manually subscribing to OnChange shouldn't do anything
            o.Set(42);
            Assert.AreEqual(o.Value, observer.Value);
            Assert.AreEqual(observer.CallCount, 1);
            
            o.Set(42);
            Assert.AreEqual(observer.CallCount, 1); // CallCount should only go up if the value actually changes
            
            o.OnChange -= observer.Callback;
            o.Set(43);
            Assert.AreEqual(observer.CallCount, 1); // CallCount should not go up after unsubscribing
        }

        [Test]
        public void ObservableTSubscribeUnsubscribe()
        {
            var o = new Observable<int>();
            var observer = new CallbackObserver<int>();
            o.Set(42);
            
            o.Subscribe(observer.Callback);
            Assert.AreEqual(o.Value, observer.Value);
            Assert.AreEqual(observer.CallCount, 1);
            
            o.Set(43);
            Assert.AreEqual(o.Value, observer.Value);
            Assert.AreEqual(observer.CallCount, 2);
            
            o.Set(43);
            Assert.AreEqual(observer.CallCount, 2); // CallCount should only go up if the value actually changes
            
            o.Unsubscribe(observer.Callback);
            o.Set(44);
            Assert.AreEqual(observer.CallCount, 2); // CallCount should not go up after unsubscribing
        }
    }
}
