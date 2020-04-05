// <copyright file="CommonTable.cs" company="WaterTrans">
// © 2020 WaterTrans
// </copyright>

using System.Collections.Generic;

namespace WaterTrans.GlyphLoader.Internal.OpenType
{
    /// <summary>
    /// The OpenType common table.
    /// </summary>
    internal class CommonTable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CommonTable"/> class.
        /// </summary>
        /// <param name="reader">The <see cref="TypefaceReader"/>.</param>
        internal CommonTable(TypefaceReader reader)
        {
            Offset = reader.Stream.Position;

            TableVersionNumberMajor = reader.ReadUInt16();
            TableVersionNumberMinor = reader.ReadUInt16();
            OffsetScriptList = reader.ReadUInt16();
            OffsetFeatureList = reader.ReadUInt16();
            OffsetLookupList = reader.ReadUInt16();

            reader.Stream.Position = Offset + OffsetScriptList;

            ScriptCount = reader.ReadUInt16();

            for (int i = 1; i <= ScriptCount; i++)
            {
                ScriptList.Add(new ScriptTable(reader));
            }

            foreach (var sr in ScriptList)
            {
                reader.Stream.Position = Offset + OffsetScriptList + sr.Offset;

                sr.DefaultLanguageSystemOffset = reader.ReadUInt16();
                sr.LanguageSystemOffset = reader.ReadUInt16();

                if (sr.DefaultLanguageSystemOffset != 0)
                {
                    sr.LanguageSystemRecords.Add(new LanguageSystemRecord(true, sr.DefaultLanguageSystemOffset));
                }

                for (int i = 1; i <= sr.LanguageSystemOffset; i++)
                {
                    sr.LanguageSystemRecords.Add(new LanguageSystemRecord(reader));
                }

                foreach (var ls in sr.LanguageSystemRecords)
                {
                    reader.Stream.Position = Offset + OffsetScriptList + sr.Offset + ls.Offset;
                    sr.LanguageSystemTables.Add(new LanguageSystemTable(reader));
                }
            }

            reader.Stream.Position = Offset + OffsetFeatureList;

            FeatureCount = reader.ReadUInt16();

            for (int i = 1; i <= FeatureCount; i++)
            {
                FeatureList.Add(new FeatureTable(reader));
            }

            foreach (var ft in FeatureList)
            {
                reader.Stream.Position = Offset + OffsetFeatureList + ft.Offset;

                ft.FeatureParams = reader.ReadUInt16();
                ft.LookupCount = reader.ReadUInt16();

                for (int i = 1; i <= ft.LookupCount; i++)
                {
                    ft.LookupListIndex.Add(reader.ReadUInt16());
                }
            }

            reader.Stream.Position = Offset + OffsetLookupList;

            LookupCount = reader.ReadUInt16();

            for (int i = 1; i <= LookupCount; i++)
            {
                LookupList.Add(new LookupTable(reader));
            }

            foreach (var lt in LookupList)
            {
                reader.Stream.Position = Offset + OffsetLookupList + lt.Offset;

                lt.LookupType = reader.ReadUInt16();
                lt.LookupFlag = reader.ReadUInt16();
                lt.SubTableCount = reader.ReadUInt16();

                for (int i = 1; i <= lt.SubTableCount; i++)
                {
                    lt.SubTableList.Add(reader.ReadUInt16());
                }
            }
        }

        /// <summary>Gets offset.</summary>
        public long Offset { get; }

        /// <summary>Gets TableVersionNumberMajor.</summary>
        /// <remarks>0x00010000 for version 1.0.</remarks>
        public ushort TableVersionNumberMajor { get; }

        /// <summary>Gets TableVersionNumberMinor.</summary>
        /// <remarks>0x00010000 for version 1.0.</remarks>
        public ushort TableVersionNumberMinor { get; }

        /// <summary>Gets OffsetScriptList.</summary>
        /// <remarks>Offset to ScriptList table.</remarks>
        public ushort OffsetScriptList { get; }

        /// <summary>Gets OffsetFeatureList.</summary>
        /// <remarks>Offset to FeatureList table.</remarks>
        public ushort OffsetFeatureList { get; }

        /// <summary>Gets OffsetLookupList.</summary>
        /// <remarks>Offset to LookupList table.</remarks>
        public ushort OffsetLookupList { get; }

        /// <summary>Gets ScriptCount.</summary>
        /// <remarks>Number of ScriptRecords.</remarks>
        public ushort ScriptCount { get; }

        /// <summary>Gets ScriptList.</summary>
        /// <remarks>List of ScriptRecord.</remarks>
        public List<ScriptTable> ScriptList { get; } = new List<ScriptTable>();

        /// <summary>Gets FeatureCount.</summary>
        /// <remarks>Number of FeatureRecords.</remarks>
        public ushort FeatureCount { get; }

        /// <summary>Gets FeatureList.</summary>
        /// <remarks>List of ScriptRecord.</remarks>
        public List<FeatureTable> FeatureList { get; } = new List<FeatureTable>();

        /// <summary>Gets LookupCount.</summary>
        /// <remarks>Number of lookups in this table.</remarks>
        public ushort LookupCount { get; }

        /// <summary>Gets LookupList.</summary>
        /// <remarks>List of LookupTable.</remarks>
        public List<LookupTable> LookupList { get; } = new List<LookupTable>();

        /// <summary>
        /// Read coverage table.
        /// </summary>
        /// <param name="reader">The <see cref="TypefaceReader"/>.</param>
        /// <returns>The list of ushort.</returns>
        protected List<ushort> ReadCoverage(TypefaceReader reader)
        {
            var coverage = new List<ushort>();
            ushort coverageFormat = reader.ReadUInt16();

            if (coverageFormat == 1)
            {
                ushort glyphCount = reader.ReadUInt16();
                for (int i = 1; i <= glyphCount; i++)
                {
                    coverage.Add(reader.ReadUInt16());
                }
            }
            else
            {
                ushort rangeCount = reader.ReadUInt16();
                for (int i = 1; i <= rangeCount; i++)
                {
                    ushort startGlyphId = reader.ReadUInt16();
                    ushort endGlyphId = reader.ReadUInt16();
                    ushort startCoverageIndex = reader.ReadUInt16();

                    for (ushort id = startGlyphId; id <= endGlyphId; id++)
                    {
                        coverage.Add(id);
                    }
                }
            }
            return coverage;
        }
    }
}
