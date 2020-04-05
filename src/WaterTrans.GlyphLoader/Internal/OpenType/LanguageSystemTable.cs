// <copyright file="LanguageSystemTable.cs" company="WaterTrans">
// © 2020 WaterTrans
// </copyright>

using System.Collections.Generic;

namespace WaterTrans.GlyphLoader.Internal.OpenType
{
    /// <summary>
    /// The OpenType language sytem table.
    /// </summary>
    internal sealed class LanguageSystemTable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LanguageSystemTable"/> class.
        /// </summary>
        /// <param name="reader">The <see cref="TypefaceReader"/>.</param>
        internal LanguageSystemTable(TypefaceReader reader)
        {
            LookupOrder = reader.ReadUInt16();
            ReqFeatureIndex = reader.ReadUInt16();
            FeatureCount = reader.ReadUInt16();
            for (int i = 1; i <= FeatureCount; i++)
            {
                FeatureIndexList.Add(reader.ReadUInt16());
            }
        }

        /// <summary>Gets LookupOrder.</summary>
        /// <remarks>Reserved for an offset to a reordering table.</remarks>
        public ushort LookupOrder { get; }

        /// <summary>Gets ReqFeatureIndex.</summary>
        /// <remarks>Index of a feature required for this language system.— if no required features = 0xFFFF.</remarks>
        public ushort ReqFeatureIndex { get; }

        /// <summary>Gets FeatureCount.</summary>
        /// <remarks>Number of FeatureIndex values for this language system.— excludes the required feature.</remarks>
        public ushort FeatureCount { get; }

        /// <summary>Gets FeatureIndexList.</summary>
        /// <remarks>Array of indices into the FeatureList.— in arbitrary order.</remarks>
        public List<ushort> FeatureIndexList { get; } = new List<ushort>();
    }
}
