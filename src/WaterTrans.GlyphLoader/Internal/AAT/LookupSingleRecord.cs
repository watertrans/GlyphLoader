// <copyright file="LookupSingleRecord.cs" company="WaterTrans">
// © 2020 WaterTrans
// </copyright>

namespace WaterTrans.GlyphLoader.Internal.AAT
{
    /// <summary>
    /// The metamorphosis subtable's lookup single record.
    /// </summary>
    internal sealed class LookupSingleRecord
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LookupSingleRecord"/> class.
        /// </summary>
        /// <param name="reader">The <see cref="TypefaceReader"/>.</param>
        internal LookupSingleRecord(TypefaceReader reader)
        {
            GlyphIndex = reader.ReadUInt16();
            LookupGlyphIndex = reader.ReadUInt16();
        }

        /// <summary>Gets GlyphIndex.</summary>
        /// <remarks>The glyph index.</remarks>
        public ushort GlyphIndex { get; }

        /// <summary>Gets LookupGlyphIndex.</summary>
        /// <remarks>The lookup value.</remarks>
        public ushort LookupGlyphIndex { get; }
    }
}
