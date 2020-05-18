// <copyright file="GlyphIndexArrayComparer.cs" company="WaterTrans">
// © 2020 WaterTrans
// </copyright>

using System.Collections.Generic;
using System.Linq;

namespace WaterTrans.GlyphLoader.Internal.SFNT
{
    /// <summary>
    /// The glyph index array equality comparer.
    /// </summary>
    internal class GlyphIndexArrayComparer : IEqualityComparer<ushort[]>
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
