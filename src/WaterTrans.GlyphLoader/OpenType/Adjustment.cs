// <copyright file="Adjustment.cs" company="WaterTrans">
// © 2020 WaterTrans
// </copyright>

namespace WaterTrans.GlyphLoader.OpenType
{
    /// <summary>
    /// The GPOS adjustment.
    /// </summary>
    public class Adjustment
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Adjustment"/> class.
        /// </summary>
        /// <param name="xPlacement">The value of a horizontal adjustment for placement.</param>
        /// <param name="yPlacement">The value of a vertical adjustment for placement.</param>
        /// <param name="xAdvance">The value of a horizontal adjustment for advance.</param>
        /// <param name="yAdvance">The value of a vertical adjustment for advance.</param>
        /// <param name="unitsPerEm">The units per em in 'head' table.</param>
        internal Adjustment(short xPlacement, short yPlacement, short xAdvance, short yAdvance, ushort unitsPerEm)
        {
            XPlacement = (double)xPlacement / unitsPerEm;
            YPlacement = (double)yPlacement / unitsPerEm;
            XAdvance = (double)xAdvance / unitsPerEm;
            YAdvance = (double)yAdvance / unitsPerEm;
        }

        /// <summary>Gets a horizontal adjustment for placement.</summary>
        public double XPlacement { get; }

        /// <summary>Gets a vertical adjustment for placement.</summary>
        public double YPlacement { get; }

        /// <summary>Gets a horizontal adjustment for advance.</summary>
        public double XAdvance { get; }

        /// <summary>Gets a vertical adjustment for advance.</summary>
        public double YAdvance { get; }
    }
}
