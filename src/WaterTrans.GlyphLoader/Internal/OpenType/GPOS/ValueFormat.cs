// <copyright file="ValueFormat.cs" company="WaterTrans">
// © 2020 WaterTrans
// </copyright>

namespace WaterTrans.GlyphLoader.Internal.OpenType.GPOS
{
    /// <summary>The value format bit enumeration.</summary>
    internal enum ValueFormat : ushort
    {
        /// <summary>Includes horizontal adjustment for placement.</summary>
        XPlacement = 0x1,

        /// <summary>Includes vertical adjustment for placement.</summary>
        YPlacement = 0x2,

        /// <summary>Includes horizontal adjustment for advance.</summary>
        XAdvance = 0x4,

        /// <summary>Includes vertical adjustment for advance.</summary>
        YAdvance = 0x8,

        /// <summary>Includes horizontal Device table for placement.</summary>
        XPlaDevice = 0x10,

        /// <summary>Includes vertical Device table for placement.</summary>
        YPlaDevice = 0x20,

        /// <summary>Includes horizontal Device table for advance.</summary>
        XAdvDevice = 0x40,

        /// <summary>Includes vertical Device table for advance.</summary>
        YAdvDevice = 0x80,
    }
}
