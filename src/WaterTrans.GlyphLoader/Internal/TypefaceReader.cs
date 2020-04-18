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
    internal sealed class TypefaceReader : IDisposable
    {
        private readonly BinaryReader _reader;
        private bool _disposedValue = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="TypefaceReader"/> class.
        /// </summary>
        /// <param name="stream">The font file stream.</param>
        internal TypefaceReader(Stream stream)
        {
            _reader = new BinaryReader(stream, Encoding.UTF8, true);
        }

        /// <summary>
        /// Gets stream.
        /// </summary>
        public Stream Stream
        {
            get
            {
                return _reader.BaseStream;
            }
        }

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
            return _reader.ReadSByte();
        }

        /// <summary>
        /// Read byte.
        /// </summary>
        /// <returns>Read result.</returns>
        public byte ReadByte()
        {
            return _reader.ReadByte();
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

        /// <inheritdoc/>
        public void Dispose()
        {
            Dispose(true);
        }

        private byte[] ReadBytesInternal(int count)
        {
            byte[] buff = _reader.ReadBytes(count);
            Array.Reverse(buff);
            return buff;
        }

        private void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    _reader.Dispose();
                }

                _disposedValue = true;
            }
        }
    }
}
