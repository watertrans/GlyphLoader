// <copyright file="WOFF2Header.cs" company="WaterTrans">
// © 2020 WaterTrans and Contributors
// </copyright>

namespace WaterTrans.GlyphLoader.Internal.WOFF2
{
    /// <summary>
    /// The WOFF 2.0 header.
    /// </summary>
    internal class WOFF2Header
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WOFF2Header"/> class.
        /// </summary>
        /// <param name="reader">The <see cref="TypefaceReader"/>.</param>
        internal WOFF2Header(TypefaceReader reader)
        {
            Signature = reader.ReadUInt32();
            Flavor = reader.ReadUInt32();
            Length = reader.ReadUInt32();
            NumTables = reader.ReadUInt16();
            Reserved = reader.ReadUInt16();
            TotalSfntSize = reader.ReadUInt32();
            TotalCompressedSize = reader.ReadUInt32();
            MajorVersion = reader.ReadUInt16();
            MinorVersion = reader.ReadUInt16();
            MetaOffset = reader.ReadUInt32();
            MetaLength = reader.ReadUInt32();
            MetaOrigLength = reader.ReadUInt32();
            PrivOffset = reader.ReadUInt32();
            PrivLength = reader.ReadUInt32();
        }

        /// <summary>Gets a signature.</summary>
        public uint Signature { get; }

        /// <summary>Gets the "sfnt version" of the input font.</summary>
        public uint Flavor { get; }

        /// <summary>Gets the total size of the WOFF file.</summary>
        public uint Length { get; }

        /// <summary>Gets a number of entries in directory of font tables.</summary>
        public ushort NumTables { get; }

        /// <summary>Gets a Reserved; set to 0.</summary>
        public ushort Reserved { get; }

        /// <summary>Gets a total size needed for the uncompressed font data, including the sfnt header, directory, and font tables(including padding).</summary>
        public uint TotalSfntSize { get; }

        /// <summary>Gets a total length of the compressed data block.</summary>
        public uint TotalCompressedSize { get; }

        /// <summary>Gets a major version of the WOFF file.</summary>
        public ushort MajorVersion { get; }

        /// <summary>Gets a minor version of the WOFF file.</summary>
        public ushort MinorVersion { get; }

        /// <summary>Gets an offset to metadata block, from beginning of WOFF file.</summary>
        public uint MetaOffset { get; }

        /// <summary>Gets a length of compressed metadata block.</summary>
        public uint MetaLength { get; }

        /// <summary>Gets an uncompressed size of metadata block.</summary>
        public uint MetaOrigLength { get; }

        /// <summary>Gets an offset to private data block, from beginning of WOFF file.</summary>
        public uint PrivOffset { get; }

        /// <summary>Gets a length of private data block.</summary>
        public uint PrivLength { get; }
    }
}
