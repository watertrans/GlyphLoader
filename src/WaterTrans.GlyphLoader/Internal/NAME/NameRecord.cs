// <copyright file="NameRecord.cs" company="WaterTrans">
// © 2020 WaterTrans and Contributors
// </copyright>

namespace WaterTrans.GlyphLoader.Internal.NAME
{
    /// <summary>
    /// The name records array item.
    /// </summary>
    internal sealed class NameRecord
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NameRecord"/> class.
        /// </summary>
        /// <param name="reader">The <see cref="TypefaceReader"/>.</param>
        internal NameRecord(TypefaceReader reader)
        {
            PlatformID = reader.ReadUInt16();
            EncodingID = reader.ReadUInt16();
            LanguageID = reader.ReadUInt16();
            NameID = reader.ReadUInt16();
            Length = reader.ReadUInt16();
            Offset = reader.ReadUInt16();
        }

        /// <summary>Platform identifier code.</summary>
        public ushort PlatformID { get; }

        /// <summary>Platform-specific encoding identifier.</summary>
        public ushort EncodingID { get; }

        /// <summary>Language identifier.</summary>
        public ushort LanguageID { get; }

        /// <summary>Name identifiers.</summary>
        public ushort NameID { get; }

        /// <summary>Name string length in bytes.</summary>
        public ushort Length { get; }

        /// <summary>Name string offset in bytes from stringOffset.</summary>
        public ushort Offset { get; }

        /// <summary>The character strings of the names (Note that these are not necessarily ASCII!).</summary>
        public string NameString { get; set; }
    }
}
