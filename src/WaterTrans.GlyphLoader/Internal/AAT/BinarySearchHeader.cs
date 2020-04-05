// <copyright file="BinarySearchHeader.cs" company="WaterTrans">
// © 2020 WaterTrans
// </copyright>

namespace WaterTrans.GlyphLoader.Internal.AAT
{
    /// <summary>
    /// The binary search tables contain data that assist in conducting binary searches for information in the table that it resides in.
    /// </summary>
    internal sealed class BinarySearchHeader
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BinarySearchHeader"/> class.
        /// </summary>
        /// <param name="reader">The <see cref="TypefaceReader"/>.</param>
        internal BinarySearchHeader(TypefaceReader reader)
        {
            UnitSize = reader.ReadUInt16();
            NUnits = reader.ReadUInt16();
            SearchRange = reader.ReadUInt16();
            EntrySelector = reader.ReadUInt16();
            RangeShift = reader.ReadUInt16();
        }

        /// <summary>Gets UnitSize.</summary>
        /// <remarks>Size of a lookup unit for this search in bytes.</remarks>
        public ushort UnitSize { get; }

        /// <summary>Gets NUnits.</summary>
        /// <remarks>Number of units of the preceding size to be searched.</remarks>
        public ushort NUnits { get; }

        /// <summary>Gets SearchRange.</summary>
        /// <remarks>The value of unitSize times the largest power of 2 that is less than or equal to the value of nUnits.</remarks>
        public ushort SearchRange { get; }

        /// <summary>Gets EntrySelector.</summary>
        /// <remarks>The log base 2 of the largest power of 2 less than or equal to the value of nUnits.</remarks>
        public ushort EntrySelector { get; }

        /// <summary>Gets RangeShift.</summary>
        /// <remarks>The value of unitSize times the difference of the value of nUnits minus the largest power of 2 less than or equal to the value of nUnits.</remarks>
        public ushort RangeShift { get; }
    }
}
