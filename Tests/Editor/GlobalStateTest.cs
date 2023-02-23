using Games.GrumpyBear.Core.Observables.ScriptableObjects;
using NUnit.Framework;
using UnityEngine;

namespace Games.GrumpyBear.Core
{
    public class GlobalStateTest
    {
        public class TestGlobalState : GlobalStateT<TestGlobalState> { }

        [Test]
        public void GlobalStateTIsActive()
        {
            var a = ScriptableObject.CreateInstance(typeof(TestGlobalState)) as TestGlobalState;
            var b = ScriptableObject.CreateInstance(typeof(TestGlobalState)) as TestGlobalState;
            
            Assume.That(a, Is.Not.Null);
            Assume.That(b, Is.Not.Null);

            Assert.IsFalse(a.IsActive);
            Assert.IsFalse(b.IsActive);
            
            a.SetActive();
            Assert.IsTrue(a.IsActive);
            Assert.IsFalse(b.IsActive);
            
            b.SetActive();
            Assert.IsFalse(a.IsActive);
            Assert.IsTrue(b.IsActive);
        }

        [Test]
        public void GlobalStateTOnEnterOnLeave()
        {
            var a = ScriptableObject.CreateInstance(typeof(TestGlobalState)) as TestGlobalState;
            var b = ScriptableObject.CreateInstance(typeof(TestGlobalState)) as TestGlobalState;

            var onEnterObserver = new ObservablesTest.CallbackObserver();
            var onLeaveObserver = new ObservablesTest.CallbackObserver();
            
            Assume.That(a, Is.Not.Null);
            Assume.That(b, Is.Not.Null);

            a.OnEnter += onEnterObserver.Callback;
            a.OnLeave += onLeaveObserver.Callback;
            
            Assert.That(onEnterObserver.CallCount, Is.EqualTo(0));
            Assert.That(onLeaveObserver.CallCount, Is.EqualTo(0));
            
            a.SetActive();
            Assert.That(onEnterObserver.CallCount, Is.EqualTo(1));
            Assert.That(onLeaveObserver.CallCount, Is.EqualTo(0));
            
            b.SetActive();
            Assert.That(onEnterObserver.CallCount, Is.EqualTo(1));
            Assert.That(onLeaveObserver.CallCount, Is.EqualTo(1));
        }
        
        [Test]
        public void GlobalStateTSubscribe()
        {
            var a = ScriptableObject.CreateInstance(typeof(TestGlobalState)) as TestGlobalState;
            var b = ScriptableObject.CreateInstance(typeof(TestGlobalState)) as TestGlobalState;

            var observer = new ObservablesTest.CallbackObserver<TestGlobalState>();
            TestGlobalState.Subscribe(observer.Callback);
            
            Assume.That(a, Is.Not.Null);
            Assume.That(b, Is.Not.Null);
            Assume.That(TestGlobalState.Current, Is.Null);
            
            Assert.That(observer.CallCount, Is.EqualTo(1));
            Assert.That(observer.Value, Is.Null);
            
            a.SetActive();
            Assume.That(TestGlobalState.Current, Is.EqualTo(a));
            Assert.That(observer.CallCount, Is.EqualTo(2));
            Assert.That(observer.Value, Is.EqualTo(a));

            b.SetActive();
            Assume.That(TestGlobalState.Current, Is.EqualTo(b));
            Assert.That(observer.CallCount, Is.EqualTo(3));
            Assert.That(observer.Value, Is.EqualTo(b));

            TestGlobalState.Unsubscribe(observer.Callback);
        }
    }
}
