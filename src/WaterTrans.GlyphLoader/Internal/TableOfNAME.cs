// <copyright file="TableOfNAME.cs" company="WaterTrans">
// © 2020 WaterTrans and Contributors
// </copyright>

using System.Collections.Generic;
using WaterTrans.GlyphLoader.Internal.NAME;

namespace WaterTrans.GlyphLoader.Internal
{
    /// <summary>
    /// Table of NAME.
    /// </summary>
    internal sealed class TableOfNAME
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TableOfNAME"/> class.
        /// </summary>
        /// <param name="reader">The <see cref="TypefaceReader"/>.</param>
        internal TableOfNAME(TypefaceReader reader)
        {
            var basePosition = reader.Position;
            Format = reader.ReadUInt16();
            NumberOfRecords = reader.ReadUInt16();
            StringOffset = reader.ReadUInt16();
            for (var i = 0; i < NumberOfRecords; i++)
            {
                NameRecords.Add(new NameRecord(reader));
            }

            foreach (var record in NameRecords)
            {
                reader.Position = basePosition + StringOffset + record.Offset;
                record.NameString = reader.ReadString(record.Length, NameEncoding.GetEncoding(record));
            }
        }

        /// <summary>Format selector. Set to 0.</summary>
        public ushort Format { get; }

        /// <summary>The number of nameRecords in this name table.</summary>
        public ushort NumberOfRecords { get; }

        /// <summary>Offset in bytes to the beginning of the name character strings.</summary>
        public ushort StringOffset { get; }

        /// <summary>Get a list of NameRecord.</summary>
        public List<NameRecord> NameRecords { get; } = new List<NameRecord>();
    }
}
