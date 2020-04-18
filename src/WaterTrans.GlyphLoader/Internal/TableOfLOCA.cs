// <copyright file="TableOfLOCA.cs" company="WaterTrans">
// © 2020 WaterTrans
// </copyright>

using System.Collections.Generic;

namespace WaterTrans.GlyphLoader.Internal
{
    /// <summary>
    /// Table of LOCA.
    /// </summary>
    internal sealed class TableOfLOCA
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TableOfLOCA"/> class.
        /// </summary>
        /// <param name="reader">The <see cref="TypefaceReader"/>.</param>
        /// <param name="numGlyphs">The value for numGlyphs is found in the 'maxp' table.</param>
        /// <param name="indexToLocFormat">The version is specified in the indexToLocFormat entry in the 'head' table.</param>
        internal TableOfLOCA(TypefaceReader reader, ushort numGlyphs, short indexToLocFormat)
        {
            if (indexToLocFormat == 0)
            {
                // Short version
                for (ushort i = 0; i < numGlyphs + 1; i++)
                {
                    Offsets[i] = (uint)(reader.ReadUInt16() * 2);
                }
            }
            else
            {
                // Long version
                for (ushort i = 0; i < numGlyphs + 1; i++)
                {
                    Offsets[i] = reader.ReadUInt32();
                }
            }

            for (ushort i = 0; i < numGlyphs; i++)
            {
                if (Offsets[i] == Offsets[(ushort)(i + 1)])
                {
                    Offsets[i] = uint.MaxValue;
                }
            }
        }

        /// <summary>Gets the offsets to the locations of the glyphs in the font, relative to the beginning of the glyphData table.</summary>
        public IDictionary<ushort, uint> Offsets { get; } = new Dictionary<ushort, uint>();
    }
}
