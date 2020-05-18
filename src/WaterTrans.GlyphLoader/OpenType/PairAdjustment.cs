// <copyright file="PairAdjustment.cs" company="WaterTrans">
// © 2020 WaterTrans
// </copyright>

namespace WaterTrans.GlyphLoader.OpenType
{
    /// <summary>
    /// The GPOS pair adjustment.
    /// </summary>
    public class PairAdjustment
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PairAdjustment"/> class.
        /// </summary>
        /// <param name="first">The value of a first glyph adjustment for placement.</param>
        /// <param name="second">The value of a second glyph adjustment for placement.</param>
        internal PairAdjustment(Adjustment first, Adjustment second)
        {
            First = first;
            Second = second;
        }

        /// <summary>Gets a first glyph adjustment for placement.</summary>
        public Adjustment First { get; }

        /// <summary>Gets a second glyph adjustment for placement.</summary>
        public Adjustment Second { get; }
    }
}
