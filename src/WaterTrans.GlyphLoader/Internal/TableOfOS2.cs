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
            CodePageRange1 = reader.ReadUInt32();
            CodePageRange2 = reader.ReadUInt32();
        }

        /// <summary>Gets Version.</summary>
        /// <remarks>OS/2 table version number.</remarks>
        public ushort Version { get; }

        /// <summary>Gets AvgCharWidth.</summary>
        /// <remarks>Average weighted escapement.</remarks>
        public short AvgCharWidth { get; }

        /// <summary>Gets WeightClass.</summary>
        /// <remarks>Weight class.</remarks>
        public ushort WeightClass { get; }

        /// <summary>Gets WidthClass.</summary>
        /// <remarks>Width class.</remarks>
        public ushort WidthClass { get; }

        /// <summary>Gets Type.</summary>
        /// <remarks>Type flags.</remarks>
        public short Type { get; }

        /// <summary>Gets SubscriptXSize.</summary>
        /// <remarks>Subscript horizontal font size.</remarks>
        public short SubscriptXSize { get; }

        /// <summary>Gets SubscriptYSize.</summary>
        /// <remarks>Subscript vertical font size.</remarks>
        public short SubscriptYSize { get; }

        /// <summary>Gets SubscriptXOffset.</summary>
        /// <remarks>Subscript x offset.</remarks>
        public short SubscriptXOffset { get; }

        /// <summary>Gets SubscriptYOffset.</summary>
        /// <remarks>Subscript y offset.</remarks>
        public short SubscriptYOffset { get; }

        /// <summary>Gets SuperscriptXSize.</summary>
        /// <remarks>Superscript horizontal font size.</remarks>
        public short SuperscriptXSize { get; }

        /// <summary>Gets SuperscriptYSize.</summary>
        /// <remarks>Superscript vertical font size.</remarks>
        public short SuperscriptYSize { get; }

        /// <summary>Gets SuperscriptXOffset.</summary>
        /// <remarks>Superscript x offset.</remarks>
        public short SuperscriptXOffset { get; }

        /// <summary>Gets SuperscriptYOffset.</summary>
        /// <remarks>Superscript y offset.</remarks>
        public short SuperscriptYOffset { get; }

        /// <summary>Gets StrikeoutSize.</summary>
        /// <remarks>Strikeout size.</remarks>
        public short StrikeoutSize { get; }

        /// <summary>Gets StrikeoutPosition.</summary>
        /// <remarks>Strikeout position.</remarks>
        public short StrikeoutPosition { get; }

        /// <summary>Gets FamilyClass.</summary>
        /// <remarks>Font-family class and subclass.</remarks>
        public short FamilyClass { get; }

        /// <summary>Gets Panose1.</summary>
        /// <remarks>PANOSE classification number.</remarks>
        public byte Panose1 { get; }

        /// <summary>Gets Panose2.</summary>
        /// <remarks>PANOSE classification number.</remarks>
        public byte Panose2 { get; }

        /// <summary>Gets Panose3.</summary>
        /// <remarks>PANOSE classification number.</remarks>
        public byte Panose3 { get; }

        /// <summary>Gets Panose4.</summary>
        /// <remarks>PANOSE classification number.</remarks>
        public byte Panose4 { get; }

        /// <summary>Gets Panose5.</summary>
        /// <remarks>PANOSE classification number.</remarks>
        public byte Panose5 { get; }

        /// <summary>Gets Panose6.</summary>
        /// <remarks>PANOSE classification number.</remarks>
        public byte Panose6 { get; }

        /// <summary>Gets Panose7.</summary>
        /// <remarks>PANOSE classification number.</remarks>
        public byte Panose7 { get; }

        /// <summary>Gets Panose8.</summary>
        /// <remarks>PANOSE classification number.</remarks>
        public byte Panose8 { get; }

        /// <summary>Gets Panose9.</summary>
        /// <remarks>PANOSE classification number.</remarks>
        public byte Panose9 { get; }

        /// <summary>Gets Panose10.</summary>
        /// <remarks>PANOSE classification number.</remarks>
        public byte Panose10 { get; }

        /// <summary>Gets UnicodeRange1.</summary>
        /// <remarks>Unicode Character Range.</remarks>
        public uint UnicodeRange1 { get; }

        /// <summary>Gets UnicodeRange2.</summary>
        /// <remarks>Unicode Character Range.</remarks>
        public uint UnicodeRange2 { get; }

        /// <summary>Gets UnicodeRange3.</summary>
        /// <remarks>Unicode Character Range.</remarks>
        public uint UnicodeRange3 { get; }

        /// <summary>Gets UnicodeRange4.</summary>
        /// <remarks>Unicode Character Range.</remarks>
        public uint UnicodeRange4 { get; }

        /// <summary>Gets VendorID.</summary>
        /// <remarks>Font Vendor Identification.</remarks>
        public string VendorID { get; }

        /// <summary>Gets Selection.</summary>
        /// <remarks>Font selection flags.</remarks>
        public ushort Selection { get; }

        /// <summary>Gets FirstCharIndex.</summary>
        /// <remarks>The minimum Unicode index (character code) in this font.</remarks>
        public ushort FirstCharIndex { get; }

        /// <summary>Gets LastCharIndex.</summary>
        /// <remarks>The maximum Unicode index (character code) in this font.</remarks>
        public ushort LastCharIndex { get; }

        /// <summary>Gets TypoAscender.</summary>
        /// <remarks>The typographic ascender for this font.</remarks>
        public short TypoAscender { get; }

        /// <summary>Gets TypoDescender.</summary>
        /// <remarks>The typographic descender for this font.</remarks>
        public short TypoDescender { get; }

        /// <summary>Gets TypoLineGap.</summary>
        /// <remarks>The typographic line gap for this font.</remarks>
        public short TypoLineGap { get; }

        /// <summary>Gets WinAscent.</summary>
        /// <remarks>The ascender metric for Windows.</remarks>
        public ushort WinAscent { get; }

        /// <summary>Gets WinDescent.</summary>
        /// <remarks>The descender metric for Windows.</remarks>
        public ushort WinDescent { get; }

        /// <summary>Gets CodePageRange1.</summary>
        /// <remarks>Code Page Character Range.</remarks>
        public uint CodePageRange1 { get; }

        /// <summary>Gets CodePageRange2.</summary>
        /// <remarks>Code Page Character Range.</remarks>
        public uint CodePageRange2 { get; }
    }
}
