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
            Offset = reader.Position;

            TableVersionNumberMajor = reader.ReadUInt16();
            TableVersionNumberMinor = reader.ReadUInt16();
            OffsetScriptList = reader.ReadUInt16();
            OffsetFeatureList = reader.ReadUInt16();
            OffsetLookupList = reader.ReadUInt16();

            reader.Position = Offset + OffsetScriptList;

            ScriptCount = reader.ReadUInt16();

            for (int i = 1; i <= ScriptCount; i++)
            {
                ScriptList.Add(new ScriptTable(reader));
            }

            foreach (var sr in ScriptList)
            {
                reader.Position = Offset + OffsetScriptList + sr.Offset;

                sr.DefaultLanguageSystemOffset = reader.ReadUInt16();
                sr.LanguageSystemCount = reader.ReadUInt16();

                if (sr.DefaultLanguageSystemOffset != 0)
                {
                    sr.LanguageSystemRecords.Add(new LanguageSystemRecord(true, sr.DefaultLanguageSystemOffset));
                }

                for (int i = 1; i <= sr.LanguageSystemCount; i++)
                {
                    sr.LanguageSystemRecords.Add(new LanguageSystemRecord(reader));
                }

                foreach (var ls in sr.LanguageSystemRecords)
                {
                    reader.Position = Offset + OffsetScriptList + sr.Offset + ls.Offset;
                    sr.LanguageSystemTables.Add(new LanguageSystemTable(reader, ls.Tag));
                }
            }

            reader.Position = Offset + OffsetFeatureList;

            FeatureCount = reader.ReadUInt16();

            for (int i = 1; i <= FeatureCount; i++)
            {
                FeatureList.Add(new FeatureTable(reader));
            }

            foreach (var ft in FeatureList)
            {
                reader.Position = Offset + OffsetFeatureList + ft.Offset;

                ft.FeatureParams = reader.ReadUInt16();
                ft.LookupCount = reader.ReadUInt16();

                for (int i = 1; i <= ft.LookupCount; i++)
                {
                    ft.LookupListIndex.Add(reader.ReadUInt16());
                }
            }

            reader.Position = Offset + OffsetLookupList;

            LookupCount = reader.ReadUInt16();

            for (int i = 1; i <= LookupCount; i++)
            {
                LookupList.Add(new LookupTable(reader));
            }

            foreach (var lt in LookupList)
            {
                reader.Position = Offset + OffsetLookupList + lt.Offset;

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

        /// <summary>Gets a major table version.</summary>
        public ushort TableVersionNumberMajor { get; }

        /// <summary>Gets a minor table version.</summary>
        public ushort TableVersionNumberMinor { get; }

        /// <summary>Gets the offset to ScriptList table.</summary>
        public ushort OffsetScriptList { get; }

        /// <summary>Gets the offset to FeatureList table.</summary>
        public ushort OffsetFeatureList { get; }

        /// <summary>Gets the offset to LookupList table.</summary>
        public ushort OffsetLookupList { get; }

        /// <summary>Gets the number of ScriptRecords.</summary>
        public ushort ScriptCount { get; }

        /// <summary>Gets the list of ScriptRecord.</summary>
        public List<ScriptTable> ScriptList { get; } = new List<ScriptTable>();

        /// <summary>Gets the number of FeatureRecords.</summary>
        public ushort FeatureCount { get; }

        /// <summary>Gets the list of ScriptRecord.</summary>
        public List<FeatureTable> FeatureList { get; } = new List<FeatureTable>();

        /// <summary>Gets the number of lookups in this table.</summary>
        public ushort LookupCount { get; }

        /// <summary>Gets the list of LookupTable.</summary>
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
