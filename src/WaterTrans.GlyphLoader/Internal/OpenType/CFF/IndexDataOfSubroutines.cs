// <copyright file="IndexDataOfSubroutines.cs" company="WaterTrans">
// © 2020 WaterTrans
// </copyright>

namespace WaterTrans.GlyphLoader.Internal.OpenType.CFF
{
    /// <summary>
    /// The Compact FontFormat Specification Local / Global Subrs INDEX.
    /// </summary>
    internal class IndexDataOfSubroutines : IndexData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IndexDataOfSubroutines"/> class.
        /// </summary>
        /// <param name="reader">The <see cref="TypefaceReader"/>.</param>
        internal IndexDataOfSubroutines(TypefaceReader reader)
            : base(reader)
        {
        }
    }
}
