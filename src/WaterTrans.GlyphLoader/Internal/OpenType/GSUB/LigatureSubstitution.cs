// <copyright file="LigatureSubstitution.cs" company="WaterTrans">
// © 2020 WaterTrans
// </copyright>

using System.Collections.Generic;

namespace WaterTrans.GlyphLoader.Internal.OpenType.GSUB
{
    /// <summary>
    /// The GSUB ligature substitution lookup.
    /// </summary>
    internal sealed class LigatureSubstitution
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LigatureSubstitution"/> class.
        /// </summary>
        /// <param name="glyphIndex">Sets original glyph index.</param>
        /// <param name="substitutionGlyphIndex">Sets lookup glyph index.</param>
        internal LigatureSubstitution(ushort[] glyphIndex, ushort substitutionGlyphIndex)
        {
            GlyphIndex = glyphIndex;
            SubstitutionGlyphIndex = substitutionGlyphIndex;
        }

        /// <summary>Gets an original glyph index.</summary>
        public ushort[] GlyphIndex { get; }

        /// <summary>Gets a lookup glyph index.</summary>
        public ushort SubstitutionGlyphIndex { get; }
    }
}
