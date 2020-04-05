// <copyright file="TableOfVHEA.cs" company="WaterTrans">
// © 2020 WaterTrans
// </copyright>

namespace WaterTrans.GlyphLoader.Internal
{
    /// <summary>
    /// Table of VHEA.
    /// </summary>
    internal sealed class TableOfVHEA
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TableOfVHEA"/> class.
        /// </summary>
        /// <param name="reader">The <see cref="TypefaceReader"/>.</param>
        internal TableOfVHEA(TypefaceReader reader)
        {
            TableVersionNumberMajor = reader.ReadUInt16();
            TableVersionNumberMinor = reader.ReadUInt16();
            Ascender = reader.ReadInt16();
            Descender = reader.ReadInt16();
            LineGap = reader.ReadInt16();
            AdvanceHeightMax = reader.ReadUInt16();
            MinTopSideBearing = reader.ReadInt16();
            MinBottomSideBearing = reader.ReadInt16();
            YMaxExtent = reader.ReadInt16();
            CaretSlopeRise = reader.ReadInt16();
            CaretSlopeRun = reader.ReadInt16();
            CaretOffset = reader.ReadInt16();
            Reserved1 = reader.ReadInt16();
            Reserved2 = reader.ReadInt16();
            Reserved3 = reader.ReadInt16();
            Reserved4 = reader.ReadInt16();
            MetricDataFormat = reader.ReadInt16();
            NumberOfVMetrics = reader.ReadUInt16();
        }

        /// <summary>Gets TableVersionNumberMajor.</summary>
        /// <remarks>0x00010000 for version 1.0.</remarks>
        public ushort TableVersionNumberMajor { get; }

        /// <summary>Gets TableVersionNumberMinor.</summary>
        /// <remarks>0x00010000 for version 1.0.</remarks>
        public ushort TableVersionNumberMinor { get; }

        /// <summary>Gets Ascender.</summary>
        /// <remarks>Distance in FUnits from the centerline to the previous line’s descent.</remarks>
        public short Ascender { get; }

        /// <summary>Gets Descender.</summary>
        /// <remarks>Distance in FUnits from the centerline to the next line’s ascent.</remarks>
        public short Descender { get; }

        /// <summary>Gets LineGap.</summary>
        /// <remarks>Reserved; set to 0.</remarks>
        public short LineGap { get; }

        /// <summary>Gets AdvanceHeightMax.</summary>
        /// <remarks>The maximum advance height measurement in FUnits found in the font. This value must be consistent with the entries in the vertical metrics table.</remarks>
        public ushort AdvanceHeightMax { get; }

        /// <summary>Gets MinTopSideBearing.</summary>
        /// <remarks>The minimum top sidebearing measurement found in the font, in FUnits. This value must be consistent with the entries in the vertical metrics table.</remarks>
        public short MinTopSideBearing { get; }

        /// <summary>Gets MinBottomSideBearing.</summary>
        /// <remarks>The minimum bottom sidebearing measurement found in the font, in FUnits. This value must be consistent with the entries in the vertical metrics table.</remarks>
        public short MinBottomSideBearing { get; }

        /// <summary>Gets YMaxExtent.</summary>
        /// <remarks>Defined as yMaxExtent=minTopSideBearing+(yMax-yMin).</remarks>
        public short YMaxExtent { get; }

        /// <summary>Gets CaretSlopeRise.</summary>
        /// <remarks>The value of the caretSlopeRise field divided by the value of the caretSlopeRun Field determines the slope of the caret. A value of 0 for the rise and a value of 1 for the run specifies a horizontal caret. A value of 1 for the rise and a value of 0 for the run specifies a vertical caret. Intermediate values are desirable for fonts whose glyphs are oblique or italic. For a vertical font, a horizontal caret is best.</remarks>
        public short CaretSlopeRise { get; }

        /// <summary>Gets CaretSlopeRun.</summary>
        /// <remarks>See the caretSlopeRise field. Value=1 for nonslanted vertical fonts.</remarks>
        public short CaretSlopeRun { get; }

        /// <summary>Gets CaretOffset.</summary>
        /// <remarks>The amount by which the highlight on a slanted glyph needs to be shifted away from the glyph in order to produce the best appearance. Set value equal to 0 for nonslanted fonts.</remarks>
        public short CaretOffset { get; }

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

        /// <summary>Gets MetricDataFormat.</summary>
        /// <remarks>0 for current format.</remarks>
        public short MetricDataFormat { get; }

        /// <summary>Gets NumberOfVMetrics.</summary>
        /// <remarks>Number of advance heights in the vertical metrics table.</remarks>
        public ushort NumberOfVMetrics { get; }
    }
}
