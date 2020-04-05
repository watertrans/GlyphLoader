// <copyright file="TableOfHMTX.cs" company="WaterTrans">
// © 2020 WaterTrans
// </copyright>

using System.Collections.Generic;

namespace WaterTrans.GlyphLoader.Internal
{
    /// <summary>
    /// Table of HMTX.
    /// </summary>
    internal sealed class TableOfHMTX
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TableOfHMTX"/> class.
        /// </summary>
        /// <param name="reader">The <see cref="TypefaceReader"/>.</param>
        /// <param name="numberOfHMetrics">The number of hMetrics in ‘hhea’ table.</param>
        /// <param name="numGlyphs">The number of glyphs in ‘maxp’ table.</param>
        /// <param name="unitsPerEm">The unitsPerEm in ‘head’ table.</param>
        internal TableOfHMTX(TypefaceReader reader, ushort numberOfHMetrics, ushort numGlyphs, ushort unitsPerEm)
        {
            double aw = 0;
            double lsb = 0;

            for (ushort i = 0; i < numGlyphs; i++)
            {
                if (i < numberOfHMetrics)
                {
                    aw = (double)reader.ReadUInt16() / unitsPerEm;
                }

                lsb = (double)reader.ReadInt16() / unitsPerEm;

                AdvanceWidths[i] = aw;
                LeftSideBearings[i] = lsb;
            }
        }

        /// <summary>Gets an advance width, in font design units.</summary>
        public IDictionary<ushort, double> AdvanceWidths { get; } = new Dictionary<ushort, double>();

        /// <summary>Gets an glyph left side bearing, in font design units.</summary>
        public IDictionary<ushort, double> LeftSideBearings { get; } = new Dictionary<ushort, double>();
    }
}
