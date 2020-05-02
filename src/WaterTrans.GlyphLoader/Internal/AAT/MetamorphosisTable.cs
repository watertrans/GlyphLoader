// <copyright file="MetamorphosisTable.cs" company="WaterTrans">
// © 2020 WaterTrans
// </copyright>

using System;
using System.Collections.Generic;
using System.IO;

namespace WaterTrans.GlyphLoader.Internal.AAT
{
    /// <summary>
    /// The chain's metamorphosis subtable.
    /// </summary>
    internal sealed class MetamorphosisTable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MetamorphosisTable"/> class.
        /// </summary>
        /// <param name="reader">The <see cref="TypefaceReader"/>.</param>
        internal MetamorphosisTable(TypefaceReader reader)
        {
            Length = reader.ReadUInt16();
            Coverage = reader.ReadUInt16();
            SubFeatureFlags = reader.ReadUInt32();

            // TODO Supported only non-contextual replacement for vertical writing.
            if ((Coverage & 0x8000) >= 1 && (Coverage & 0x0004) >= 1)
            {
                Format = reader.ReadUInt16();

                if (Format == 2 || Format == 4 || Format == 6)
                {
                    Header = new BinarySearchHeader(reader);
                }

                if (Header.UnitSize != 4)
                {
                    throw new NotImplementedException($"Not implemented metamorphosis subtable unit size {Header.UnitSize}.");
                }

                switch (Format)
                {
                    case 6:
                        for (int i = 1; i <= Header.NUnits; i++)
                        {
                            LookupSingleRecords.Add(new LookupSingleRecord(reader));
                        }
                        break;
                    default:
                        throw new NotImplementedException($"Not implemented metamorphosis subtable format {Format}.");
                }
            }
            else
            {
                reader.Position += Length - 8;
            }
        }

        /// <summary>Gets BinarySearchHeader.</summary>
        public BinarySearchHeader Header { get; }

        /// <summary>Gets LookupSingleRecords.</summary>
        /// <remarks>The list of lookup single record.</remarks>
        public List<LookupSingleRecord> LookupSingleRecords { get; } = new List<LookupSingleRecord>();

        /// <summary>Gets Length.</summary>
        /// <remarks>Length of subtable in bytes, including this header.</remarks>
        public ushort Length { get; }

        /// <summary>Gets Coverage.</summary>
        /// <remarks>Length of subtable in bytes, including this header.</remarks>
        public ushort Coverage { get; }

        /// <summary>Gets SubFeatureFlags.</summary>
        /// <remarks>Flags for the settings that this subtable describes.</remarks>
        public uint SubFeatureFlags { get; }

        /// <summary>Gets Format.</summary>
        /// <remarks>Format of this lookup table. There are five lookup table formats, each with a format number.</remarks>
        public ushort Format { get; }
    }
}
