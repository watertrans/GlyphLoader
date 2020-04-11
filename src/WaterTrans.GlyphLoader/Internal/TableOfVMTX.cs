// <copyright file="TableOfVMTX.cs" company="WaterTrans">
// © 2020 WaterTrans
// </copyright>

using System.Collections.Generic;

namespace WaterTrans.GlyphLoader.Internal
{
    /// <summary>
    /// Table of VMTX.
    /// </summary>
    internal sealed class TableOfVMTX
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TableOfVMTX"/> class.
        /// </summary>
        /// <param name="reader">The <see cref="TypefaceReader"/>.</param>
        /// <param name="numberOfVMetrics">The number of vMetrics in ‘vhea’ table.</param>
        /// <param name="numGlyphs">The number of glyphs in ‘maxp’ table.</param>
        /// <param name="unitsPerEm">The unitsPerEm in ‘head’ table.</param>
        internal TableOfVMTX(TypefaceReader reader, ushort numberOfVMetrics, ushort numGlyphs, ushort unitsPerEm)
        {
            double ah = 0;
            double tsb = 0;

            for (ushort i = 0; i < numGlyphs; i++)
            {
                if (i < numberOfVMetrics)
                {
                    ah = (double)reader.ReadUInt16() / unitsPerEm;
                }

                tsb = (double)reader.ReadInt16() / unitsPerEm;

                AdvanceHeights[i] = ah;
                TopSideBearings[i] = tsb;
            }
        }

        /// <summary>Gets an advance height, in font design units.</summary>
        public IDictionary<ushort, double> AdvanceHeights { get; } = new Dictionary<ushort, double>();

        /// <summary>Gets an glyph top side bearing, in font design units.</summary>
        public IDictionary<ushort, double> TopSideBearings { get; } = new Dictionary<ushort, double>();
    }
}
