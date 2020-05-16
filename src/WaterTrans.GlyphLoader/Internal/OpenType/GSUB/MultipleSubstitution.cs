// <copyright file="MultipleSubstitution.cs" company="WaterTrans">
// © 2020 WaterTrans
// </copyright>

using System.Collections.Generic;

namespace WaterTrans.GlyphLoader.Internal.OpenType.GSUB
{
    /// <summary>
    /// The GSUB multiple substitution lookup.
    /// </summary>
    internal sealed class MultipleSubstitution
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MultipleSubstitution"/> class.
        /// </summary>
        /// <param name="glyphIndex">Sets original glyph index.</param>
        /// <param name="substitutionGlyphIndex">Sets lookup glyph index.</param>
        internal MultipleSubstitution(ushort glyphIndex, ushort[] substitutionGlyphIndex)
        {
            GlyphIndex = glyphIndex;
            SubstitutionGlyphIndex = substitutionGlyphIndex;
        }

        /// <summary>Gets an original glyph index.</summary>
        public ushort GlyphIndex { get; }

        /// <summary>Gets a lookup glyph index.</summary>
        public ushort[] SubstitutionGlyphIndex { get; }
    }
}
