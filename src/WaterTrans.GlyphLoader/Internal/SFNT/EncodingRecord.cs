// <copyright file="EncodingRecord.cs" company="WaterTrans">
// © 2020 WaterTrans
// </copyright>

using System;
using System.Collections.Generic;

namespace WaterTrans.GlyphLoader.Internal.SFNT
{
    /// <summary>
    /// The offset to the subtable for each encoding.
    /// </summary>
    internal sealed class EncodingRecord
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EncodingRecord"/> class.
        /// </summary>
        /// <param name="reader">The <see cref="TypefaceReader"/>.</param>
        internal EncodingRecord(TypefaceReader reader)
        {
            PlatformID = reader.ReadUInt16();
            EncodingID = reader.ReadUInt16();
            Offset = reader.ReadUInt32();
        }

        /// <summary>Gets a platform ID.</summary>
        public ushort PlatformID { get; }

        /// <summary>Gets a platform-specific encoding ID.</summary>
        public ushort EncodingID { get; }

        /// <summary>Gets a byte offset from beginning of table to the subtable for this encoding.</summary>
        public uint Offset { get; }

        /// <summary>Gets the mapping of a charactor code point to a glyph index.</summary>
        public Dictionary<int, ushort> GlyphMap { get; } = new Dictionary<int, ushort>();

        /// <summary>Gets the mapping of a Unicode Variation Sequences to a glyph index.</summary>
        public Dictionary<Tuple<uint, uint>, ushort> VariationGlyphMap { get; } = new Dictionary<Tuple<uint, uint>, ushort>();
    }
}
