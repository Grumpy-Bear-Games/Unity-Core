using Games.GrumpyBear.Core.Observables.ScriptableObjects;
using NUnit.Framework;
using UnityEngine;

namespace Games.GrumpyBear.Core
{
    [TestOf(typeof(GlobalStateT<>))]
    public class GlobalStateTest
    {
        public class TestGlobalState : GlobalStateT<TestGlobalState> { }

        private TestGlobalState a;
        private TestGlobalState b;

        [SetUp]
        public void Setup()
        {
            a = ScriptableObject.CreateInstance(typeof(TestGlobalState)) as TestGlobalState;
            b = ScriptableObject.CreateInstance(typeof(TestGlobalState)) as TestGlobalState;

            Assume.That(a, Is.Not.Null);
            Assume.That(b, Is.Not.Null);
        }

        [TearDown]
        public void TearDown()
        {
            TestGlobalState.SetToNull();
        }

        [Test]
        [Description("Verify that GlobalStateT<T>.SetAction() and GlobalStateT<T>.IsActive works correctly")]
        public void GlobalStateTIsActive()
        {
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
        [Description("Verify that GlobalStateT<T>.OnEnter and GlobalStateT<T>.OnEnter triggers correctly")]
        public void GlobalStateTOnEnterOnLeave()
        {
            var onEnterObserver = new ObservablesTest.CallbackObserver();
            var onLeaveObserver = new ObservablesTest.CallbackObserver();
            
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
        [Description("Verify that GlobalStateT<T>.Subscribe() triggers the right callbacks, and that GlobalStateT<T>.Current holds the correct value")]
        public void GlobalStateTSubscribe()
        {
            var observer = new ObservablesTest.CallbackObserver<TestGlobalState>();
            TestGlobalState.Subscribe(observer.Callback);
            
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
