// <copyright file="PlatformID.cs" company="WaterTrans">
// © 2020 WaterTrans and Contributors
// </copyright>

namespace WaterTrans.GlyphLoader.Internal.NAME
{
    /// <summary>The NAME NameRecord PlatformID type enumeration.</summary>
    internal enum PlatformID : ushort
    {
        /// <summary>Indicates Unicode version.</summary>
        Unicode = 0,

        /// <summary>QuickDraw Script Manager code.</summary>
        Macintosh = 1,

        /// <summary>(reserved; do not use)</summary>
        ISO = 2,

        /// <summary>Microsoft encoding.</summary>
        Microsoft = 3,
    }
}
