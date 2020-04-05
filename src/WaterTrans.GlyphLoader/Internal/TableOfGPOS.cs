// <copyright file="TableOfGPOS.cs" company="WaterTrans">
// © 2020 WaterTrans
// </copyright>

using System;
using System.Collections.Generic;
using WaterTrans.GlyphLoader.Internal.OpenType;
using WaterTrans.GlyphLoader.Internal.OpenType.GPOS;

namespace WaterTrans.GlyphLoader.Internal
{
    /// <summary>
    /// Table of GPOS.
    /// </summary>
    internal sealed class TableOfGPOS : CommonTable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TableOfGPOS"/> class.
        /// </summary>
        /// <param name="reader">The <see cref="TypefaceReader"/>.</param>
        internal TableOfGPOS(TypefaceReader reader)
            : base(reader)
        {
            foreach (var lt in LookupList)
            {
                foreach (ushort offset in lt.SubTableList)
                {
                    reader.Stream.Position = Offset + OffsetLookupList + lt.Offset + offset;

                    if (lt.LookupType == (ushort)LookupType.SingleAdjustment)
                    {
                        ushort format = reader.ReadUInt16();
                        switch (format)
                        {
                            case 1:
                            {
                                ushort coverageOffset = reader.ReadUInt16();
                                ushort valueFormat = reader.ReadUInt16();
                                var valueRecord = new ValueRecord(reader, valueFormat);
                                reader.Stream.Position = Offset + OffsetLookupList + lt.Offset + offset + coverageOffset;
                                List<ushort> coverages = ReadCoverage(reader);

                                for (int i = 0; i <= coverages.Count - 1; i++)
                                {
                                    lt.SingleAdjustmentList.Add(new SingleAdjustment(coverages[i], valueRecord));
                                }

                                break;
                            }
                            case 2:
                            {
                                ushort coverageOffset = reader.ReadUInt16();
                                ushort valueFormat = reader.ReadUInt16();
                                ushort valueCount = reader.ReadUInt16();
                                var valueRecords = new List<ValueRecord>();
                                for (int i = 1; i <= valueCount; i++)
                                {
                                    valueRecords.Add(new ValueRecord(reader, valueFormat));
                                }

                                reader.Stream.Position = Offset + OffsetLookupList + lt.Offset + offset + coverageOffset;
                                List<ushort> coverages = ReadCoverage(reader);
                                for (int i = 0; i <= coverages.Count - 1; i++)
                                {
                                    lt.SingleAdjustmentList.Add(new SingleAdjustment(coverages[i], valueRecords[i]));
                                }

                                break;
                            }
                        }
                    }
                    else if (lt.LookupType == (ushort)LookupType.PairAdjustment)
                    {
                        ushort format = reader.ReadUInt16();
                        switch (format)
                        {
                            case 1:
                            {
                                ushort coverageOffset = reader.ReadUInt16();
                                ushort valueFormat1 = reader.ReadUInt16();
                                ushort valueFormat2 = reader.ReadUInt16();
                                ushort pairsetCount = reader.ReadUInt16();
                                List<ushort> pairsetOffset = new List<ushort>();
                                for (int i = 1; i <= pairsetCount; i++)
                                {
                                    pairsetOffset.Add(reader.ReadUInt16());
                                }

                                reader.Stream.Position = Offset + OffsetLookupList + lt.Offset + offset + coverageOffset;
                                List<ushort> coverages = ReadCoverage(reader);

                                for (int i = 0; i <= pairsetOffset.Count - 1; i++)
                                {
                                    reader.Stream.Position = Offset + OffsetLookupList + lt.Offset + offset + pairsetOffset[i];
                                    ushort pairValueCount = reader.ReadUInt16();
                                    for (int j = 1; j <= pairValueCount; j++)
                                    {
                                        lt.PairAdjustmentList.Add(new PairAdjustment(
                                            coverages[i],
                                            reader.ReadUInt16(),
                                            new ValueRecord(reader, valueFormat1),
                                            new ValueRecord(reader, valueFormat2)));
                                    }
                                }

                                break;
                            }
                            case 2:
                            {
                                ushort coverageOffset = reader.ReadUInt16();
                                ushort valueFormat1 = reader.ReadUInt16();
                                ushort valueFormat2 = reader.ReadUInt16();
                                ushort classDefOffset1 = reader.ReadUInt16();
                                ushort classDefOffset2 = reader.ReadUInt16();
                                ushort classCount1 = reader.ReadUInt16();
                                ushort classCount2 = reader.ReadUInt16();
                                var classValueRecord1 = new ValueRecord[classCount1, classCount2];
                                var classValueRecord2 = new ValueRecord[classCount1, classCount2];
                                for (int i = 0; i <= classCount1 - 1; i++)
                                {
                                    for (int j = 0; j <= classCount2 - 1; j++)
                                    {
                                        classValueRecord1[i, j] = new ValueRecord(reader, valueFormat1);
                                        classValueRecord2[i, j] = new ValueRecord(reader, valueFormat2);
                                    }
                                }

                                reader.Stream.Position = Offset + OffsetLookupList + lt.Offset + offset + coverageOffset;
                                List<ushort> coverages = ReadCoverage(reader);
                                reader.Stream.Position = Offset + OffsetLookupList + lt.Offset + offset + classDefOffset1;
                                Dictionary<ushort, ushort> classDef1 = ReadClass(reader);
                                reader.Stream.Position = Offset + OffsetLookupList + lt.Offset + offset + classDefOffset2;
                                Dictionary<ushort, ushort> classDef2 = ReadClass(reader);

                                foreach (ushort gid1 in classDef1.Keys)
                                {
                                    foreach (ushort gid2 in classDef2.Keys)
                                    {
                                        ushort classId1 = classDef1[gid1];
                                        ushort classId2 = classDef2[gid2];
                                        if (classValueRecord1[classId1, classId2].IsEmpty == false | classValueRecord2[classId1, classId2].IsEmpty == false)
                                        {
                                            lt.PairAdjustmentList.Add(new PairAdjustment(
                                                gid1,
                                                gid2,
                                                classValueRecord1[classId1, classId2],
                                                classValueRecord2[classId1, classId2]));
                                        }
                                    }
                                }
                                break;
                            }
                        }
                    }
                    else
                    {
                        // TODO LookupType.CursiveAttachment; https://docs.microsoft.com/en-us/typography/opentype/spec/gpos#lookup-type-3-cursive-attachment-positioning-subtable
                        // TODO LookupType.MarkToBaseAttachment; https://docs.microsoft.com/en-us/typography/opentype/spec/gpos#lookup-type-4-mark-to-base-attachment-positioning-subtable
                        // TODO LookupType.MarkToLigatureAttachment; https://docs.microsoft.com/en-us/typography/opentype/spec/gpos#lookup-type-5-mark-to-ligature-attachment-positioning-subtable
                        // TODO LookupType.MarkToMarkAttachment; https://docs.microsoft.com/en-us/typography/opentype/spec/gpos#lookup-type-6-mark-to-mark-attachment-positioning-subtable
                        // TODO LookupType.ContextPositioning; https://docs.microsoft.com/en-us/typography/opentype/spec/gpos#lookup-type-7-contextual-positioning-subtables
                        // TODO LookupType.ChainedContextPositioning; https://docs.microsoft.com/en-us/typography/opentype/spec/gpos#lookuptype-8-chaining-contextual-positioning-subtable
                        // TODO LookupType.ExtensionPositioning; https://docs.microsoft.com/en-us/typography/opentype/spec/gpos#lookuptype-9-extension-positioning
                        continue;
                    }
                }
            }
        }

        private Dictionary<ushort, ushort> ReadClass(TypefaceReader reader)
        {
            var classGlyph = new Dictionary<ushort, ushort>();
            ushort classFormat = reader.ReadUInt16();
            if (classFormat == 1)
            {
                ushort glyphIndex = reader.ReadUInt16();
                ushort glyphCount = reader.ReadUInt16();
                for (int i = 1; i <= glyphCount; i++)
                {
                    classGlyph.Add(glyphIndex, reader.ReadUInt16());
                    glyphIndex = (ushort)(glyphIndex + Convert.ToUInt16(1));
                }
            }
            else
            {
                ushort rangeCount = reader.ReadUInt16();
                for (int i = 1; i <= rangeCount; i++)
                {
                    ushort startGlyphId = reader.ReadUInt16();
                    ushort endGlyphId = reader.ReadUInt16();
                    ushort classValue = reader.ReadUInt16();
                    for (ushort id = startGlyphId; id <= endGlyphId; id++)
                    {
                        classGlyph.Add(id, classValue);
                    }
                }
            }
            return classGlyph;
        }
    }
}
