// <copyright file="TableOfOS2.cs" company="WaterTrans">
// © 2020 WaterTrans
// </copyright>

namespace WaterTrans.GlyphLoader.Internal
{
    /// <summary>
    /// Table of OS2.
    /// </summary>
    internal sealed class TableOfOS2
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TableOfOS2"/> class.
        /// </summary>
        /// <param name="reader">The <see cref="TypefaceReader"/>.</param>
        internal TableOfOS2(TypefaceReader reader)
        {
            Version = reader.ReadUInt16();
            AvgCharWidth = reader.ReadInt16();
            WeightClass = reader.ReadUInt16();
            WidthClass = reader.ReadUInt16();
            Type = reader.ReadInt16();
            SubscriptXSize = reader.ReadInt16();
            SubscriptYSize = reader.ReadInt16();
            SubscriptXOffset = reader.ReadInt16();
            SubscriptYOffset = reader.ReadInt16();
            SuperscriptXSize = reader.ReadInt16();
            SuperscriptYSize = reader.ReadInt16();
            SuperscriptXOffset = reader.ReadInt16();
            SuperscriptYOffset = reader.ReadInt16();
            StrikeoutSize = reader.ReadInt16();
            StrikeoutPosition = reader.ReadInt16();
            FamilyClass = reader.ReadInt16();
            Panose1 = reader.ReadByte();
            Panose2 = reader.ReadByte();
            Panose3 = reader.ReadByte();
            Panose4 = reader.ReadByte();
            Panose5 = reader.ReadByte();
            Panose6 = reader.ReadByte();
            Panose7 = reader.ReadByte();
            Panose8 = reader.ReadByte();
            Panose9 = reader.ReadByte();
            Panose10 = reader.ReadByte();
            UnicodeRange1 = reader.ReadUInt32();
            UnicodeRange2 = reader.ReadUInt32();
            UnicodeRange3 = reader.ReadUInt32();
            UnicodeRange4 = reader.ReadUInt32();
            VendorID = reader.ReadCharArray(4);
            Selection = reader.ReadUInt16();
            FirstCharIndex = reader.ReadUInt16();
            LastCharIndex = reader.ReadUInt16();
            TypoAscender = reader.ReadInt16();
            TypoDescender = reader.ReadInt16();
            TypoLineGap = reader.ReadInt16();
            WinAscent = reader.ReadUInt16();
            WinDescent = reader.ReadUInt16();

            if (Version <= 0)
            {
                return;
            }

            CodePageRange1 = reader.ReadUInt32();
            CodePageRange2 = reader.ReadUInt32();

            if (Version <= 1)
            {
                return;
            }

            XHeight = reader.ReadInt16();
            CapHeight = reader.ReadInt16();
            DefaultChar = reader.ReadUInt16();
            BreakChar = reader.ReadUInt16();
            MaxContext = reader.ReadUInt16();

            if (Version <= 4)
            {
                return;
            }

            LowerOpticalPointSize = reader.ReadUInt16();
            UpperOpticalPointSize = reader.ReadUInt16();
        }

        /// <summary>Gets the version number for the OS/2 table: 0x0000 to 0x0005.</summary>
        public ushort Version { get; }

        /// <summary>Gets the Average Character Width parameter specifies the arithmetic average of the escapement (width) of all non-zero width glyphs in the font.</summary>
        public short AvgCharWidth { get; }

        /// <summary>Gets indicates the visual weight (degree of blackness or thickness of strokes) of the characters in the font. Values from 1 to 1000 are valid.</summary>
        public ushort WeightClass { get; }

        /// <summary>Gets indicates a relative change from the normal aspect ratio (width to height ratio) as specified by a font designer for the glyphs in a font.</summary>
        public ushort WidthClass { get; }

        /// <summary>Gets indicates font embedding licensing rights for the font.</summary>
        public short Type { get; }

        /// <summary>Gets the recommended horizontal size in font design units for subscripts for this font.</summary>
        public short SubscriptXSize { get; }

        /// <summary>Gets the recommended vertical size in font design units for subscripts for this font.</summary>
        public short SubscriptYSize { get; }

        /// <summary>Gets the recommended horizontal offset in font design units for subscripts for this font.</summary>
        public short SubscriptXOffset { get; }

        /// <summary>Gets the recommended vertical offset in font design units from the baseline for subscripts for this font.</summary>
        public short SubscriptYOffset { get; }

        /// <summary>Gets the recommended horizontal size in font design units for superscripts for this font.</summary>
        public short SuperscriptXSize { get; }

        /// <summary>Gets the recommended vertical size in font design units for superscripts for this font.</summary>
        public short SuperscriptYSize { get; }

        /// <summary>Gets the recommended horizontal offset in font design units for superscripts for this font.</summary>
        public short SuperscriptXOffset { get; }

        /// <summary>Gets the recommended vertical offset in font design units from the baseline for superscripts for this font.</summary>
        public short SuperscriptYOffset { get; }

        /// <summary>Gets thickness of the strikeout stroke in font design units.</summary>
        public short StrikeoutSize { get; }

        /// <summary>Gets the position of the top of the strikeout stroke relative to the baseline in font design units.</summary>
        public short StrikeoutPosition { get; }

        /// <summary>Gets a classification of font-family design.</summary>
        public short FamilyClass { get; }

        /// <summary>Gets additional specifications are required for PANOSE to classify non-Latin character sets.</summary>
        public byte Panose1 { get; }

