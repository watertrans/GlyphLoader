// <copyright file="TableOfHEAD.cs" company="WaterTrans">
// © 2020 WaterTrans
// </copyright>

using System;
using System.Collections.Generic;
using System.Text;

namespace WaterTrans.GlyphLoader.Internal
{
    /// <summary>
    /// Table of HEAD.
    /// </summary>
    internal sealed class TableOfHEAD
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TableOfHEAD"/> class.
        /// </summary>
        /// <param name="reader">The <see cref="TypefaceReader"/>.</param>
        internal TableOfHEAD(TypefaceReader reader)
        {
            TableVersionNumberMajor = reader.ReadUInt16();
            TableVersionNumberMinor = reader.ReadUInt16();
            FontRevisionMajor = reader.ReadUInt16();
            FontRevisionMinor = reader.ReadUInt16();
            CheckSumAdjustment = reader.ReadUInt32();
            MagicNumber = reader.ReadUInt32();
            Flags = reader.ReadUInt16();
            UnitsPerEm = reader.ReadUInt16();
            Created = reader.ReadInt64();
            Modified = reader.ReadInt64();
            XMin = reader.ReadInt16();
            YMin = reader.ReadInt16();
            XMax = reader.ReadInt16();
            YMax = reader.ReadInt16();
            MacStyle = reader.ReadUInt16();
            LowestRecPPEM = reader.ReadUInt16();
            FontDirectionHint = reader.ReadInt16();
            IndexToLocFormat = reader.ReadInt16();
            GlyphDataFormat = reader.ReadInt16();
        }

        /// <summary>Gets a major table version.</summary>
        public ushort TableVersionNumberMajor { get; }

        /// <summary>Gets a minor table version.</summary>
        public ushort TableVersionNumberMinor { get; }

        /// <summary>Gets a major font revision.</summary>
        public ushort FontRevisionMajor { get; }

        /// <summary>Gets a minor font revision.</summary>
        public ushort FontRevisionMinor { get; }

        /// <summary>Gets a check sum adjustment.</summary>
        /// <remarks>To compute:  set it to 0, sum the entire font as ULONG, then store 0xB1B0AFBA - sum.</remarks>
        public uint CheckSumAdjustment { get; }

        /// <summary>Gets a magic number.</summary>
        /// <remarks>Set to 0x5F0F3CF5.</remarks>
        public uint MagicNumber { get; }

        /// <summary>Gets a flags.</summary>
        /// <remarks>Bit 0 - baseline for font at y=0;Bit 1 - left sidebearing at x=0;Bit 2 - instructions may depend on point size;Bit 3 - force ppem to integer values for all internal scaler math; may use fractional ppem sizes if this bit is clear;Bit 4 - instructions may alter advance width (the advance widths might not scale linearly);Note: All other bits must be zero.</remarks>
        public ushort Flags { get; }

        /// <summary>Gets the UnitsPerEm.</summary>
        /// <remarks>Valid range is from 16 to 16384.</remarks>
        public ushort UnitsPerEm { get; }

        /// <summary>Gets a created date.</summary>
        /// <remarks>International date (8-byte field).</remarks>
        public long Created { get; }

        /// <summary>Gets a modified date.</summary>
        /// <remarks>International date (8-byte field).</remarks>
        public long Modified { get; }

        /// <summary>Gets a xMin.</summary>
        /// <remarks>For all glyph bounding boxes.</remarks>
        public long XMin { get; }

        /// <summary>Gets a yMin.</summary>
        /// <remarks>For all glyph bounding boxes.</remarks>
        public long YMin { get; }

        /// <summary>Gets a xMax.</summary>
        /// <remarks>For all glyph bounding boxes.</remarks>
        public long XMax { get; }

        /// <summary>Gets a yMax.</summary>
        /// <remarks>For all glyph bounding boxes.</remarks>
        public long YMax { get; }

        /// <summary>Gets the MacStyle.</summary>
        /// <remarks>Bit 0 bold (if set to 1); Bit 1 italic (if set to 1)Bits 2-15 reserved (set to 0).</remarks>
        public ushort MacStyle { get; }

        /// <summary>Gets a smallest readable size in pixels.</summary>
        public ushort LowestRecPPEM { get; }

        /// <summary>Gets a font direction hint.</summary>
        /// <remarks>0 Fully mixed directional glyphs; 1 Only strongly left to right; 2 Like 1 but also contains neutrals ; -1 Only strongly right to left; -2 Like -1 but also contains neutrals.</remarks>
        public short FontDirectionHint { get; }

        /// <summary>Gets the IndexToLocFormat.</summary>
        /// <remarks>0 for short offsets, 1 for long.</remarks>
        public short IndexToLocFormat { get; }

        /// <summary>Gets the GlyphDataFormat.</summary>
        /// <remarks>0 for current format.</remarks>
        public short GlyphDataFormat { get; }
    }
}
