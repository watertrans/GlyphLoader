// <copyright file="SingleSubstitution.cs" company="WaterTrans">
// © 2020 WaterTrans
// </copyright>

namespace WaterTrans.GlyphLoader.Internal.OpenType.GSUB
{
    /// <summary>
    /// The GSUB single substitution lookup.
    /// </summary>
    internal sealed class SingleSubstitution
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SingleSubstitution"/> class.
        /// </summary>
        /// <param name="glyphIndex">Sets original glyph index.</param>
        /// <param name="substitutionGlyphIndex">Sets lookup glyph index.</param>
        internal SingleSubstitution(ushort glyphIndex, ushort substitutionGlyphIndex)
        {
            GlyphIndex = glyphIndex;
            SubstitutionGlyphIndex = substitutionGlyphIndex;
        }

        /// <summary>Gets an original glyph index.</summary>
        public ushort GlyphIndex { get; }

        /// <summary>Gets a lookup glyph index.</summary>
        public ushort SubstitutionGlyphIndex { get; }
    }
}
