// <copyright file="IndexDataOfTopDictionary.cs" company="WaterTrans">
// © 2020 WaterTrans
// </copyright>

using System.Collections.Generic;

namespace WaterTrans.GlyphLoader.Internal.OpenType.CFF
{
    /// <summary>
    /// The Compact FontFormat Specification Top DICT INDEX.
    /// </summary>
    internal class IndexDataOfTopDictionary : IndexData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IndexDataOfTopDictionary"/> class.
        /// </summary>
        /// <param name="reader">The <see cref="TypefaceReader"/>.</param>
        internal IndexDataOfTopDictionary(TypefaceReader reader)
            : base(reader)
        {
        }
    }
}
