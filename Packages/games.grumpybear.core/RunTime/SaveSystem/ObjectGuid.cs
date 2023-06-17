using System;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;

namespace Games.GrumpyBear.Core.SaveSystem
{
    /// <summary>
    /// Represents a unique identifier for a Unity object.
    /// </summary>    
    [Serializable]
    public sealed class ObjectGuid: IEquatable<ObjectGuid>
    {
        [SerializeField] private byte[] _data;

        /// <summary>
        /// Gets the bytes that represent the <see cref="ObjectGuid"/>.
        /// </summary>
        public ReadOnlySpan<byte> Bytes => new(_data, 0, 24);

        /// <summary>
        /// Gets or sets the <see cref="Guid"/> of the <see cref="ObjectGuid"/>.
        /// </summary>
        public Guid Guid
        {
            get => _data is null ? Guid.Empty : new Guid(new ReadOnlySpan<byte>(_data, 0, 16));
            set => Buffer.BlockCopy(value.ToByteArray(), 0, _data, 0, 16);
        }

        /// <summary>
        /// Gets or sets the local ID of the <see cref="ObjectGuid"/>.
        /// </summary>
        public long LocalId
        {
            get => _data is null ? 0L : BitConverter.ToInt64(_data, 16);
            set => Buffer.BlockCopy(BitConverter.GetBytes(value), 0, _data, 16, 8);
        }

        /// <summary>
        /// Returns a string that represents the current <see cref="ObjectGuid"/>.
        /// </summary>
        /// <returns>A string that represents the current <see cref="ObjectGuid"/>.</returns>
        public override string ToString() => $"{Guid:N}-{LocalId:X}".ToUpper();

        #region Constructors
        /// <summary>
        /// Creates an instance of ObjectGuid with default values.
        /// </summary>
        public ObjectGuid()
        {
            _data = new byte[24];
        }

        /// <summary>
        /// Creates an instance of ObjectGuid from an array of bytes.
        /// </summary>
        /// <param name="bytes">Backing byte array for the ObjectGuid. Must be exactly 24 bytes long.</param>
        public ObjectGuid(ref byte[] bytes)
        {
            if (bytes.Length != 24) throw new ArgumentException("Backing byte array must be exactly 24 bytes");
            _data = bytes;
        }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectGuid"/> class with the specified <see cref="Guid"/> and local ID.
        /// </summary>
        /// <param name="guid">The <see cref="Guid"/> used to initialize the <see cref="ObjectGuid"/>.</param>
        /// <param name="localId">The local ID used to initialize the <see cref="ObjectGuid"/>.</param>
        public ObjectGuid(Guid guid, long localId): this()
        {
            Guid = guid;
            LocalId = localId;
        }

        /// <summary>
        /// Creates an instance of ObjectGuid from an array of bytes with the specified Guid and LocalId values.
        /// </summary>
        /// <param name="bytes">Backing byte array for the ObjectGuid. Must be exactly 24 bytes long.</param>
        /// <param name="guid">The Guid value for the ObjectGuid.</param>
        /// <param name="localId">The LocalId value for the ObjectGuid.</param>
        public ObjectGuid(ref byte[] bytes, Guid guid, long localId): this(ref bytes)
        {
            Guid = guid;
            LocalId = localId;
        }

        /// <summary>
        /// Creates an instance of ObjectGuid with the specified Guid and LocalId values.
        /// </summary>
        /// <param name="guid">The Guid value for the ObjectGuid.</param>
        /// <param name="localId">The LocalId value for the ObjectGuid.</param>
        public ObjectGuid(string guid, long localId): this(new Guid(guid), localId) { }

        /// <summary>
        /// Creates an instance of ObjectGuid from an array of bytes with the specified Guid and LocalId values.
        /// </summary>
        /// <param name="bytes">Backing byte array for the ObjectGuid. Must be exactly 24 bytes long.</param>
        /// <param name="guid">The Guid value for the ObjectGuid.</param>
        /// <param name="localId">The LocalId value for the ObjectGuid.</param>
        public ObjectGuid(ref byte[] bytes, string guid, long localId): this(ref bytes, new Guid(guid), localId) { }
        #endregion

        #region Comparison
        /// <summary>
        /// Determines whether two instances of <see cref="ObjectGuid"/> are equal.
        /// </summary>
        /// <param name="a">The first object to compare.</param>
        /// <param name="b">The second object to compare.</param>
        /// <returns>true if <paramref name="a"/> and <paramref name="b"/> are equal; otherwise, false.</returns>
        public static bool operator ==([CanBeNull] ObjectGuid a, [CanBeNull] ObjectGuid b) => a?.Equals(b) ?? b is null;

        /// <summary>
        /// Determines whether two instances of <see cref="ObjectGuid"/> are not equal.
        /// </summary>
        /// <param name="a">The first object to compare.</param>
        /// <param name="b">The second object to compare.</param>
        /// <returns>true if <paramref name="a"/> and <paramref name="b"/> are not equal; otherwise, false.</returns>
        public static bool operator !=([CanBeNull] ObjectGuid a, [CanBeNull] ObjectGuid b) => !(a == b);

        /// <summary>
        /// Determines whether two instances of <see cref="ObjectGuid"/> are not equal.
        /// </summary>
        /// <param name="a">The first object to compare.</param>
        /// <param name="b">The second object to compare.</param>
        /// <returns>true if <paramref name="a"/> and <paramref name="b"/> are not equal; otherwise, false.</returns>
        public bool Equals(ObjectGuid other) => other?._data is not null && _data is not null && _data.SequenceEqual(other._data);

        /// <inheritdoc/>
        public override bool Equals(object obj) => (obj is ObjectGuid otherGuid) && Equals(otherGuid);

        /// <inheritdoc/>
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
