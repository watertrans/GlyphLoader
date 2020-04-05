// <copyright file="TableOfHHEA.cs" company="WaterTrans">
// © 2020 WaterTrans
// </copyright>

using System;
using System.Collections.Generic;
using System.Text;

namespace WaterTrans.GlyphLoader.Internal
{
    /// <summary>
    /// Table of HHEA.
    /// </summary>
    internal sealed class TableOfHHEA
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TableOfHHEA"/> class.
        /// </summary>
        /// <param name="reader">The <see cref="TypefaceReader"/>.</param>
        internal TableOfHHEA(TypefaceReader reader)
        {
            TableVersionNumberMajor = reader.ReadUInt16();
            TableVersionNumberMinor = reader.ReadUInt16();
            Ascender = reader.ReadInt16();
            Descender = reader.ReadInt16();
            LineGap = reader.ReadInt16();
            AdvanceWidthMax = reader.ReadUInt16();
            MinLeftSideBearing = reader.ReadInt16();
            MinRightSideBearing = reader.ReadInt16();
            XMaxExtent = reader.ReadInt16();
            CaretSlopeRise = reader.ReadInt16();
            CaretSlopeRun = reader.ReadInt16();
            Reserved1 = reader.ReadInt16();
            Reserved2 = reader.ReadInt16();
            Reserved3 = reader.ReadInt16();
            Reserved4 = reader.ReadInt16();
            Reserved5 = reader.ReadInt16();
            MetricDataFormat = reader.ReadInt16();
            NumberOfHMetrics = reader.ReadUInt16();
        }

        /// <summary>Gets TableVersionNumberMajor.</summary>
        /// <remarks>0x00010000 for version 1.0.</remarks>
        public ushort TableVersionNumberMajor { get; }

        /// <summary>Gets TableVersionNumberMinor.</summary>
        /// <remarks>0x00010000 for version 1.0.</remarks>
        public ushort TableVersionNumberMinor { get; }

        /// <summary>Gets Ascender.</summary>
        /// <remarks>Typographic ascent.</remarks>
        public short Ascender { get; }

        /// <summary>Gets Descender.</summary>
        /// <remarks>Typographic descent.</remarks>
        public short Descender { get; }

        /// <summary>Gets LineGap.</summary>
        /// <remarks>Typographic line gap. Negative LineGap values are treated as zero in Windows 3.1, System 6, and System 7.</remarks>
        public short LineGap { get; }

        /// <summary>Gets AdvanceWidthMax.</summary>
        /// <remarks>Maximum advance width value in ‘hmtx’ table.</remarks>
        public ushort AdvanceWidthMax { get; }

        /// <summary>Gets MinLeftSideBearing.</summary>
        /// <remarks>Minimum left sidebearing value in ‘hmtx’ table.</remarks>
        public short MinLeftSideBearing { get; }

        /// <summary>Gets MinRightSideBearing.</summary>
        /// <remarks>Minimum right sidebearing value; calculated as Min(aw - lsb - (xMax - xMin)).</remarks>
        public short MinRightSideBearing { get; }

        /// <summary>Gets XMaxExtent.</summary>
        /// <remarks>Max(lsb + (xMax - xMin)).</remarks>
        public short XMaxExtent { get; }

        /// <summary>Gets CaretSlopeRise.</summary>
        /// <remarks>Used to calculate the slope of the cursor (rise/run); 1 for vertical.</remarks>
        public short CaretSlopeRise { get; }

        /// <summary>Gets CaretSlopeRun.</summary>
        /// <remarks>0 for vertical.</remarks>
        public short CaretSlopeRun { get; }

        /// <summary>Gets Reserved1.</summary>
        /// <remarks>Reserved.</remarks>
        public short Reserved1 { get; }

        /// <summary>Gets Reserved2.</summary>
        /// <remarks>Reserved.</remarks>
        public short Reserved2 { get; }

        /// <summary>Gets Reserved3.</summary>
        /// <remarks>Reserved.</remarks>
        public short Reserved3 { get; }

        /// <summary>Gets Reserved4.</summary>
        /// <remarks>Reserved.</remarks>
        public short Reserved4 { get; }

        /// <summary>Gets Reserved5.</summary>
        /// <remarks>Reserved.</remarks>
        public short Reserved5 { get; }

        /// <summary>Gets MetricDataFormat.</summary>
        /// <remarks>0 for current format.</remarks>
        public short MetricDataFormat { get; }

        /// <summary>Gets NumberOfHMetrics.</summary>
        /// <remarks>Number of hMetric entries in ‘hmtx’table; may be smaller than the total number of glyphs in the font.</remarks>
        public ushort NumberOfHMetrics { get; }
    }
}
