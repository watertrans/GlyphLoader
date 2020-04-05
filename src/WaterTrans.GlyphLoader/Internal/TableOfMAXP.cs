// <copyright file="TableOfMAXP.cs" company="WaterTrans">
// © 2020 WaterTrans
// </copyright>

namespace WaterTrans.GlyphLoader.Internal
{
    /// <summary>
    /// Table of MAXP.
    /// </summary>
    internal sealed class TableOfMAXP
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TableOfMAXP"/> class.
        /// </summary>
        /// <param name="reader">The <see cref="TypefaceReader"/>.</param>
        internal TableOfMAXP(TypefaceReader reader)
        {
            TableVersionNumberMajor = reader.ReadUInt16();
            TableVersionNumberMinor = reader.ReadUInt16();
            NumGlyphs = reader.ReadUInt16();

            if (TableVersionNumberMajor == 0 && TableVersionNumberMinor == 0x5000)
            {
                return;
            }

            MaxPoints = reader.ReadUInt16();
            MaxContours = reader.ReadUInt16();
            MaxCompositePoints = reader.ReadUInt16();
            MaxCompositeContours = reader.ReadUInt16();
            MaxZones = reader.ReadUInt16();
            MaxTwilightPoints = reader.ReadUInt16();
            MaxStorage = reader.ReadUInt16();
            MaxFunctionDefs = reader.ReadUInt16();
            MaxInstructionDefs = reader.ReadUInt16();
            MaxStackElements = reader.ReadUInt16();
            MaxSizeOfInstructions = reader.ReadUInt16();
            MaxComponentElements = reader.ReadUInt16();
            MaxComponentDepth = reader.ReadUInt16();
        }

        /// <summary>Gets a major table version.</summary>
        public ushort TableVersionNumberMajor { get; }

        /// <summary>Gets a minor table version.</summary>
        public ushort TableVersionNumberMinor { get; }

        /// <summary>Gets a number of glyphs in the font.</summary>
        public ushort NumGlyphs { get; }

        /// <summary>Gets a maximum points in a non-composite glyph.</summary>
        public ushort MaxPoints { get; }

        /// <summary>Gets a maximum contours in a non-composite glyph.</summary>
        public ushort MaxContours { get; }

        /// <summary>Gets a maximum points in a composite glyph.</summary>
        public ushort MaxCompositePoints { get; }

        /// <summary>Gets a maximum contours in a composite glyph.</summary>
        public ushort MaxCompositeContours { get; }

        /// <summary>Gets a maximum zones.</summary>
        /// <remarks>1 if instructions do not use the twilight zone (Z0), or 2 if instructions do use Z0; should be set to 2 in most cases.</remarks>
        public ushort MaxZones { get; }

        /// <summary>Gets a maximum points used in Z0.</summary>
        public ushort MaxTwilightPoints { get; }

        /// <summary>Gets a number of Storage Area locations.</summary>
        public ushort MaxStorage { get; }

        /// <summary>Gets a number of FDEFs, equal to the highest function number + 1.</summary>
        public ushort MaxFunctionDefs { get; }

        /// <summary>Gets a number of IDEFs.</summary>
        public ushort MaxInstructionDefs { get; }

        /// <summary>Gets a maximum stack depth across Font Program ('fpgm' table), CVT Program ('prep' table) and all glyph instructions (in the 'glyf' table).</summary>
        public ushort MaxStackElements { get; }

        /// <summary>Gets a maximum byte count for glyph instructions.</summary>
        public ushort MaxSizeOfInstructions { get; }

        /// <summary>Gets a maximum number of components referenced at “top level” for any composite glyph.</summary>
        public ushort MaxComponentElements { get; }

        /// <summary>Gets a maximum levels of recursion; 1 for simple components.</summary>
        public ushort MaxComponentDepth { get; }
    }
}
