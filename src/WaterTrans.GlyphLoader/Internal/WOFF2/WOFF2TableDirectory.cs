// <copyright file="WOFF2TableDirectory.cs" company="WaterTrans">
// © 2020 WaterTrans and Contributors
// </copyright>

namespace WaterTrans.GlyphLoader.Internal.WOFF2
{
    /// <summary>
    /// The WOFF 2.0 table directory.
    /// </summary>
    internal class WOFF2TableDirectory
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WOFF2TableDirectory"/> class.
        /// </summary>
        /// <param name="reader">The <see cref="TypefaceReader"/>.</param>
        /// <param name="offset">An offset from beginning of compressed data.</param>
        internal WOFF2TableDirectory(TypefaceReader reader, uint offset)
        {
            Offset = offset;
            Flags = reader.ReadByte();

            int knowTable = Flags & 0x3F;
            if (knowTable < 63)
            {
                Tag = KnownTable.Tags[knowTable];
            }
            else
            {
                Tag = reader.ReadCharArray(4);
            }

            PreprocessingTransformationVersion = (byte)((Flags >> 5) & 0x3);
            OrigLength = reader.ReadUIntBase128();

            if (PreprocessingTransformationVersion == 0)
            {
                if (Tag == TableNames.GLYF || Tag == TableNames.LOCA)
                {
                    TransformLength = reader.ReadUIntBase128();
                    Length = TransformLength;
                    return;
                }
            }

            Length = OrigLength;
        }

        /// <summary>Gets a table type and flags.</summary>
        public byte Flags { get; }

        /// <summary>Gets a tag 4-byte tag(optional).</summary>
        public string Tag { get; }

        /// <summary>Gets the preprocessing transformation version number (0-3) that was applied to each table.</summary>
        public byte PreprocessingTransformationVersion { get; }

        /// <summary>Gets a length of original table.</summary>
        public uint OrigLength { get; }

        /// <summary>Gets a transformed length(if applicable).</summary>
        public uint TransformLength { get; }

        /// <summary>Gets a length of compressed data.</summary>
        public uint Length { get; }

        /// <summary>Gets an offset from beginning of compressed data.</summary>
        public uint Offset { get; }
    }
}
