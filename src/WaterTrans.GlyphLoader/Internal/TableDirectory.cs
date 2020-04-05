// <copyright file="TableDirectory.cs" company="WaterTrans">
// © 2020 WaterTrans
// </copyright>

namespace WaterTrans.GlyphLoader.Internal
{
    /// <summary>
    /// Directory of font tables, containing size and other info.
    /// </summary>
    internal sealed class TableDirectory
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TableDirectory"/> class.
        /// </summary>
        /// <param name="tag">Set instance value for Tag.</param>
        /// <param name="checksum">Set instance value for CheckSum.</param>
        /// <param name="offset">Set instance value for Offset.</param>
        /// <param name="length">Set instance value for Length.</param>
        internal TableDirectory(string tag, uint checksum, uint offset, uint length)
        {
            Tag = tag;
            CheckSum = checksum;
            Offset = offset;
            Length = length;
        }

        /// <summary>
        /// Gets 4-byte identifier.
        /// </summary>
        public string Tag { get; }

        /// <summary>
        /// Gets checkSum for this table.
        /// </summary>
        public uint CheckSum { get; }

        /// <summary>
        /// Gets offset from beginning of TrueType font file.
        /// </summary>
        public uint Offset { get; }

        /// <summary>
        /// Gets length of this table.
        /// </summary>
        public uint Length { get; }
    }
}
