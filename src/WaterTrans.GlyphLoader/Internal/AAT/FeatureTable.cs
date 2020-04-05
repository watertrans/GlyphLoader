// <copyright file="FeatureTable.cs" company="WaterTrans">
// © 2020 WaterTrans
// </copyright>

namespace WaterTrans.GlyphLoader.Internal.AAT
{
    /// <summary>
    /// The chain's feature subtable.
    /// </summary>
    internal sealed class FeatureTable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FeatureTable"/> class.
        /// </summary>
        /// <param name="reader">The <see cref="TypefaceReader"/>.</param>
        internal FeatureTable(TypefaceReader reader)
        {
            FeatureType = reader.ReadUInt16();
            FeatureSetting = reader.ReadUInt16();
            EnableFlags = reader.ReadUInt32();
            DisableFlags = reader.ReadUInt32();
        }

        /// <summary>Gets FeatureType.</summary>
        /// <remarks>The type of feature.</remarks>
        public ushort FeatureType { get; }

        /// <summary>Gets FeatureSetting.</summary>
        /// <remarks>The feature's setting.</remarks>
        public ushort FeatureSetting { get; }

        /// <summary>Gets EnableFlags.</summary>
        /// <remarks>Flags for the settings that this feature and setting enables.</remarks>
        public uint EnableFlags { get; }

        /// <summary>Gets DisableFlags.</summary>
        /// <remarks>Complement of flags for the settings that this feature and setting disable.</remarks>
        public uint DisableFlags { get; }
    }
}
