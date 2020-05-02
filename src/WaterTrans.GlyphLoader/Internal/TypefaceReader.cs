// <copyright file="TypefaceReader.cs" company="WaterTrans">
// © 2020 WaterTrans
// </copyright>

using System;
using System.IO;
using System.Text;

namespace WaterTrans.GlyphLoader.Internal
{
    /// <summary>
    /// Internal processing class for accessing font data.
    /// </summary>
    internal sealed class TypefaceReader
    {
        private byte[] _byteArray;

        /// <summary>
        /// Initializes a new instance of the <see cref="TypefaceReader"/> class.
        /// </summary>
        /// <param name="byteArray">The font file byte array.</param>
        /// <param name="position">The byte array position.</param>
        internal TypefaceReader(byte[] byteArray, long position)
        {
            _byteArray = byteArray;
            Position = position;
        }

        /// <summary>
        /// Gets or sets the stream position.
        /// </summary>
        public long Position { get; set; }

        /// <summary>
        /// Read char array.
        /// </summary>
        /// <param name="len">Number of length.</param>
        /// <returns>Read result.</returns>
        public string ReadCharArray(int len)
        {
            var sb = new StringBuilder();
            for (int i = 0; i < len; i++)
            {
                sb.Append(ReadChar());
            }
            return sb.ToString();
        }

        /// <summary>
        /// Read byte array.
        /// </summary>
        /// <param name="len">Number of length.</param>
        /// <returns>Read result.</returns>
        public byte[] ReadBytes(int len)
        {
            byte[] result = new byte[len];
            Array.Copy(_byteArray, Position, result, 0, len);
            Position += len;
            return result;
        }

        /// <summary>
        /// Read specified (1-4) length.
        /// </summary>
        /// <param name="size">The value is element size.</param>
        /// <returns>Read result.</returns>
        public uint ReadOffset(byte size)
        {
            switch (size)
            {
                case 1:
                    return ReadByte();
                case 2:
                    return ReadUInt16();
                case 3:
                    return ReadUInt24();
                case 4:
                    return ReadUInt32();
            }
            throw new ArgumentOutOfRangeException(nameof(size));
        }

        /// <summary>
        /// Read char.
        /// </summary>
        /// <returns>Read result.</returns>
        public char ReadChar()
        {
            return (char)ReadByte();
        }

        /// <summary>
        /// Read sbyte.
        /// </summary>
        /// <returns>Read result.</returns>
        public sbyte ReadSByte()
        {
            return unchecked((sbyte)ReadByte());
        }

        /// <summary>
        /// Read byte.
        /// </summary>
        /// <returns>Read result.</returns>
        public byte ReadByte()
        {
            byte result = _byteArray[Position];
            Position += 1;
            return result;
        }

        /// <summary>
        /// Read UInt16.
        /// </summary>
        /// <returns>Read result.</returns>
        public ushort ReadUInt16()
        {
            return BitConverter.ToUInt16(ReadBytesInternal(2), 0);
        }

        /// <summary>
        /// Read Int16.
        /// </summary>
        /// <returns>Read result.</returns>
        public short ReadInt16()
        {
            return BitConverter.ToInt16(ReadBytesInternal(2), 0);
        }

        /// <summary>
        /// Read UInt24.
        /// </summary>
        /// <returns>Read result.</returns>
        public uint ReadUInt24()
        {
            byte[] buf = ReadBytesInternal(3);
            return (uint)(buf[0] | (buf[1] << 8) | (buf[2] << 16));
        }

        /// <summary>
        /// Read UInt32.
        /// </summary>
        /// <returns>Read result.</returns>
        public uint ReadUInt32()
        {
            return BitConverter.ToUInt32(ReadBytesInternal(4), 0);
        }

        /// <summary>
        /// Read Int32.
        /// </summary>
        /// <returns>Read result.</returns>
        public int ReadInt32()
        {
            return BitConverter.ToInt32(ReadBytesInternal(4), 0);
        }

        /// <summary>
        /// Read UInt64.
        /// </summary>
        /// <returns>Read result.</returns>
        public ulong ReadUInt64()
        {
            return BitConverter.ToUInt64(ReadBytesInternal(8), 0);
        }

        /// <summary>
        /// Read Int64.
        /// </summary>
        /// <returns>Read result.</returns>
        public long ReadInt64()
        {
            return BitConverter.ToInt64(ReadBytesInternal(8), 0);
        }

        /// <summary>
        /// Read Fixed.
        /// </summary>
        /// <returns>Read result.</returns>
        public float ReadFixed()
        {
            return ReadInt32() / 65536F;
        }

        /// <summary>
        /// Read Fword.
        /// </summary>
        /// <returns>Read result.</returns>
        public short ReadFword()
        {
            return ReadInt16();
        }

        /// <summary>
        /// Read F2Dot14.
        /// </summary>
        /// <returns>Read result.</returns>
        public float ReadF2Dot14()
        {
            return (float)ReadInt16() / 16384;
        }

        private byte[] ReadBytesInternal(int count)
        {
            byte[] buff = ReadBytes(count);
            Array.Reverse(buff);
            return buff;
        }
    }
}
