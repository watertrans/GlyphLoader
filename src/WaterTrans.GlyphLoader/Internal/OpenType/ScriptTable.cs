// <copyright file="ScriptTable.cs" company="WaterTrans">
// © 2020 WaterTrans
// </copyright>

using System.Collections.Generic;

namespace WaterTrans.GlyphLoader.Internal.OpenType
{
    /// <summary>
    /// The OpenType script table.
    /// </summary>
    internal sealed class ScriptTable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ScriptTable"/> class.
        /// </summary>
        /// <param name="reader">The <see cref="TypefaceReader"/>.</param>
        internal ScriptTable(TypefaceReader reader)
        {
            Tag = reader.ReadCharArray(4);
            Offset = reader.ReadUInt16();
        }

        /// <summary>Gets Tag.</summary>
        /// <remarks>4-byte ScriptTag identifier.</remarks>
        public string Tag { get; }

        /// <summary>Gets Offset.</summary>
        /// <remarks>Offset to Script table.— from beginning of ScriptList.</remarks>
        public ushort Offset { get; }

        /// <summary>Gets or Sets DefaultLanguageSystemOffset.</summary>
        /// <remarks>Offset to DefaultLanguageSystem table.— from beginning of Script table.</remarks>
        public ushort DefaultLanguageSystemOffset { get; internal set; }

        /// <summary>Gets or Sets LanguageSystemCount.</summary>
        /// <remarks>Number of LanguageSystemRecords for this script.— excluding the DefaultLangSys.</remarks>
        public ushort LanguageSystemCount { get; internal set; }

        /// <summary>Gets LanguageSystemRecords.</summary>
        /// <remarks>List of LanguageSystemRecord.</remarks>
        public List<LanguageSystemRecord> LanguageSystemRecords { get; } = new List<LanguageSystemRecord>();

        /// <summary>Gets LanguageSystemTables.</summary>
        /// <remarks>List of LanguageSystemTable.</remarks>
        public List<LanguageSystemTable> LanguageSystemTables { get; } = new List<LanguageSystemTable>();
    }
}
