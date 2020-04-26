// <copyright file="IndexDataOfString.cs" company="WaterTrans">
// © 2020 WaterTrans
// </copyright>

using System.Collections.Generic;

namespace WaterTrans.GlyphLoader.Internal.OpenType.CFF
{
    /// <summary>
    /// The Compact FontFormat Specification String INDEX.
    /// </summary>
    internal class IndexDataOfString : IndexData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IndexDataOfString"/> class.
        /// </summary>
        /// <param name="reader">The <see cref="TypefaceReader"/>.</param>
        internal IndexDataOfString(TypefaceReader reader)
            : base(reader)
        {
            for (int i = 0; i < Objects.Count; i++)
            {
                Strings.Add(System.Text.Encoding.UTF8.GetString(Objects[i], 0, Objects[i].Length));
            }
        }

        /// <summary>
        /// Gets a list of String.
        /// </summary>
        public List<string> Strings { get; } = new List<string>(StandardStrings.Values);
    }
}
