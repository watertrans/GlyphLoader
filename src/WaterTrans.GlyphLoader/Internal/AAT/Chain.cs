// <copyright file="Chain.cs" company="WaterTrans">
// © 2020 WaterTrans
// </copyright>

using System.Collections.Generic;

namespace WaterTrans.GlyphLoader.Internal.AAT
{
    /// <summary>
    /// The metamorphosis chain.
    /// </summary>
    internal sealed class Chain
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Chain"/> class.
        /// </summary>
        /// <param name="reader">The <see cref="TypefaceReader"/>.</param>
        internal Chain(TypefaceReader reader)
        {
            DefaultFlags = reader.ReadUInt32();
            ChainLength = reader.ReadUInt32();
            NFeatureEntries = reader.ReadUInt16();
            NSubtables = reader.ReadUInt16();

            for (int i = 1; i <= NFeatureEntries; i++)
            {
                FeatureTables.Add(new FeatureTable(reader));
            }

            for (int i = 1; i <= NSubtables; i++)
            {
                MetamorphosisTables.Add(new MetamorphosisTable(reader));
            }
        }

        /// <summary>Gets FeatureTables.</summary>
        /// <remarks>The list of FeatureTable.</remarks>
        public List<FeatureTable> FeatureTables { get; } = new List<FeatureTable>();

        /// <summary>Gets MetamorphosisTables.</summary>
        /// <remarks>The list of metamorphosis table.</remarks>
        public List<MetamorphosisTable> MetamorphosisTables { get; } = new List<MetamorphosisTable>();

        /// <summary>Gets DefaultFlags.</summary>
        /// <remarks>The default sub-feature flags for this chain.</remarks>
        public uint DefaultFlags { get; }

        /// <summary>Gets ChainLength.</summary>
        /// <remarks>The length of the chain in bytes, including this header.</remarks>
        public uint ChainLength { get; }

        /// <summary>Gets NFeatureEntries.</summary>
        /// <remarks>The number of entries in the chain's feature subtable.</remarks>
        public ushort NFeatureEntries { get; }

        /// <summary>Gets NSubtables.</summary>
        /// <remarks>The number of subtables in the chain.</remarks>
        public ushort NSubtables { get; }
    }
}
