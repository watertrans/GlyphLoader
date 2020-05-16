// <copyright file="LigatureGlyphMapComparer.cs" company="WaterTrans">
// © 2020 WaterTrans
// </copyright>

using System.Collections.Generic;
using System.Linq;

namespace WaterTrans.GlyphLoader.Internal.SFNT
{
    /// <summary>
    /// The ligature glyph map dictionary equality comparer.
    /// </summary>
    internal class LigatureGlyphMapComparer : IEqualityComparer<ushort[]>
    {
        /// <inheritdoc/>
        public bool Equals(ushort[] x, ushort[] y)
        {
            return x.SequenceEqual(y);
        }

        /// <inheritdoc/>
        public int GetHashCode(ushort[] obj)
        {
            return obj.Aggregate(0, (acc, i) => unchecked((acc * 457) + (i * 389)));
        }
    }
}
