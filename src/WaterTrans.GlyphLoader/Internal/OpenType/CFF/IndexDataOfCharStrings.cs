// <copyright file="IndexDataOfCharStrings.cs" company="WaterTrans">
// © 2020 WaterTrans
// </copyright>

using System.Collections.Generic;

namespace WaterTrans.GlyphLoader.Internal.OpenType.CFF
{
    /// <summary>
    /// The Compact FontFormat Specification CharStrings INDEX.
    /// </summary>
    internal class IndexDataOfCharStrings : IndexData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IndexDataOfCharStrings"/> class.
        /// </summary>
        /// <param name="reader">The <see cref="TypefaceReader"/>.</param>
        internal IndexDataOfCharStrings(TypefaceReader reader)
            : base(reader)
        {
        }
    }
}
