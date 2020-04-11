// <copyright file="TableOfPOST.cs" company="WaterTrans">
// © 2020 WaterTrans
// </copyright>

namespace WaterTrans.GlyphLoader.Internal
{
    /// <summary>
    /// Table of POST.
    /// </summary>
    internal sealed class TableOfPOST
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TableOfPOST"/> class.
        /// </summary>
        /// <param name="reader">The <see cref="TypefaceReader"/>.</param>
        internal TableOfPOST(TypefaceReader reader)
        {
            Version = reader.ReadFixed();
            ItalicAngle = reader.ReadFixed();
            UnderlinePosition = reader.ReadFword();
            UnderlineThickness = reader.ReadFword();
            IsFixedPitch = reader.ReadUInt32();
            MinMemType42 = reader.ReadUInt32();
            MaxMemType42 = reader.ReadUInt32();
            MinMemType1 = reader.ReadUInt32();
            MaxMemType1 = reader.ReadUInt32();

            // Not interested in the data after here...
        }

        /// <summary>Gets the table version.</summary>
        public float Version { get; }

        /// <summary>Gets the italic angle in counter-clockwise degrees from the vertical. Zero for upright text, negative for text that leans to the right (forward).</summary>
        public float ItalicAngle { get; }

        /// <summary>Gets suggested distance of the top of the underline from the baseline (negative values indicate below baseline).</summary>
        public short UnderlinePosition { get; }

        /// <summary>Gets suggested values for the underline thickness. In general, the underline thickness should match the thickness of the underscore character (U+005F LOW LINE), and should also match the strikeout thickness, which is specified in the OS/2 table.</summary>
        public short UnderlineThickness { get; }

        /// <summary>Gets the fixed pitch. Set to 0 if the font is proportionally spaced, non-zero if the font is not proportionally spaced (i.e. monospaced).</summary>
        public uint IsFixedPitch { get; }

        /// <summary>Gets the minimum memory usage when an OpenType font is downloaded.</summary>
        public uint MinMemType42 { get; }

        /// <summary>Gets the maximum memory usage when an OpenType font is downloaded.</summary>
        public uint MaxMemType42 { get; }

        /// <summary>Gets the minimum memory usage when an OpenType font is downloaded as a Type 1 font.</summary>
        public uint MinMemType1 { get; }

        /// <summary>Gets the maximum memory usage when an OpenType font is downloaded as a Type 1 font.</summary>
        public uint MaxMemType1 { get; }
    }
}
