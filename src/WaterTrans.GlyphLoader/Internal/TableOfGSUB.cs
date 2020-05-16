// <copyright file="TableOfGSUB.cs" company="WaterTrans">
// © 2020 WaterTrans
// </copyright>

using System.Collections.Generic;
using WaterTrans.GlyphLoader.Internal.OpenType;
using WaterTrans.GlyphLoader.Internal.OpenType.GSUB;

namespace WaterTrans.GlyphLoader.Internal
{
    /// <summary>
    /// Table of GSUB.
    /// </summary>
    internal sealed class TableOfGSUB : CommonTable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TableOfGSUB"/> class.
        /// </summary>
        /// <param name="reader">The <see cref="TypefaceReader"/>.</param>
        internal TableOfGSUB(TypefaceReader reader)
            : base(reader)
        {
            foreach (var lt in LookupList)
            {
                foreach (ushort offset in lt.SubTableList)
                {
                    reader.Position = Offset + OffsetLookupList + lt.Offset + offset;

                    if (lt.LookupType == (ushort)LookupType.SingleSubstitution)
                    {
                        ushort format = reader.ReadUInt16();
                        switch (format)
                        {
                            case 1:
                            {
                                ushort coverageOffset = reader.ReadUInt16();
                                ushort glyphID = reader.ReadUInt16();
                                reader.Position = Offset + OffsetLookupList + lt.Offset + offset + coverageOffset;
                                List<ushort> coverages = ReadCoverage(reader);
                                for (int i = 0; i <= coverages.Count - 1; i++)
                                {
                                    lt.SingleSubstitutionList.Add(new SingleSubstitution(coverages[i], (ushort)(coverages[i] + glyphID)));
                                }
                                break;
                            }
                            case 2:
                            {
                                ushort coverageOffset = reader.ReadUInt16();
                                ushort glyphCount = reader.ReadUInt16();
                                List<ushort> glyphList = new List<ushort>();
                                for (int i = 1; i <= glyphCount; i++)
                                {
                                    glyphList.Add(reader.ReadUInt16());
                                }

                                reader.Position = Offset + OffsetLookupList + lt.Offset + offset + coverageOffset;
                                List<ushort> coverages = ReadCoverage(reader);
                                for (int i = 0; i <= coverages.Count - 1; i++)
                                {
                                    lt.SingleSubstitutionList.Add(new SingleSubstitution(coverages[i], glyphList[i]));
                                }
                                break;
                            }
                        }
                    }
                    else if (lt.LookupType == (ushort)LookupType.MultipleSubstitution)
                    {
                        ushort format = reader.ReadUInt16();
                        ushort coverageOffset = reader.ReadUInt16();
                        ushort sequenceCount = reader.ReadUInt16();
                        List<ushort> sequenceList = new List<ushort>();
                        for (int i = 1; i <= sequenceCount; i++)
                        {
                            sequenceList.Add(reader.ReadUInt16());
                        }

                        reader.Position = Offset + OffsetLookupList + lt.Offset + offset + coverageOffset;
                        List<ushort> coverage = ReadCoverage(reader);
                        for (int i = 0; i <= coverage.Count - 1; i++)
                        {
                            var substitutionGlyphIndex = new List<ushort>();
                            ushort sequenceOffset = sequenceList[i];
                            reader.Position = Offset + OffsetLookupList + lt.Offset + offset + sequenceOffset;
                            ushort glyphCount = reader.ReadUInt16();
                            for (int j = 1; j <= glyphCount; j++)
                            {
                                substitutionGlyphIndex.Add(reader.ReadUInt16());
                            }
                            lt.MultipleSubstitutionList.Add(new MultipleSubstitution(coverage[i], substitutionGlyphIndex.ToArray()));
                        }
                    }
                    else if (lt.LookupType == (ushort)LookupType.AlternateSubstitution)
                    {
                        ushort format = reader.ReadUInt16();
                        ushort coverageOffset = reader.ReadUInt16();
                        ushort alternateSetCount = reader.ReadUInt16();
                        List<ushort> alternateSetList = new List<ushort>();
                        for (int i = 1; i <= alternateSetCount; i++)
                        {
                            alternateSetList.Add(reader.ReadUInt16());
                        }

                        reader.Position = Offset + OffsetLookupList + lt.Offset + offset + coverageOffset;
                        List<ushort> coverage = ReadCoverage(reader);
                        for (int i = 0; i <= coverage.Count - 1; i++)
                        {
                            var substitutionGlyphIndex = new List<ushort>();
                            ushort alternateSetOffset = alternateSetList[i];
                            reader.Position = Offset + OffsetLookupList + lt.Offset + offset + alternateSetOffset;
                            ushort glyphCount = reader.ReadUInt16();
                            for (int j = 1; j <= glyphCount; j++)
                            {
                                substitutionGlyphIndex.Add(reader.ReadUInt16());
                            }
                            lt.AlternateSubstitutionList.Add(new AlternateSubstitution(coverage[i], substitutionGlyphIndex.ToArray()));
                        }
                    }
                    else if (lt.LookupType == (ushort)LookupType.LigatureSubstitution)
                    {
                        ushort format = reader.ReadUInt16();
                        ushort coverageOffset = reader.ReadUInt16();
                        ushort ligatureSetCount = reader.ReadUInt16();
                        List<ushort> ligatureSetList = new List<ushort>();
                        for (int i = 1; i <= ligatureSetCount; i++)
                        {
                            ligatureSetList.Add(reader.ReadUInt16());
                        }

                        reader.Position = Offset + OffsetLookupList + lt.Offset + offset + coverageOffset;
                        List<ushort> coverages = ReadCoverage(reader);
                        for (int i = 0; i <= coverages.Count - 1; i++)
                        {
                            ushort ligatureSetOffset = ligatureSetList[i];
                            reader.Position = Offset + OffsetLookupList + lt.Offset + offset + ligatureSetOffset;
                            ushort ligatureCount = reader.ReadUInt16();
                            List<ushort> ligatureList = new List<ushort>();
                            for (int j = 1; j <= ligatureCount; j++)
                            {
                                ligatureList.Add(reader.ReadUInt16());
                            }
                            foreach (ushort ligatureOffset in ligatureList)
                            {
                                var glyphIndex = new List<ushort>();
                                glyphIndex.Add(coverages[i]);
                                reader.Position = Offset + OffsetLookupList + lt.Offset + offset + ligatureSetOffset + ligatureOffset;
                                ushort substitutionGlyphIndex = reader.ReadUInt16();
                                ushort componentCount = reader.ReadUInt16();
                                for (int k = 1; k <= componentCount - 1; k++)
                                {
                                    glyphIndex.Add(reader.ReadUInt16());
                                }
                                lt.LigatureSubstitutionList.Add(new LigatureSubstitution(glyphIndex.ToArray(), substitutionGlyphIndex));
                            }
                        }
                    }
                    else
                    {
                        // TODO LookupType.ContextualSubstitution; https://docs.microsoft.com/en-us/typography/opentype/spec/gsub#lookuptype-5-contextual-substitution-subtable
                        // TODO LookupType.ChainingContextualSubstitution; https://docs.microsoft.com/en-us/typography/opentype/spec/gsub#lookuptype-6-chaining-contextual-substitution-subtable
                        // TODO LookupType.ExtensionSubstitution; https://docs.microsoft.com/en-us/typography/opentype/spec/gsub#lookuptype-7-extension-substitution
                        // TODO LookupType.ReverseChainingContextualSingleSubstitution; https://docs.microsoft.com/en-us/typography/opentype/spec/gsub#lookuptype-8-reverse-chaining-contextual-single-substitution-subtable
                        continue;
                    }
                }
            }
        }
    }
}
