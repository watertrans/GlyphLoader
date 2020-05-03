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
        internal TableOfVMTX(TypefaceReader reader, ushort numberOfVMetrics, ushort numGlyphs)
        {
            ushort ah = 0;
            short tsb = 0;

            for (ushort i = 0; i < numGlyphs; i++)
            {
                if (i < numberOfVMetrics)
                {
                    ah = reader.ReadUInt16();
                }

                tsb = reader.ReadInt16();

                AdvanceHeights[i] = ah;
                TopSideBearings[i] = tsb;
            }
        }

        /// <summary>Gets an advance height, in font design units.</summary>
        public Dictionary<ushort, ushort> AdvanceHeights { get; } = new Dictionary<ushort, ushort>();

        /// <summary>Gets an glyph top side bearing, in font design units.</summary>
        public Dictionary<ushort, short> TopSideBearings { get; } = new Dictionary<ushort, short>();
    }
}
