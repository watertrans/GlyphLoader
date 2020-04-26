// <copyright file="TableOfCMAP.cs" company="WaterTrans">
// © 2020 WaterTrans
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using WaterTrans.GlyphLoader.Internal.SFNT;

namespace WaterTrans.GlyphLoader.Internal
{
    /// <summary>
    /// Table of CMAP.
    /// </summary>
    internal sealed class TableOfCMAP
    {
        private readonly Dictionary<int, ushort> _glyphMap;

        /// <summary>
        /// Initializes a new instance of the <see cref="TableOfCMAP"/> class.
        /// </summary>
        /// <param name="reader">The <see cref="TypefaceReader"/>.</param>
        internal TableOfCMAP(TypefaceReader reader)
        {
            long basePosition = reader.Stream.Position;
            TableVersionNumber = reader.ReadUInt16();
            NumTables = reader.ReadUInt16();

            for (int i = 1; i <= NumTables; i++)
            {
                EncodingRecords.Add(new EncodingRecord(reader));
            }

            foreach (var record in EncodingRecords)
            {
                reader.Stream.Position = basePosition + record.Offset;
                ushort format = reader.ReadUInt16();

                if (format == 0) // Byte encoding table
                {
                    // TODO Not tested. Please provide the font file.
                    ushort length = reader.ReadUInt16();
                    ushort language = reader.ReadUInt16();
                    for (int i = 1; i <= 256; i++)
                    {
                        record.GlyphMap[i] = reader.ReadByte();
                    }
                }
                else if (format == 2) // High-byte mapping through table
                {
                    ushort length = reader.ReadUInt16();
                    ushort language = reader.ReadUInt16();
                    var subHeaderKeys = new List<ushort>();
                    ushort key;
                    int maxIndex = 0;
                    for (int i = 1; i <= 256; i++)
                    {
                        key = reader.ReadUInt16();
                        if (key != 0 && key / 8 > maxIndex)
                        {
                            maxIndex = key / 8;
                        }
                        subHeaderKeys.Add(key);
                    }
                    var subHeaders = new List<Tuple<ushort, ushort, short, ushort>>();
                    for (int i = 0; i <= maxIndex; i++)
                    {
                        subHeaders.Add(Tuple.Create(reader.ReadUInt16(), reader.ReadUInt16(), reader.ReadInt16(), reader.ReadUInt16()));
                    }

                    // TODO Not tested. Please provide the font file.
                }
                else if (format == 4) // Segment mapping to delta values
                {
                    ushort length = reader.ReadUInt16();
                    ushort language = reader.ReadUInt16();
                    ushort segCountX2 = reader.ReadUInt16();
                    ushort searchRange = reader.ReadUInt16();
                    ushort entrySelector = reader.ReadUInt16();
                    ushort rangeShift = reader.ReadUInt16();
                    var endCodes = new List<ushort>();
                    int segCount = segCountX2 / 2;
                    for (int i = 1; i <= segCount; i++)
                    {
                        endCodes.Add(reader.ReadUInt16());
                    }
                    ushort reservedPad = reader.ReadUInt16();
                    var startCodes = new List<ushort>();
                    for (int i = 1; i <= segCount; i++)
                    {
                        startCodes.Add(reader.ReadUInt16());
                    }
                    var idDeltas = new List<short>();
                    for (int i = 1; i <= segCount; i++)
                    {
                        idDeltas.Add(reader.ReadInt16());
                    }
                    var idRangeOffsets = new List<ushort>();
                    for (int i = 1; i <= segCount; i++)
                    {
                        idRangeOffsets.Add(reader.ReadUInt16());
                    }

                    long currentPosition = reader.Stream.Position;
                    for (int i = 0; i < segCount; i++)
                    {
                        ushort start = startCodes[i];
                        ushort end = endCodes[i];
                        short delta = idDeltas[i];
                        ushort rangeOffset = idRangeOffsets[i];
                        if (start != 65535 && end != 65535)
                        {
                            for (int j = start; j <= end; j++)
                            {
                                if (rangeOffset == 0)
                                {
                                    record.GlyphMap[j] = (ushort)((j + delta) % 65536);
                                }
                                else
                                {
                                    long glyphOffset = currentPosition + (((rangeOffset / 2) + (j - start) + (i - segCount)) * 2);
                                    reader.Stream.Position = glyphOffset;
                                    int glyphIndex = reader.ReadUInt16();
                                    if (glyphIndex != 0)
                                    {
                                        glyphIndex += delta;
                                        glyphIndex %= 65536;
                                        record.GlyphMap[j] = (ushort)glyphIndex;
                                    }
                                }
                            }
                        }
                    }
                }
                else if (format == 6) // Trimmed table mapping
                {
                    ushort length = reader.ReadUInt16();
                    ushort language = reader.ReadUInt16();
                    ushort firstCode = reader.ReadUInt16();
                    ushort entryCount = reader.ReadUInt16();

                    for (int i = 0; i < entryCount; i++)
                    {
                        record.GlyphMap[firstCode + i] = reader.ReadUInt16();
                    }
                }
                else if (format == 8) // mixed 16-bit and 32-bit coverage
                {
                    ushort reserved = reader.ReadUInt16();
                    uint length = reader.ReadUInt32();
                    uint language = reader.ReadUInt32();
                    var is32 = new List<byte>();
                    for (int i = 0; i < 8192; i++)
                    {
                        is32.Add(reader.ReadByte());
                    }
                    uint numGroups = reader.ReadUInt32();
                    var sequentialMapGroups = new List<Tuple<uint, uint, uint>>();
                    for (int i = 0; i < numGroups; i++)
                    {
                        sequentialMapGroups.Add(Tuple.Create(reader.ReadUInt32(), reader.ReadUInt32(), reader.ReadUInt32()));
                    }

                    // TODO Not tested. Please provide the font file.
                }
                else if (format == 10) // Trimmed array
                {
                    // This format is not widely used and is not supported by Microsoft.
                }
                else if (format == 12) // Segmented coverage
                {
                    ushort reserved = reader.ReadUInt16();
                    uint length = reader.ReadUInt32();
                    uint language = reader.ReadUInt32();
                    uint numGroups = reader.ReadUInt32();
                    var sequentialMapGroups = new List<Tuple<uint, uint, uint>>();
                    for (int i = 0; i < numGroups; i++)
                    {
                        sequentialMapGroups.Add(Tuple.Create(reader.ReadUInt32(), reader.ReadUInt32(), reader.ReadUInt32()));
                    }

                    for (int i = 0; i < sequentialMapGroups.Count; i++)
                    {
                        uint start = sequentialMapGroups[i].Item1;
                        uint end = sequentialMapGroups[i].Item2;
                        uint glyphIndex = sequentialMapGroups[i].Item3;
                        for (uint j = start; j <= end; j++)
                        {
                            record.GlyphMap[(int)j] = (ushort)(glyphIndex + (j - start));
                        }
                    }
                }
                else if (format == 13) // Many-to-one range mappings
                {
                    ushort reserved = reader.ReadUInt16();
                    uint length = reader.ReadUInt32();
                    uint language = reader.ReadUInt32();
                    uint numGroups = reader.ReadUInt32();
                    var constantMapGroups = new List<Tuple<uint, uint, uint>>();
                    for (int i = 0; i < numGroups; i++)
                    {
                        constantMapGroups.Add(Tuple.Create(reader.ReadUInt32(), reader.ReadUInt32(), reader.ReadUInt32()));
                    }

                    for (int i = 0; i < constantMapGroups.Count; i++)
                    {
                        uint start = constantMapGroups[i].Item1;
                        uint end = constantMapGroups[i].Item2;
                        uint glyphIndex = constantMapGroups[i].Item3;
                        for (uint j = start; j <= end; j++)
                        {
                            record.GlyphMap[(int)j] = (ushort)glyphIndex;
                        }
                    }
                }
                else if (format == 14) // Unicode Variation Sequences
                {
                    uint length = reader.ReadUInt32();
                    uint numVarSelectorRecords = reader.ReadUInt32();
                    var variationSelectors = new List<Tuple<uint, uint, uint>>();
                    for (int i = 0; i < numVarSelectorRecords; i++)
                    {
                        variationSelectors.Add(Tuple.Create(reader.ReadUInt24(), reader.ReadUInt32(), reader.ReadUInt32()));
                    }

                    for (int i = 0; i < variationSelectors.Count; i++)
                    {
                        // [SKIP] defaultUVSOffset
                        // if (variationSelectors[i].Item2 > 0)
                        // {
                        //     reader.Stream.Position = basePosition + record.Offset + variationSelectors[i].Item2;
                        //     uint numUnicodeValueRanges = reader.ReadUInt32();
                        //     var unicodeRanges = new List<Tuple<uint, byte>>();
                        //     for (uint j = 0; j < numUnicodeValueRanges; j++)
                        //     {
                        //         unicodeRanges.Add(Tuple.Create(reader.ReadUInt24(), reader.ReadByte()));
                        //     }
                        // }

                        // nonDefaultUVSOffset
                        if (variationSelectors[i].Item3 > 0)
                        {
                            reader.Stream.Position = basePosition + record.Offset + variationSelectors[i].Item3;
                            uint numUVSMappings = reader.ReadUInt32();
                            var uvsMappings = new List<Tuple<uint, ushort>>();
                            for (int j = 0; j < numUVSMappings; j++)
                            {
                                uvsMappings.Add(Tuple.Create(reader.ReadUInt24(), reader.ReadUInt16()));
                            }

                            for (int j = 0; j < numUVSMappings; j++)
                            {
                                var key = Tuple.Create(uvsMappings[j].Item1, variationSelectors[i].Item1);
                                record.VariationGlyphMap[key] = uvsMappings[j].Item2;
                            }
                        }
                    }
                }
            }

            var sorted = new SortedDictionary<int, Dictionary<int, ushort>>();

            foreach (var record in EncodingRecords)
            {
                if (record.PlatformID == 0 && record.EncodingID == 6)
                {
                    sorted.Add(1, record.GlyphMap);
                    continue;
                }
                if (record.PlatformID == 0 && record.EncodingID == 4)
                {
                    sorted.Add(2, record.GlyphMap);
                    continue;
                }
                if (record.PlatformID == 3 && record.EncodingID == 10)
                {
                    sorted.Add(3, record.GlyphMap);
                    continue;
                }
                if (record.PlatformID == 0 && record.EncodingID == 3)
                {
                    sorted.Add(4, record.GlyphMap);
                    continue;
                }
                if (record.PlatformID == 3 && record.EncodingID == 1)
                {
                    sorted.Add(5, record.GlyphMap);
                    continue;
                }
                if (record.PlatformID == 0 && record.EncodingID == 2)
                {
                    sorted.Add(6, record.GlyphMap);
                    continue;
                }
                if (record.PlatformID == 0 && record.EncodingID == 1)
                {
                    sorted.Add(7, record.GlyphMap);
                    continue;
                }
                if (record.PlatformID == 0 && record.EncodingID == 0)
                {
                    sorted.Add(8, record.GlyphMap);
                    continue;
                }
            }

            _glyphMap = sorted.Values.ToArray()[0];
        }

        /// <summary>Gets a list of EncodingRecord.</summary>
        public List<EncodingRecord> EncodingRecords { get; } = new List<EncodingRecord>();

        /// <summary>Gets a table version number.</summary>
        public ushort TableVersionNumber { get; }

        /// <summary>Gets a number of encoding tables.</summary>
        public ushort NumTables { get; }

        /// <summary>Gets the mapping of a charactor code point to a glyph index optimal.</summary>
        public Dictionary<int, ushort> GlyphMap
        {
            get { return _glyphMap; }
        }
    }
}
