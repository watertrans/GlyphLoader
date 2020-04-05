// <copyright file="LookupType.cs" company="WaterTrans">
// © 2020 WaterTrans
// </copyright>

namespace WaterTrans.GlyphLoader.Internal.OpenType.GPOS
{
    /// <summary>The GPOS lookup type enumeration.</summary>
    internal enum LookupType : ushort
    {
        /// <summary>Single Adjustment Subtable.</summary>
        SingleAdjustment = 1,

        /// <summary>Pair Adjustment Subtable.</summary>
        PairAdjustment = 2,

        /// <summary>Cursive Attachment Subtable.</summary>
        CursiveAttachment = 3,

        /// <summary>MarkToBase Attachment Subtable.</summary>
        MarkToBaseAttachment = 4,

        /// <summary>MarkToLigature Attachment Subtable.</summary>
        MarkToLigatureAttachment = 5,

        /// <summary>MarkToMark Attachment Subtable.</summary>
        MarkToMarkAttachment = 6,

        /// <summary>Context Positioning Subtable.</summary>
        ContextPositioning = 7,

        /// <summary>Chained Context positioning Subtable.</summary>
        ChainedContextPositioning = 8,

        /// <summary>Extension positioning Subtable.</summary>
        ExtensionPositioning = 9,
    }
}
