// <copyright file="IndexDataOfFontDictionary.cs" company="WaterTrans">
// © 2020 WaterTrans
// </copyright>

namespace WaterTrans.GlyphLoader.Internal.OpenType.CFF
{
    /// <summary>
    /// The Compact FontFormat Specification Font DICT INDEX.
    /// </summary>
    internal class IndexDataOfFontDictionary : IndexData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IndexDataOfFontDictionary"/> class.
        /// </summary>
        /// <param name="reader">The <see cref="TypefaceReader"/>.</param>
        internal IndexDataOfFontDictionary(TypefaceReader reader)
            : base(reader)
        {
        }
    }
}
