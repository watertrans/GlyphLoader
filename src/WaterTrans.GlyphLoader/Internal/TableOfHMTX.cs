// <copyright file="TableOfHMTX.cs" company="WaterTrans">
// © 2020 WaterTrans
// </copyright>

using System;
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
        /// <param name="isTransformed">The value that indicates the WOFF2.0 transformed.</param>
        internal TableOfHMTX(TypefaceReader reader, ushort numberOfHMetrics, ushort numGlyphs, bool isTransformed)
        {
            if (isTransformed)
            {
                var flags = reader.ReadByte();
                if ((flags & 0x01) != 1 && (flags & 0x02) != 1)
                {
                    throw new NotSupportedException("When hmtx transform is indicated by the table directory, the Flags (bits 0 or 1 or both) MUST be set.");
                }

                ushort aw = 0;
                for (ushort i = 0; i < numGlyphs; i++)
                {
                    if (i < numberOfHMetrics)
                    {
                        aw = reader.ReadUInt16();
                    }
                    AdvanceWidths[i] = aw;
                }

                if ((flags & 0x01) != 1)
                {
                    for (ushort i = 0; i < numberOfHMetrics; i++)
                    {
                        LeftSideBearings[i] = reader.ReadInt16();
                    }
                }
                if ((flags & 0x02) != 1)
                {
                    for (ushort i = numberOfHMetrics; i < numGlyphs; i++)
                    {
                        LeftSideBearings[i] = reader.ReadInt16();
                    }
                }
            }
            else
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
                }
            }
        }

        /// <summary>Gets an advance width.</summary>
        public Dictionary<ushort, ushort> AdvanceWidths { get; } = new Dictionary<ushort, ushort>();

        /// <summary>Gets an glyph left side bearing.</summary>
        public Dictionary<ushort, short> LeftSideBearings { get; } = new Dictionary<ushort, short>();
    }
}
