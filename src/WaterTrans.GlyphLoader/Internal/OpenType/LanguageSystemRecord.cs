// <copyright file="LanguageSystemRecord.cs" company="WaterTrans">
// © 2020 WaterTrans
// </copyright>

namespace WaterTrans.GlyphLoader.Internal.OpenType
{
    /// <summary>
    /// The OpenType language system record.
    /// </summary>
    internal sealed class LanguageSystemRecord
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LanguageSystemRecord"/> class.
        /// </summary>
        /// <param name="reader">The <see cref="TypefaceReader"/>.</param>
        internal LanguageSystemRecord(TypefaceReader reader)
        {
            IsDefault = false;
            Tag = reader.ReadCharArray(4);
            Offset = reader.ReadUInt16();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LanguageSystemRecord"/> class.
        /// </summary>
        /// <param name="isDefault">Sets IsDefault.</param>
        /// <param name="offset">Sets Offset.</param>
        internal LanguageSystemRecord(bool isDefault, ushort offset)
        {
            IsDefault = isDefault;
            Tag = "DFLT";
            Offset = offset;
        }

        /// <summary>Gets a value indicating whether default LangSysTable.</summary>
        /// <remarks>Default LangSysTable.</remarks>
        public bool IsDefault { get; }

        /// <summary>Gets Tag.</summary>
        /// <remarks>LangSysTag identifier.</remarks>
        public string Tag { get; }

        /// <summary>Gets Offset.</summary>
        /// <remarks>Offset to LangSys table.— from beginning of Script table.</remarks>
        public ushort Offset { get; }
    }
}
