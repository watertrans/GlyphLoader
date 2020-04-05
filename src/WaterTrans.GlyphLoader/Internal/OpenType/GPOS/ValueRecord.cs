// <copyright file="ValueRecord.cs" company="WaterTrans">
// © 2020 WaterTrans
// </copyright>

namespace WaterTrans.GlyphLoader.Internal.OpenType.GPOS
{
    /// <summary>
    /// The GPOS ValueRecord.
    /// </summary>
    internal sealed class ValueRecord
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ValueRecord"/> class.
        /// </summary>
        /// <param name="reader">The <see cref="TypefaceReader"/>.</param>
        /// <param name="valueFormat">The ValueFormat Flags.</param>
        internal ValueRecord(TypefaceReader reader, ushort valueFormat)
        {
            Format = valueFormat;
            if (Format == 0)
            {
                return;
            }
            if ((Format & (ushort)ValueFormat.XPlacement) > 0)
            {
                XPlacement = reader.ReadInt16();
            }
            if ((Format & (ushort)ValueFormat.YPlacement) > 0)
            {
                YPlacement = reader.ReadInt16();
            }
            if ((Format & (ushort)ValueFormat.XAdvance) > 0)
            {
                XAdvance = reader.ReadInt16();
            }
            if ((Format & (ushort)ValueFormat.YAdvance) > 0)
            {
                YAdvance = reader.ReadInt16();
            }
            if ((Format & (ushort)ValueFormat.XPlaDevice) > 0)
            {
                XPlaDevice = reader.ReadUInt16();
            }
            if ((Format & (ushort)ValueFormat.YPlaDevice) > 0)
            {
                YPlaDevice = reader.ReadUInt16();
            }
            if ((Format & (ushort)ValueFormat.XAdvDevice) > 0)
            {
                XAdvDevice = reader.ReadUInt16();
            }
            if ((Format & (ushort)ValueFormat.YAdvDevice) > 0)
            {
                YAdvDevice = reader.ReadUInt16();
            }
        }

        /// <summary>Gets the ValueFormat.</summary>
        public ushort Format { get; }

        /// <summary>Gets a horizontal adjustment for placement.</summary>
        public short XPlacement { get; }

        /// <summary>Gets a vertical adjustment for placement.</summary>
        public short YPlacement { get; }

        /// <summary>Gets a horizontal adjustment for advance.</summary>
        public short XAdvance { get; }

        /// <summary>Gets a vertical adjustment for advance.</summary>
        public short YAdvance { get; }

        /// <summary>Gets an offset to Device table for horizontal placement.</summary>
        public ushort XPlaDevice { get; }

        /// <summary>Gets an offset to Device table for vertical placement.</summary>
        public ushort YPlaDevice { get; }

        /// <summary>Gets an offset to Device table for horizontal advance.</summary>
        public ushort XAdvDevice { get; }

        /// <summary>Gets an offset to Device table for vertical advance.</summary>
        public ushort YAdvDevice { get; }

        /// <summary>Gets a value indicating whether empty.</summary>
        public bool IsEmpty
        {
            get
            {
                if (Format == 0)
                {
                    return true;
                }
                else
                {
                    if ((Format & (ushort)ValueFormat.XPlacement) > 0 && XPlacement != 0)
                    {
                        return false;
                    }
                    if ((Format & (ushort)ValueFormat.YPlacement) > 0 && YPlacement != 0)
                    {
                        return false;
                    }
                    if ((Format & (ushort)ValueFormat.XAdvance) > 0 && XAdvance != 0)
                    {
                        return false;
                    }
                    if ((Format & (ushort)ValueFormat.YAdvance) > 0 && YAdvance != 0)
                    {
                        return false;
                    }
                    if ((Format & (ushort)ValueFormat.XPlaDevice) > 0 && XPlaDevice != 0)
                    {
                        return false;
                    }
                    if ((Format & (ushort)ValueFormat.YPlaDevice) > 0 && YPlaDevice != 0)
                    {
                        return false;
                    }
                    if ((Format & (ushort)ValueFormat.XAdvDevice) > 0 && XAdvDevice != 0)
                    {
                        return false;
                    }
                    if ((Format & (ushort)ValueFormat.YAdvDevice) > 0 && YAdvDevice != 0)
                    {
                        return false;
                    }
                    return true;
                }
            }
        }
    }
}
