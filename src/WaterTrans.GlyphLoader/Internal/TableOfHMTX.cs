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
            ushort aw = 0;
            short lsb = 0;

            for (ushort i = 0; i < numGlyphs; i++)
            {
                if (i < numberOfHMetrics)
                {
                    aw = reader.ReadUInt16();
                }

                lsb = reader.ReadInt16();

                AdvanceWidths[i] = aw;
                LeftSideBearings[i] = lsb;
                DesignUnitsAdvanceWidths[i] = (double)aw / unitsPerEm;
                DesignUnitsLeftSideBearings[i] = (double)lsb / unitsPerEm;
            }
        }

        /// <summary>Gets an advance width.</summary>
        public IDictionary<ushort, ushort> AdvanceWidths { get; } = new Dictionary<ushort, ushort>();

        /// <summary>Gets an glyph left side bearing.</summary>
        public IDictionary<ushort, short> LeftSideBearings { get; } = new Dictionary<ushort, short>();

        /// <summary>Gets an advance width, in font design units.</summary>
        public IDictionary<ushort, double> DesignUnitsAdvanceWidths { get; } = new Dictionary<ushort, double>();

        /// <summary>Gets an glyph left side bearing, in font design units.</summary>
        public IDictionary<ushort, double> DesignUnitsLeftSideBearings { get; } = new Dictionary<ushort, double>();
    }
}
