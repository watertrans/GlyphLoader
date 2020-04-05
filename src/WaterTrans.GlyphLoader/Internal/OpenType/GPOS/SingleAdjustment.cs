// <copyright file="SingleAdjustment.cs" company="WaterTrans">
// © 2020 WaterTrans
// </copyright>

namespace WaterTrans.GlyphLoader.Internal.OpenType.GPOS
{
    /// <summary>
    /// The Single Adjustment Positioning Subtable.
    /// </summary>
    internal sealed class SingleAdjustment
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SingleAdjustment"/> class.
        /// </summary>
        /// <param name="glyphIndex">Sets GlyphIndex.</param>
        /// <param name="valueRecord">Sets ValueRecord.</param>
        internal SingleAdjustment(ushort glyphIndex, ValueRecord valueRecord)
        {
            GlyphIndex = glyphIndex;
            ValueRecord = valueRecord;
        }

        /// <summary>Gets a glyph index.</summary>
        public ushort GlyphIndex { get; }

        /// <summary>Gets a ValueRecord.</summary>
        public ValueRecord ValueRecord { get; }
    }
}
