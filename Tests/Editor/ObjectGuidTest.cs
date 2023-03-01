using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Games.GrumpyBear.Core.SaveSystem;
using NUnit.Framework;
using UnityEngine;

namespace Games.GrumpyBear.Core
{
    public class ObjectGuidTest
    {
        private Guid guid;

        private ObjectGuid a; 
        private ObjectGuid b;
        private ObjectGuid c;

        [SetUp]
        public void Setup()
        {
            guid = Guid.NewGuid();
            a = new ObjectGuid(guid, long.MaxValue);
            b = new ObjectGuid(guid.ToString("N"), long.MaxValue);
            c = new ObjectGuid(guid, long.MinValue);
        }

        [Test]
        public void ObjectGuidConstructor()
        {
            Assert.That(a.Guid, Is.EqualTo(guid));
            Assert.That(b.Guid, Is.EqualTo(guid));
            Assert.That(c.Guid, Is.EqualTo(guid));
            
            Assert.That(a.LocalId, Is.EqualTo(long.MaxValue));
            Assert.That(b.LocalId, Is.EqualTo(long.MaxValue));
            Assert.That(c.LocalId, Is.EqualTo(long.MinValue));
        }

        [Test]
        public void ObjectGuidEquals()
        {
            Assert.That(a.Equals(b), Is.True);
            Assert.That(a.Equals(c), Is.False);
        }
        
        [Test]
        public void ObjectGuidEqualOperator()
        {
            Assert.That(a==b, Is.True);
            Assert.That(a==c, Is.False);
        }

        [Test]
        public void ObjectGuidGetHashCode()
        {
            Assert.That(a.GetHashCode(), Is.EqualTo(b.GetHashCode()));
            Assert.That(a.GetHashCode(), Is.Not.EqualTo(c.GetHashCode()));
        }

        [Test]
        public void ObjectGuidSerialization()
        {
            var formatter = new BinaryFormatter();
            using var writeStream = new MemoryStream();
            formatter.Serialize(writeStream, a);

            Debug.Log(writeStream.Length);
            var bytes = writeStream.GetBuffer();
            using var readStream = new MemoryStream(bytes);
            var deserializedA = formatter.Deserialize(readStream) as ObjectGuid;
            
            Assert.That(deserializedA, Is.Not.Null);
            Assert.That(deserializedA, Is.EqualTo(a));
            Assert.That(deserializedA.Guid, Is.EqualTo(guid));
            Assert.That(deserializedA.LocalId, Is.EqualTo(long.MaxValue));
        }

        [Test]
        public void ObjectGuidHashable()
        {
            var dict = new Dictionary<ObjectGuid, ObjectGuid>();
            
            dict.Add(a, a);
            Assert.That(dict.ContainsKey(a), Is.True);
            Assert.That(dict.ContainsKey(b), Is.True);
            
            dict.Add(c, c);
            Assert.That(dict.ContainsKey(c), Is.True);
            
            Assert.That(dict[a], Is.EqualTo(a));
            Assert.That(dict[c], Is.EqualTo(c));
        }

    }
}
