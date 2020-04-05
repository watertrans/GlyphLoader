// <copyright file="AlternateSubstitution.cs" company="WaterTrans">
// © 2020 WaterTrans
// </copyright>

using System.Collections.Generic;

namespace WaterTrans.GlyphLoader.Internal.OpenType.GSUB
{
    /// <summary>
    /// The GSUB alternate substitution lookup.
    /// </summary>
    internal sealed class AlternateSubstitution
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AlternateSubstitution"/> class.
        /// </summary>
        /// <param name="glyphIndex">Sets original glyph index.</param>
        /// <param name="substitutionGlyphIndex">Sets lookup glyph index.</param>
        internal AlternateSubstitution(ushort glyphIndex, List<ushort> substitutionGlyphIndex)
        {
            GlyphIndex = glyphIndex;
            SubstitutionGlyphIndex = substitutionGlyphIndex;
        }

        /// <summary>Gets an original glyph index.</summary>
        public ushort GlyphIndex { get; }

        /// <summary>Gets a lookup glyph index.</summary>
        public List<ushort> SubstitutionGlyphIndex { get; } = new List<ushort>();
    }
}
