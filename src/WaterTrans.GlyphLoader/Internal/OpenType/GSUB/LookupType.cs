// <copyright file="LookupType.cs" company="WaterTrans">
// © 2020 WaterTrans
// </copyright>

namespace WaterTrans.GlyphLoader.Internal.OpenType.GSUB
{
    /// <summary>The GUB lookup type enumeration.</summary>
    internal enum LookupType : ushort
    {
        /// <summary>Single Substitution Subtable.</summary>
        SingleSubstitution = 1,

        /// <summary>Multiple Substitution Subtable.</summary>
        MultipleSubstitution = 2,

        /// <summary>Alternate Substitution Subtable.</summary>
        AlternateSubstitution = 3,

        /// <summary>Ligature Substitution Subtable.</summary>
        LigatureSubstitution = 4,

        /// <summary>Contextual Substitution Subtable.</summary>
        ContextualSubstitution = 5,

        /// <summary>Chaining Contextual Substitution Subtable.</summary>
        ChainingContextualSubstitution = 6,

        /// <summary>Extension Substitution Subtable.</summary>
        ExtensionSubstitution = 7,

        /// <summary>Reverse Chaining Contextual Single Substitution Subtable.</summary>
        ReverseChainingContextualSingleSubstitution = 8,
    }
}
