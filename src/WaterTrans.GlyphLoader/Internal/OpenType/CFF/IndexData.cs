// <copyright file="IndexData.cs" company="WaterTrans">
// © 2020 WaterTrans
// </copyright>

using System.Collections.Generic;

namespace WaterTrans.GlyphLoader.Internal.OpenType.CFF
{
    /// <summary>
    /// The Compact FontFormat Specification INDEX Data.
    /// </summary>
    internal class IndexData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IndexData"/> class.
        /// </summary>
        /// <param name="reader">The <see cref="TypefaceReader"/>.</param>
        internal IndexData(TypefaceReader reader)
        {
            Count = reader.ReadUInt16();

            if (Count == 0)
            {
                return;
            }

            OffsetSize = reader.ReadByte();

            for (int i = 0; i < Count + 1; i++)
            {
                Offsets.Add(reader.ReadOffset(OffsetSize));
            }

            for (int i = 0; i < Offsets.Count - 1; i++)
            {
                Objects.Add(reader.ReadBytes((int)(Offsets[i + 1] - Offsets[i])));
            }
        }

        /// <summary>
        /// Gets a Number of objects stored in INDEX.
        /// </summary>
        public ushort Count { get; }

        /// <summary>
        /// Gets an offset array element size.
        /// </summary>
        public byte OffsetSize { get; }

        /// <summary>
        /// Gets a list of offset.
        /// </summary>
        public List<uint> Offsets { get; } = new List<uint>();

        /// <summary>
        /// Gets a lisy of object.
        /// </summary>
        public List<byte[]> Objects { get; } = new List<byte[]>();
    }
}
