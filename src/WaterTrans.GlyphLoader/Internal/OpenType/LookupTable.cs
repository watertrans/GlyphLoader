// <copyright file="LookupTable.cs" company="WaterTrans">
// © 2020 WaterTrans
// </copyright>

using System.Collections.Generic;
using WaterTrans.GlyphLoader.Internal.OpenType.GPOS;
using WaterTrans.GlyphLoader.Internal.OpenType.GSUB;

namespace WaterTrans.GlyphLoader.Internal.OpenType
{
    /// <summary>
    /// The OpenType lookup table.
    /// </summary>
    internal sealed class LookupTable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LookupTable"/> class.
        /// </summary>
        /// <param name="reader">The <see cref="TypefaceReader"/>.</param>
        internal LookupTable(TypefaceReader reader)
        {
            Offset = reader.ReadUInt16();
        }

        /// <summary>Gets Offset.</summary>
        /// <remarks>Offset to Script table.- from beginning of ScriptList.</remarks>
        public ushort Offset { get; }

        /// <summary>Gets or sets LookupType.</summary>
        /// <remarks>Different enumerations for GSUB and GPOS.</remarks>
        public ushort LookupType { get; internal set; }

        /// <summary>Gets or sets LookupFlag.</summary>
        /// <remarks>Lookup qualifiers.</remarks>
        public ushort LookupFlag { get; internal set; }

        /// <summary>Gets or sets SubTableCount.</summary>
        /// <remarks>Number of SubTables for this lookup.</remarks>
        public ushort SubTableCount { get; internal set; }

        /// <summary>Gets array of offsets to SubTables.- from beginning of Lookup table.</summary>
        public List<ushort> SubTableList { get; } = new List<ushort>();

        /// <summary>Gets list of SingleSubstitution.</summary>
        public List<SingleSubstitution> SingleSubstitutionList { get; } = new List<SingleSubstitution>();

        /// <summary>Gets list of MultipleSubstitution.</summary>
        public List<MultipleSubstitution> MultipleSubstitutionList { get; } = new List<MultipleSubstitution>();

        /// <summary>Gets list of AlternateSubstitution.</summary>
        public List<AlternateSubstitution> AlternateSubstitutionList { get; } = new List<AlternateSubstitution>();

        /// <summary>Gets list of LigatureSubstitution.</summary>
        public List<LigatureSubstitution> LigatureSubstitutionList { get; } = new List<LigatureSubstitution>();

        /// <summary>Gets list of SingleAdjustment.</summary>
        public List<SingleAdjustment> SingleAdjustmentList { get; } = new List<SingleAdjustment>();

        /// <summary>Gets list of PairAdjustment.</summary>
        public List<PairAdjustment> PairAdjustmentList { get; } = new List<PairAdjustment>();
    }
}
