// <copyright file="FeatureTable.cs" company="WaterTrans">
// © 2020 WaterTrans
// </copyright>

using System.Collections.Generic;

namespace WaterTrans.GlyphLoader.Internal.OpenType
{
    /// <summary>
    /// The OpenType feature table.
    /// </summary>
    internal sealed class FeatureTable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FeatureTable"/> class.
        /// </summary>
        /// <param name="reader">The <see cref="TypefaceReader"/>.</param>
        internal FeatureTable(TypefaceReader reader)
        {
            Tag = reader.ReadCharArray(4);
            Offset = reader.ReadUInt16();
        }

        /// <summary>Gets Tag.</summary>
        /// <remarks>4-byte feature identification tag.</remarks>
        public string Tag { get; }

        /// <summary>Gets Offset.</summary>
        /// <remarks>Offset to Script table.- from beginning of ScriptList.</remarks>
        public ushort Offset { get; }

        /// <summary>Gets or sets FeatureParams.</summary>
        /// <remarks>NULL (reserved for offset to FeatureParams).</remarks>
        public ushort FeatureParams { get; internal set; }

        /// <summary>Gets or sets LookupCount.</summary>
        /// <remarks>Number of LookupList indices for this feature.</remarks>
        public ushort LookupCount { get; internal set; }

        /// <summary>Gets list of LookupList.</summary>
        /// <remarks>Array of LookupList indices for this feature.- zero-based (first lookup is LookupListIndex = 0).</remarks>
        public List<ushort> LookupListIndex { get; } = new List<ushort>();
    }
}
