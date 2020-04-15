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

        /// <summary>Gets a major table version.</summary>
        public ushort TableVersionNumberMajor { get; }

        /// <summary>Gets a minor table version.</summary>
        public ushort TableVersionNumberMinor { get; }

        /// <summary>Gets the typographic ascent.</summary>
        public short Ascender { get; }

        /// <summary>Gets the typographic descent.</summary>
        public short Descender { get; }

        /// <summary>Gets the line gap.</summary>
        /// <remarks>Typographic line gap. Negative LineGap values are treated as zero in Windows 3.1, System 6, and System 7.</remarks>
        public short LineGap { get; }

        /// <summary>Gets the maximum advance width value in ‘hmtx’ table.</summary>
        public ushort AdvanceWidthMax { get; }

        /// <summary>Gets the minimum left sidebearing value in ‘hmtx’ table.</summary>
        public short MinLeftSideBearing { get; }

        /// <summary>Gets the minimum right sidebearing value; calculated as Min(aw - lsb - (xMax - xMin)).</summary>
        public short MinRightSideBearing { get; }

        /// <summary>Gets the xMaxExtent.</summary>
        /// <remarks>Max(lsb + (xMax - xMin)).</remarks>
        public short XMaxExtent { get; }

        /// <summary>Gets the caretSlopeRise.</summary>
        /// <remarks>Used to calculate the slope of the cursor (rise/run); 1 for vertical.</remarks>
        public short CaretSlopeRise { get; }

        /// <summary>Gets the caretSlopeRun.</summary>
        /// <remarks>0 for vertical.</remarks>
        public short CaretSlopeRun { get; }

        /// <summary>Gets a reserved1.</summary>
        /// <remarks>Reserved.</remarks>
        public short Reserved1 { get; }

        /// <summary>Gets a reserved2.</summary>
        /// <remarks>Reserved.</remarks>
        public short Reserved2 { get; }

        /// <summary>Gets a reserved3.</summary>
        /// <remarks>Reserved.</remarks>
        public short Reserved3 { get; }

        /// <summary>Gets a reserved4.</summary>
        /// <remarks>Reserved.</remarks>
        public short Reserved4 { get; }

        /// <summary>Gets a reserved5.</summary>
        /// <remarks>Reserved.</remarks>
        public short Reserved5 { get; }

        /// <summary>Gets the metric data format.</summary>
        /// <remarks>0 for current format.</remarks>
        public short MetricDataFormat { get; }

        /// <summary>Gets the number of hMetrics.</summary>
        /// <remarks>Number of hMetric entries in ‘hmtx’table; may be smaller than the total number of glyphs in the font.</remarks>
        public ushort NumberOfHMetrics { get; }
    }
}