        /// <summary>Gets additional specifications are required for PANOSE to classify non-Latin character sets.</summary>
        public byte Panose2 { get; }

        /// <summary>Gets additional specifications are required for PANOSE to classify non-Latin character sets.</summary>
        public byte Panose3 { get; }

        /// <summary>Gets additional specifications are required for PANOSE to classify non-Latin character sets.</summary>
        public byte Panose4 { get; }

        /// <summary>Gets additional specifications are required for PANOSE to classify non-Latin character sets.</summary>
        public byte Panose5 { get; }

        /// <summary>Gets additional specifications are required for PANOSE to classify non-Latin character sets.</summary>
        public byte Panose6 { get; }

        /// <summary>Gets additional specifications are required for PANOSE to classify non-Latin character sets.</summary>
        public byte Panose7 { get; }

        /// <summary>Gets additional specifications are required for PANOSE to classify non-Latin character sets.</summary>
        public byte Panose8 { get; }

        /// <summary>Gets additional specifications are required for PANOSE to classify non-Latin character sets.</summary>
        public byte Panose9 { get; }

        /// <summary>Gets additional specifications are required for PANOSE to classify non-Latin character sets.</summary>
        public byte Panose10 { get; }

        /// <summary>Gets the Unicode blocks or ranges encompassed by the font file in 'cmap' subtables for platform 3, encoding ID 1 (Microsoft platform, Unicode BMP) and platform 3, encoding ID 10 (Microsoft platform, Unicode full repertoire).</summary>
        public uint UnicodeRange1 { get; }

        /// <summary>Gets the Unicode blocks or ranges encompassed by the font file in 'cmap' subtables for platform 3, encoding ID 1 (Microsoft platform, Unicode BMP) and platform 3, encoding ID 10 (Microsoft platform, Unicode full repertoire).</summary>
        public uint UnicodeRange2 { get; }

        /// <summary>Gets the Unicode blocks or ranges encompassed by the font file in 'cmap' subtables for platform 3, encoding ID 1 (Microsoft platform, Unicode BMP) and platform 3, encoding ID 10 (Microsoft platform, Unicode full repertoire).</summary>
        public uint UnicodeRange3 { get; }

        /// <summary>Gets the Unicode blocks or ranges encompassed by the font file in 'cmap' subtables for platform 3, encoding ID 1 (Microsoft platform, Unicode BMP) and platform 3, encoding ID 10 (Microsoft platform, Unicode full repertoire).</summary>
        public uint UnicodeRange4 { get; }

        /// <summary>Gets the four-character identifier for the vendor of the given type face.</summary>
        public string VendorID { get; }

        /// <summary>Gets Contains information concerning the nature of the font patterns.</summary>
        public ushort Selection { get; }

        /// <summary>Gets the minimum Unicode index (character code) in this font, according to the 'cmap' subtable for platform ID 3 and platform- specific encoding ID 0 or 1.</summary>
        public ushort FirstCharIndex { get; }

        /// <summary>Gets the maximum Unicode index (character code) in this font, according to the 'cmap' subtable for platform ID 3 and encoding ID 0 or 1.</summary>
        public ushort LastCharIndex { get; }

        /// <summary>Gets the typographic ascender for this font. This field should be combined with the sTypoDescender and sTypoLineGap values to determine default line spacing.</summary>
        public short TypoAscender { get; }

        /// <summary>Gets the typographic descender for this font. This field should be combined with the sTypoAscender and sTypoLineGap values to determine default line spacing. </summary>
        public short TypoDescender { get; }

        /// <summary>Gets the typographic line gap for this font. This field should be combined with the sTypoAscender and sTypoDescender values to determine default line spacing. </summary>
        public short TypoLineGap { get; }

        /// <summary>Gets the “Windows ascender” metric. </summary>
        public ushort WinAscent { get; }

        /// <summary>Gets the “Windows descender” metric.</summary>
        public ushort WinDescent { get; }

        /// <summary>Gets the code pages encompassed by the font file in the 'cmap' subtable for platform 3, encoding ID 1 (Microsoft platform, Unicode BMP). If the font file is encoding ID 0, then the Symbol Character Set bit should be set.</summary>
        public uint CodePageRange1 { get; }

        /// <summary>Gets the code pages encompassed by the font file in the 'cmap' subtable for platform 3, encoding ID 1 (Microsoft platform, Unicode BMP). If the font file is encoding ID 0, then the Symbol Character Set bit should be set.</summary>
        public uint CodePageRange2 { get; }

        /// <summary>Gets the distance between the baseline and the approximate height of non-ascending lowercase letters measured in FUnits.</summary>
        public short XHeight { get; }

        /// <summary>Gets the distance between the baseline and the approximate height of non-ascending lowercase letters measured in FUnits.</summary>
        public short CapHeight { get; }

        /// <summary>Gets the Unicode code point, in UTF-16 encoding, of a character that can be used for a default glyph if a requested character is not supported in the font.</summary>
        public ushort DefaultChar { get; }

        /// <summary>Gets the Unicode code point, in UTF-16 encoding, of a character that can be used as a default break character.</summary>
        public ushort BreakChar { get; }

        /// <summary>Gets the maximum length of a target glyph context for any feature in this font.</summary>
        public ushort MaxContext { get; }

        /// <summary>Gets the lower value of the size range for which this font has been designed.</summary>
        public ushort LowerOpticalPointSize { get; }

        /// <summary>Gets the upper value of the size range for which this font has been designed.</summary>
        public ushort UpperOpticalPointSize { get; }
    }
}
