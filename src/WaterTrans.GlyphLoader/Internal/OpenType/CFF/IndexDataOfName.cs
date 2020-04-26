// <copyright file="IndexDataOfName.cs" company="WaterTrans">
// © 2020 WaterTrans
// </copyright>

using System.Collections.Generic;

namespace WaterTrans.GlyphLoader.Internal.OpenType.CFF
{
    /// <summary>
    /// The Compact FontFormat Specification Name INDEX.
    /// </summary>
    internal class IndexDataOfName : IndexData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IndexDataOfName"/> class.
        /// </summary>
        /// <param name="reader">The <see cref="TypefaceReader"/>.</param>
        internal IndexDataOfName(TypefaceReader reader)
            : base(reader)
        {
            for (int i = 0; i < Objects.Count; i++)
            {
                Names.Add(System.Text.Encoding.UTF8.GetString(Objects[i], 0, Objects[i].Length));
            }
        }

        /// <summary>
        /// Gets a list of name.
        /// </summary>
        public List<string> Names { get; } = new List<string>();
    }
}
