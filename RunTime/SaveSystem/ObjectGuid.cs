using System;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;

namespace Games.GrumpyBear.Core.SaveSystem
{
    [Serializable]
    public sealed class ObjectGuid: IEquatable<ObjectGuid>
    {
        [SerializeField] private byte[] _data;

        public ReadOnlySpan<byte> Bytes => new(_data, 0, 24);
        public Guid Guid
        {
            get => _data is null ? Guid.Empty : new Guid(new ReadOnlySpan<byte>(_data, 0, 16));
            set => Buffer.BlockCopy(value.ToByteArray(), 0, _data, 0, 16);
        }

        public long LocalId
        {
            get => _data is null ? 0L : BitConverter.ToInt64(_data, 16);
            set => Buffer.BlockCopy(BitConverter.GetBytes(value), 0, _data, 16, 8);
        }

        public override string ToString() => $"{Guid:N}-{LocalId:X}".ToUpper();

        #region Constructors
        public ObjectGuid()
        {
            _data = new byte[24];
        }

        public ObjectGuid(ref byte[] bytes)
        {
            if (bytes.Length != 24) throw new ArgumentException("Backing byte array must be exactly 24 bytes");
            _data = bytes;
        }
        
        public ObjectGuid(Guid guid, long localId): this()
        {
            Guid = guid;
            LocalId = localId;
        }

        public ObjectGuid(ref byte[] bytes, Guid guid, long localId): this(ref bytes)
        {
            Guid = guid;
            LocalId = localId;
        }

        public ObjectGuid(string guid, long localId): this(new Guid(guid), localId) { }
        public ObjectGuid(ref byte[] bytes, string guid, long localId): this(ref bytes, new Guid(guid), localId) { }
        #endregion

        #region Comparison
        public static bool operator ==([CanBeNull] ObjectGuid a, [CanBeNull] ObjectGuid b) => a?.Equals(b) ?? b is null;
        public static bool operator !=([CanBeNull] ObjectGuid a, [CanBeNull] ObjectGuid b) => !(a == b);
        public bool Equals(ObjectGuid other) => other?._data is not null && _data is not null && _data.SequenceEqual(other._data);
        public override bool Equals(object obj) => (obj is ObjectGuid otherGuid) && Equals(otherGuid);

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            foreach (var b in _data)
            {
                hashcode.Add(b);    
            }

            return hashcode.ToHashCode();
        }
        #endregion
    }
}
