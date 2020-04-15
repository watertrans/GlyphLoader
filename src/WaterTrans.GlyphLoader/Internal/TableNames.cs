// <copyright file="TableNames.cs" company="WaterTrans">
// © 2020 WaterTrans
// </copyright>

namespace WaterTrans.GlyphLoader.Internal
{
    /// <summary>
    /// Font tables.
    /// </summary>
    internal static class TableNames
    {
        /// <summary>
        /// CMAP table.
        /// </summary>
        public const string CMAP = "cmap";

        /// <summary>
        /// NAME table.
        /// </summary>
        public const string NAME = "name";

        /// <summary>
        /// MAXP table.
        /// </summary>
        public const string MAXP = "maxp";

        /// <summary>
        /// HEAD table.
        /// </summary>
        public const string HEAD = "head";

        /// <summary>
        /// HHEA table.
        /// </summary>
        public const string HHEA = "hhea";

        /// <summary>
        /// HMTX table.
        /// </summary>
        public const string HMTX = "hmtx";

        /// <summary>
        /// OS2 table.
        /// </summary>
        public const string OS2 = "OS/2";

        /// <summary>
        /// POST table.
        /// </summary>
        public const string POST = "post";

        /// <summary>
        /// LOCA table.
        /// </summary>
        public const string LOCA = "loca";

        /// <summary>
        /// VHEA table.
        /// </summary>
        public const string VHEA = "vhea";

        /// <summary>
        /// VMTX table.
        /// </summary>
        public const string VMTX = "vmtx";

        /// <summary>
        /// MORT table.
        /// </summary>
        public const string MORT = "mort";

        /// <summary>
        /// GSUB table.
        /// </summary>
        public const string GSUB = "GSUB";

        /// <summary>
        /// GPOS table.
        /// </summary>
        public const string GPOS = "GPOS";

        /// <summary>
        /// GLYF table.
        /// </summary>
        public const string GLYF = "glyf";

        /// <summary>
        /// Gets the required tables.
        /// </summary>
        public static readonly string[] RequiedTables = { CMAP, HEAD, HHEA, HMTX, MAXP, NAME, POST };
    }
}
