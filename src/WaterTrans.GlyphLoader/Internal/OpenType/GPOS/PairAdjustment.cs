// <copyright file="PairAdjustment.cs" company="WaterTrans">
// © 2020 WaterTrans
// </copyright>

namespace WaterTrans.GlyphLoader.Internal.OpenType.GPOS
{
    /// <summary>
    /// The Pair Adjustment Positioning Subtable.
    /// </summary>
    internal sealed class PairAdjustment
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PairAdjustment"/> class.
        /// </summary>
        /// <param name="firstGlyphIndex">Sets FirstGlyphIndex.</param>
        /// <param name="firstValueRecord">Sets FirstValueRecord.</param>
        /// <param name="secondGlyphIndex">Sets SecondGlyphIndex.</param>
        /// <param name="secondValueRecord">Sets SecondValueRecord.</param>
        internal PairAdjustment(
            ushort firstGlyphIndex,
            ushort secondGlyphIndex,
            ValueRecord firstValueRecord,
            ValueRecord secondValueRecord)
        {
            FirstGlyphIndex = firstGlyphIndex;
            FirstValueRecord = firstValueRecord;
            SecondGlyphIndex = secondGlyphIndex;
            SecondValueRecord = secondValueRecord;
        }

        /// <summary>Gets a first glyph index.</summary>
        public ushort FirstGlyphIndex { get; }

        /// <summary>Gets a first glyph ValueRecord.</summary>
        public ValueRecord FirstValueRecord { get; }

        /// <summary>Gets a second glyph index.</summary>
        public ushort SecondGlyphIndex { get; }

        /// <summary>Gets a second glyph ValueRecord.</summary>
        public ValueRecord SecondValueRecord { get; }
    }
}
